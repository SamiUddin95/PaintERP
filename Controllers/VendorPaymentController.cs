using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Helpers;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;
using PaintERP.Services;

namespace PaintERP.Controllers;

public class VendorPaymentController : Controller
{
    private readonly PaintErpDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IAccountingService _accountingService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<VendorPaymentController> _logger;

    public VendorPaymentController(
        PaintErpDbContext context,
        IValidationService validationService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService,
        ILogger<VendorPaymentController> logger)
    {
        _context = context;
        _validationService = validationService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _logger = logger;
    }

    // GET: VendorPayment
    public async Task<IActionResult> Index()
    {
        var payments = await _context.VendorPayments
            .Include(vp => vp.Vendor)
            .OrderByDescending(vp => vp.PaymentDate)
            .ToListAsync();

        return View(payments);
    }

    // GET: VendorPayment/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var payment = await _context.VendorPayments
            .Include(vp => vp.Vendor)
            .Include(vp => vp.VendorPaymentBills)
                .ThenInclude(vpb => vpb.Bill)
            .FirstOrDefaultAsync(vp => vp.Id == id);

        if (payment == null)
        {
            return NotFound();
        }

        return View(payment);
    }

    // GET: VendorPayment/Create
    public async Task<IActionResult> Create()
    {
        var model = new VendorPaymentFormViewModel
        {
            PaymentNumber = await GeneratePaymentNumber(),
            PaymentDate = DateTime.UtcNow,
            Status = "Draft",
            VendorPaymentBills = new List<VendorPaymentBillViewModel>()
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    // POST: VendorPayment/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VendorPaymentFormViewModel model, CancellationToken cancellationToken)
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

            var payment = new VendorPayment
            {
                CompanyId = model.CompanyId,
                VendorId = model.VendorId,
                PaymentNumber = model.PaymentNumber,
                PaymentDate = model.PaymentDate,
                BankAccount = model.BankAccount,
                PaymentMethod = model.PaymentMethod,
                ReferenceNumber = model.ReferenceNumber,
                TotalPaymentAmount = model.TotalPaymentAmount,
                TotalApplied = model.TotalApplied,
                UnappliedAmount = model.UnappliedAmount,
                Status = model.Status,
                IsPrinted = false,
                IsReconciled = false,
                Notes = model.Notes,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name ?? "System",
                UpdatedBy = User.Identity?.Name ?? "System"
            };

            foreach (var bill in model.VendorPaymentBills.Where(vpb => vpb.PaymentAmount > 0))
            {
                payment.VendorPaymentBills.Add(new VendorPaymentBill
                {
                    BillId = bill.BillId,
                    BillAmount = bill.BillAmount,
                    AmountDue = bill.AmountDue,
                    PaymentAmount = bill.PaymentAmount,
                    RemainingBalance = bill.RemainingBalance
                });
            }

            // Step 2: Business Validation
            var validationResult = await _validationService.ValidateVendorPaymentAsync(payment, cancellationToken);
            
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
                    _context.VendorPayments.Add(payment);
                    await _context.SaveChangesAsync(cancellationToken);

                    // Update bill balances
                    foreach (var bill in payment.VendorPaymentBills)
                    {
                        var billEntity = await _context.Bills.FindAsync(bill.BillId, cancellationToken);
                        if (billEntity != null)
                        {
                            billEntity.AmountPaid += bill.PaymentAmount;
                        }
                    }

                    // Create accounting entry
                    var journalEntry = await _accountingService.CreateVendorPaymentEntryAsync(payment, cancellationToken);
                    
                    if (journalEntry == null)
                    {
                        return TransactionResult.FailureResult(
                            "Failed to create accounting entries",
                            "Accounting entry creation failed. Transaction will be rolled back."
                        );
                    }

                    _logger.LogInformation(
                        "Vendor Payment {PaymentNumber} created successfully with Journal Entry {EntryNumber}",
                        payment.PaymentNumber,
                        journalEntry.EntryNumber
                    );

                    return TransactionResult.SuccessResult(
                        $"Vendor Payment {payment.PaymentNumber} created successfully",
                        new { PaymentId = payment.Id, JournalEntryId = journalEntry.Id }
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating vendor payment: {Message}", ex.Message);
                    return TransactionResult.FailureResult(
                        "An error occurred while creating the vendor payment",
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

    // GET: VendorPayment/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var payment = await _context.VendorPayments
            .Include(vp => vp.VendorPaymentBills)
            .FirstOrDefaultAsync(vp => vp.Id == id);

        if (payment == null)
        {
            return NotFound();
        }

        var model = new VendorPaymentFormViewModel
        {
            Id = payment.Id,
            CompanyId = payment.CompanyId,
            VendorId = payment.VendorId,
            PaymentNumber = payment.PaymentNumber,
            PaymentDate = payment.PaymentDate,
            BankAccount = payment.BankAccount,
            PaymentMethod = payment.PaymentMethod,
            ReferenceNumber = payment.ReferenceNumber,
            TotalPaymentAmount = payment.TotalPaymentAmount,
            TotalApplied = payment.TotalApplied,
            UnappliedAmount = payment.UnappliedAmount,
            Status = payment.Status,
            Notes = payment.Notes,
            VendorPaymentBills = payment.VendorPaymentBills.Select(vpb => new VendorPaymentBillViewModel
            {
                Id = vpb.Id,
                VendorPaymentId = vpb.VendorPaymentId,
                BillId = vpb.BillId,
                BillNumber = vpb.Bill?.BillNumber ?? "",
                BillAmount = vpb.BillAmount,
                AmountDue = vpb.AmountDue,
                PaymentAmount = vpb.PaymentAmount,
                RemainingBalance = vpb.RemainingBalance,
                IsSelected = vpb.PaymentAmount > 0
            }).ToList()
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    // POST: VendorPayment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VendorPaymentFormViewModel model)
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

        var payment = await _context.VendorPayments
            .Include(vp => vp.VendorPaymentBills)
            .FirstOrDefaultAsync(vp => vp.Id == id);

        if (payment == null)
        {
            return NotFound();
        }

        // Revert previous bill payments
        foreach (var existingBill in payment.VendorPaymentBills)
        {
            var billEntity = await _context.Bills.FindAsync(existingBill.BillId);
            if (billEntity != null)
            {
                billEntity.AmountPaid -= existingBill.PaymentAmount;
            }
        }

        payment.VendorId = model.VendorId;
        payment.PaymentDate = model.PaymentDate;
        payment.BankAccount = model.BankAccount;
        payment.PaymentMethod = model.PaymentMethod;
        payment.ReferenceNumber = model.ReferenceNumber;
        payment.TotalPaymentAmount = model.TotalPaymentAmount;
        payment.TotalApplied = model.TotalApplied;
        payment.UnappliedAmount = model.UnappliedAmount;
        payment.Status = model.Status;
        payment.Notes = model.Notes;
        payment.UpdatedAtUtc = DateTime.UtcNow;
        payment.UpdatedBy = User.Identity?.Name ?? "System";

        if (model.Status == "Reconciled" && !payment.IsReconciled)
        {
            payment.IsReconciled = true;
            payment.ReconciledDate = DateTime.UtcNow;
        }

        // Remove existing bill payments
        var existingBills = payment.VendorPaymentBills.ToList();
        _context.VendorPaymentBills.RemoveRange(existingBills);

        // Add new bill payments
        foreach (var bill in model.VendorPaymentBills.Where(vpb => vpb.PaymentAmount > 0))
        {
            payment.VendorPaymentBills.Add(new VendorPaymentBill
            {
                BillId = bill.BillId,
                BillAmount = bill.BillAmount,
                AmountDue = bill.AmountDue,
                PaymentAmount = bill.PaymentAmount,
                RemainingBalance = bill.RemainingBalance
            });

            // Update the bill's AmountPaid
            var billEntity = await _context.Bills.FindAsync(bill.BillId);
            if (billEntity != null)
            {
                billEntity.AmountPaid += bill.PaymentAmount;
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: VendorPayment/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var payment = await _context.VendorPayments
            .Include(vp => vp.Vendor)
            .FirstOrDefaultAsync(vp => vp.Id == id);

        if (payment == null)
        {
            return NotFound();
        }

        return View(payment);
    }

    // POST: VendorPayment/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        try
        {
            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                var payment = await _context.VendorPayments
                    .Include(vp => vp.VendorPaymentBills)
                    .FirstOrDefaultAsync(vp => vp.Id == id, cancellationToken);

                if (payment == null)
                {
                    return TransactionResult.FailureResult("Vendor payment not found");
                }

                // Find associated journal entry
                var journalEntry = await _context.JournalEntries
                    .FirstOrDefaultAsync(je => je.TransactionType == "Vendor Payment" && je.ReferenceId == id, cancellationToken);

                // Revert bill payments
                foreach (var billPayment in payment.VendorPaymentBills)
                {
                    var billEntity = await _context.Bills.FindAsync(billPayment.BillId, cancellationToken);
                    if (billEntity != null)
                    {
                        billEntity.AmountPaid -= billPayment.PaymentAmount;
                    }
                }

                // Reverse journal entry
                if (journalEntry != null)
                {
                    await _accountingService.ReverseJournalEntryAsync(
                        journalEntry.Id,
                        $"Vendor Payment {payment.PaymentNumber} deleted",
                        cancellationToken
                    );
                }

                // Delete payment
                _context.VendorPayments.Remove(payment);
                await _context.SaveChangesAsync(cancellationToken);

                return TransactionResult.SuccessResult($"Vendor Payment {payment.PaymentNumber} deleted successfully");
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
            _logger.LogError(ex, "Error deleting vendor payment: {Message}", ex.Message);
            return this.WithError("An error occurred while deleting the vendor payment", "Index");
        }
    }

    // POST: VendorPayment/Print/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Print(int id)
    {
        var payment = await _context.VendorPayments.FindAsync(id);
        if (payment == null)
        {
            return NotFound();
        }

        payment.IsPrinted = true;
        payment.UpdatedAtUtc = DateTime.UtcNow;
        payment.UpdatedBy = User.Identity?.Name ?? "System";

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Edit), new { id });
    }

    // POST: VendorPayment/Reconcile/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reconcile(int id)
    {
        var payment = await _context.VendorPayments.FindAsync(id);
        if (payment == null)
        {
            return NotFound();
        }

        payment.Status = "Reconciled";
        payment.IsReconciled = true;
        payment.ReconciledDate = DateTime.UtcNow;
        payment.UpdatedAtUtc = DateTime.UtcNow;
        payment.UpdatedBy = User.Identity?.Name ?? "System";

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Edit), new { id });
    }

    // GET: VendorPayment/GetVendorBills
    [HttpGet]
    public async Task<IActionResult> GetVendorBills(int vendorId)
    {
        var bills = await _context.Bills
            .Where(b => b.VendorId == vendorId && b.Status != "Paid")
            .Select(b => new VendorPaymentBillListItem
            {
                Id = b.Id,
                BillNumber = b.BillNumber,
                BillDate = b.BillDate,
                BillAmount = b.GrandTotal,
                AmountDue = b.GrandTotal - b.AmountPaid
            })
            .ToListAsync();

        return Json(bills);
    }

    private async Task<string> GeneratePaymentNumber()
    {
        var lastPayment = await _context.VendorPayments
            .OrderByDescending(vp => vp.Id)
            .FirstOrDefaultAsync();

        if (lastPayment == null)
        {
            return "VP-0001";
        }

        var lastNumber = lastPayment.PaymentNumber.Replace("VP-", "");
        if (int.TryParse(lastNumber, out int number))
        {
            return $"VP-{(number + 1):D4}";
        }

        return "VP-0001";
    }

    private async Task PopulateDropdowns(VendorPaymentFormViewModel model)
    {
        model.Vendors = await _context.Vendors
            .OrderBy(v => v.BusinessName)
            .Select(v => new VendorPaymentVendorListItem { Id = v.Id, BusinessName = v.BusinessName })
            .ToListAsync();
    }
}
