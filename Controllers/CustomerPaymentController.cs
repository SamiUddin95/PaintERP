using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Helpers;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;
using PaintERP.Services;

namespace PaintERP.Controllers;

public class CustomerPaymentController : Controller
{
    private readonly PaintErpDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IAccountingService _accountingService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<CustomerPaymentController> _logger;

    public CustomerPaymentController(
        PaintErpDbContext context,
        IValidationService validationService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService,
        ILogger<CustomerPaymentController> logger)
    {
        _context = context;
        _validationService = validationService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _logger = logger;
    }

    // GET: CustomerPayment
    public async Task<IActionResult> Index()
    {
        var payments = await _context.CustomerPayments
            .Include(cp => cp.Customer)
            .OrderByDescending(cp => cp.PaymentDate)
            .ToListAsync();

        return View(payments);
    }

    // GET: CustomerPayment/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var payment = await _context.CustomerPayments
            .Include(cp => cp.Customer)
            .Include(cp => cp.CustomerPaymentInvoices)
                .ThenInclude(cpi => cpi.SalesInvoice)
            .FirstOrDefaultAsync(cp => cp.Id == id);

        if (payment == null)
        {
            return NotFound();
        }

        return View(payment);
    }

    // GET: CustomerPayment/Create
    public async Task<IActionResult> Create()
    {
        var model = new CustomerPaymentFormViewModel
        {
            PaymentNumber = await GeneratePaymentNumber(),
            PaymentDate = DateTime.UtcNow,
            Status = "Draft",
            CustomerPaymentInvoices = new List<CustomerPaymentInvoiceViewModel>()
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    // POST: CustomerPayment/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerPaymentFormViewModel model, CancellationToken cancellationToken)
    {
        try
        {
            // Step 1: Validate ModelState
            if (!ModelState.IsValid)
            {
                this.AddErrorMessage("Please correct the validation errors");
                await PopulateDropdowns(model);
                return View(model);
            }

            var payment = new CustomerPayment
            {
                CompanyId = model.CompanyId,
                CustomerId = model.CustomerId,
                PaymentNumber = model.PaymentNumber,
                PaymentDate = model.PaymentDate,
                DepositAccount = model.DepositAccount,
                PaymentMethod = model.PaymentMethod,
                ReferenceNumber = model.ReferenceNumber,
                AmountReceived = model.AmountReceived,
                TotalApplied = model.TotalApplied,
                UnappliedAmount = model.UnappliedAmount,
                Status = model.Status,
                IsDeposited = false,
                Notes = model.Notes,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name ?? "System",
                UpdatedBy = User.Identity?.Name ?? "System"
            };

            foreach (var invoice in model.CustomerPaymentInvoices.Where(cpi => cpi.PaymentApplied > 0))
            {
                payment.CustomerPaymentInvoices.Add(new CustomerPaymentInvoice
                {
                    SalesInvoiceId = invoice.SalesInvoiceId,
                    InvoiceAmount = invoice.InvoiceAmount,
                    AmountDue = invoice.AmountDue,
                    PaymentApplied = invoice.PaymentApplied,
                    RemainingBalance = invoice.RemainingBalance
                });
            }

            // Step 2: Business Validation
            var validationResult = await _validationService.ValidateCustomerPaymentAsync(payment, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                _notificationService.AddValidationErrors(validationResult);
                await PopulateDropdowns(model);
                return this.WithValidationErrors(validationResult);
            }

            // Display warnings if any
            foreach (var warning in validationResult.Warnings)
            {
                this.AddWarningMessage(warning);
            }

            // Step 3: Execute in Transaction (ensures atomicity)
            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                try
                {
                    // Save payment
                    _context.CustomerPayments.Add(payment);
                    await _context.SaveChangesAsync(cancellationToken);

                    // Update invoice balances
                    foreach (var invoice in payment.CustomerPaymentInvoices)
                    {
                        var invoiceEntity = await _context.SalesInvoices.FindAsync(invoice.SalesInvoiceId, cancellationToken);
                        if (invoiceEntity != null)
                        {
                            invoiceEntity.AmountPaid += invoice.PaymentApplied;
                            invoiceEntity.BalanceDue = invoiceEntity.GrandTotal - invoiceEntity.AmountPaid;
                        }
                    }

                    // Create accounting entry
                    var journalEntry = await _accountingService.CreateCustomerPaymentEntryAsync(payment, cancellationToken);
                    
                    if (journalEntry == null)
                    {
                        return TransactionResult.FailureResult(
                            "Failed to create accounting entries",
                            "Accounting entry creation failed. Transaction will be rolled back."
                        );
                    }

                    _logger.LogInformation(
                        "Customer Payment {PaymentNumber} created successfully with Journal Entry {EntryNumber}",
                        payment.PaymentNumber,
                        journalEntry.EntryNumber
                    );

                    return TransactionResult.SuccessResult(
                        $"Customer Payment {payment.PaymentNumber} created successfully",
                        new { PaymentId = payment.Id, JournalEntryId = journalEntry.Id }
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating customer payment: {Message}", ex.Message);
                    return TransactionResult.FailureResult(
                        "An error occurred while creating the customer payment",
                        ex.Message
                    );
                }
            }, cancellationToken);

            // Step 4: Handle Transaction Result
            if (transactionResult.Success)
            {
                return this.WithSuccess(transactionResult.Message, "Index");
            }
            else
            {
                foreach (var error in transactionResult.Errors)
                {
                    this.AddErrorMessage(error);
                }
                await PopulateDropdowns(model);
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in Create action: {Message}", ex.Message);
            await PopulateDropdowns(model);
            return this.WithError("An unexpected error occurred. Please try again.", "Index");
        }
    }

    // GET: CustomerPayment/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var payment = await _context.CustomerPayments
            .Include(cp => cp.CustomerPaymentInvoices)
            .FirstOrDefaultAsync(cp => cp.Id == id);

        if (payment == null)
        {
            return NotFound();
        }

        var model = new CustomerPaymentFormViewModel
        {
            Id = payment.Id,
            CompanyId = payment.CompanyId,
            CustomerId = payment.CustomerId,
            PaymentNumber = payment.PaymentNumber,
            PaymentDate = payment.PaymentDate,
            DepositAccount = payment.DepositAccount,
            PaymentMethod = payment.PaymentMethod,
            ReferenceNumber = payment.ReferenceNumber,
            AmountReceived = payment.AmountReceived,
            TotalApplied = payment.TotalApplied,
            UnappliedAmount = payment.UnappliedAmount,
            Status = payment.Status,
            Notes = payment.Notes,
            CustomerPaymentInvoices = payment.CustomerPaymentInvoices.Select(cpi => new CustomerPaymentInvoiceViewModel
            {
                Id = cpi.Id,
                CustomerPaymentId = cpi.CustomerPaymentId,
                SalesInvoiceId = cpi.SalesInvoiceId,
                InvoiceNumber = cpi.SalesInvoice?.InvoiceNumber ?? "",
                InvoiceAmount = cpi.InvoiceAmount,
                AmountDue = cpi.AmountDue,
                PaymentApplied = cpi.PaymentApplied,
                RemainingBalance = cpi.RemainingBalance,
                IsSelected = cpi.PaymentApplied > 0
            }).ToList()
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    // POST: CustomerPayment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CustomerPaymentFormViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model);
            return View(model);
        }

        var payment = await _context.CustomerPayments
            .Include(cp => cp.CustomerPaymentInvoices)
            .FirstOrDefaultAsync(cp => cp.Id == id);

        if (payment == null)
        {
            return NotFound();
        }

        // Revert previous invoice payments
        foreach (var existingInvoice in payment.CustomerPaymentInvoices)
        {
            var invoiceEntity = await _context.SalesInvoices.FindAsync(existingInvoice.SalesInvoiceId);
            if (invoiceEntity != null)
            {
                invoiceEntity.AmountPaid -= existingInvoice.PaymentApplied;
                invoiceEntity.BalanceDue = invoiceEntity.GrandTotal - invoiceEntity.AmountPaid;
            }
        }

        payment.CustomerId = model.CustomerId;
        payment.PaymentDate = model.PaymentDate;
        payment.DepositAccount = model.DepositAccount;
        payment.PaymentMethod = model.PaymentMethod;
        payment.ReferenceNumber = model.ReferenceNumber;
        payment.AmountReceived = model.AmountReceived;
        payment.TotalApplied = model.TotalApplied;
        payment.UnappliedAmount = model.UnappliedAmount;
        payment.Status = model.Status;
        payment.Notes = model.Notes;
        payment.UpdatedAtUtc = DateTime.UtcNow;
        payment.UpdatedBy = User.Identity?.Name ?? "System";

        if (model.Status == "Deposited" && !payment.IsDeposited)
        {
            payment.IsDeposited = true;
            payment.DepositedDate = DateTime.UtcNow;
        }

        // Remove existing invoice payments
        var existingInvoices = payment.CustomerPaymentInvoices.ToList();
        _context.CustomerPaymentInvoices.RemoveRange(existingInvoices);

        // Add new invoice payments
        foreach (var invoice in model.CustomerPaymentInvoices.Where(cpi => cpi.PaymentApplied > 0))
        {
            payment.CustomerPaymentInvoices.Add(new CustomerPaymentInvoice
            {
                SalesInvoiceId = invoice.SalesInvoiceId,
                InvoiceAmount = invoice.InvoiceAmount,
                AmountDue = invoice.AmountDue,
                PaymentApplied = invoice.PaymentApplied,
                RemainingBalance = invoice.RemainingBalance
            });

            // Update the invoice's AmountPaid
            var invoiceEntity = await _context.SalesInvoices.FindAsync(invoice.SalesInvoiceId);
            if (invoiceEntity != null)
            {
                invoiceEntity.AmountPaid += invoice.PaymentApplied;
                invoiceEntity.BalanceDue = invoiceEntity.GrandTotal - invoiceEntity.AmountPaid;
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: CustomerPayment/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var payment = await _context.CustomerPayments
            .Include(cp => cp.Customer)
            .FirstOrDefaultAsync(cp => cp.Id == id);

        if (payment == null)
        {
            return NotFound();
        }

        return View(payment);
    }

    // POST: CustomerPayment/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        try
        {
            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                var payment = await _context.CustomerPayments
                    .Include(cp => cp.CustomerPaymentInvoices)
                    .FirstOrDefaultAsync(cp => cp.Id == id, cancellationToken);

                if (payment == null)
                {
                    return TransactionResult.FailureResult("Customer payment not found");
                }

                // Find associated journal entry
                var journalEntry = await _context.JournalEntries
                    .FirstOrDefaultAsync(je => je.TransactionType == "Customer Payment" && je.ReferenceId == id, cancellationToken);

                // Revert invoice payments
                foreach (var invoicePayment in payment.CustomerPaymentInvoices)
                {
                    var invoiceEntity = await _context.SalesInvoices.FindAsync(invoicePayment.SalesInvoiceId, cancellationToken);
                    if (invoiceEntity != null)
                    {
                        invoiceEntity.AmountPaid -= invoicePayment.PaymentApplied;
                        invoiceEntity.BalanceDue = invoiceEntity.GrandTotal - invoiceEntity.AmountPaid;
                    }
                }

                // Reverse journal entry
                if (journalEntry != null)
                {
                    await _accountingService.ReverseJournalEntryAsync(
                        journalEntry.Id,
                        $"Customer Payment {payment.PaymentNumber} deleted",
                        cancellationToken
                    );
                }

                // Delete payment
                _context.CustomerPayments.Remove(payment);
                await _context.SaveChangesAsync(cancellationToken);

                return TransactionResult.SuccessResult($"Customer Payment {payment.PaymentNumber} deleted successfully");
            }, cancellationToken);

            if (transactionResult.Success)
            {
                return this.WithSuccess(transactionResult.Message, "Index");
            }
            else
            {
                return this.WithError(transactionResult.Message, "Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer payment: {Message}", ex.Message);
            return this.WithError("An error occurred while deleting the customer payment", "Index");
        }
    }

    // POST: CustomerPayment/Deposit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deposit(int id)
    {
        var payment = await _context.CustomerPayments.FindAsync(id);
        if (payment == null)
        {
            return NotFound();
        }

        payment.Status = "Deposited";
        payment.IsDeposited = true;
        payment.DepositedDate = DateTime.UtcNow;
        payment.UpdatedAtUtc = DateTime.UtcNow;
        payment.UpdatedBy = User.Identity?.Name ?? "System";

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Edit), new { id });
    }

    // GET: CustomerPayment/GetCustomerInvoices
    [HttpGet]
    public async Task<IActionResult> GetCustomerInvoices(int customerId)
    {
        var invoices = await _context.SalesInvoices
            .Where(si => si.CustomerId == customerId && si.Status != "Paid")
            .Select(si => new CustomerPaymentInvoiceListItem
            {
                Id = si.Id,
                InvoiceNumber = si.InvoiceNumber,
                InvoiceDate = si.InvoiceDate,
                InvoiceAmount = si.GrandTotal,
                AmountDue = si.GrandTotal - si.AmountPaid
            })
            .ToListAsync();

        return Json(invoices);
    }

    private async Task<string> GeneratePaymentNumber()
    {
        var lastPayment = await _context.CustomerPayments
            .OrderByDescending(cp => cp.Id)
            .FirstOrDefaultAsync();

        if (lastPayment == null)
        {
            return "CP-0001";
        }

        var lastNumber = lastPayment.PaymentNumber.Replace("CP-", "");
        if (int.TryParse(lastNumber, out int number))
        {
            return $"CP-{(number + 1):D4}";
        }

        return "CP-0001";
    }

    private async Task PopulateDropdowns(CustomerPaymentFormViewModel model)
    {
        model.Customers = await _context.Customers
            .OrderBy(c => c.BusinessName)
            .Select(c => new CustomerPaymentCustomerListItem { Id = c.Id, BusinessName = c.BusinessName })
            .ToListAsync();
    }
}
