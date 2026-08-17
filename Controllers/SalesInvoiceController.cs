using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Helpers;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;
using PaintERP.Services;

namespace PaintERP.Controllers;

public class SalesInvoiceController : Controller
{
    private readonly PaintErpDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IInventoryService _inventoryService;
    private readonly IAccountingService _accountingService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<SalesInvoiceController> _logger;

    public SalesInvoiceController(
        PaintErpDbContext context,
        IValidationService validationService,
        IInventoryService inventoryService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService,
        ILogger<SalesInvoiceController> logger)
    {
        _context = context;
        _validationService = validationService;
        _inventoryService = inventoryService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _logger = logger;
    }

    // GET: SalesInvoice
    public async Task<IActionResult> Index(int? page, int pageSize = 10)
    {
        var pageNumber = page ?? 1;

        var query = _context.SalesInvoices
            .Include(si => si.Customer)
            .OrderByDescending(si => si.InvoiceDate)
            .Select(si => new SalesInvoiceListItem
            {
                Id = si.Id,
                InvoiceNumber = si.InvoiceNumber,
                CustomerName = si.Customer.BusinessName,
                InvoiceDate = si.InvoiceDate,
                DueDate = si.DueDate,
                GrandTotal = si.GrandTotal,
                BalanceDue = si.BalanceDue,
                Status = si.Status,
                IsPaid = si.IsPaid
            });

        var invoices = await PaginatedList<SalesInvoiceListItem>.CreateAsync(query, pageNumber, pageSize);

        return View(invoices);
    }

    // GET: SalesInvoice/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var invoice = await _context.SalesInvoices
            .Include(si => si.Customer)
            .Include(si => si.Warehouse)
            .Include(si => si.SalesInvoiceItems)
                .ThenInclude(sii => sii.PaintItem)
            .FirstOrDefaultAsync(si => si.Id == id);

        if (invoice == null)
        {
            return NotFound();
        }

        return View(invoice);
    }

    // GET: SalesInvoice/Create
    public async Task<IActionResult> Create()
    {
        var model = new SalesInvoiceFormViewModel
        {
            InvoiceNumber = await GenerateInvoiceNumber(),
            InvoiceDate = DateTime.UtcNow,
            Status = "Draft",
            SalesInvoiceItems = new List<SalesInvoiceItemViewModel> { new SalesInvoiceItemViewModel() }
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    // POST: SalesInvoice/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SalesInvoiceFormViewModel model, CancellationToken cancellationToken)
    {
        try
        {
            // Ensure model is not null and fully initialized
            if (model == null)
            {
                model = new SalesInvoiceFormViewModel();
            }

            // Ensure SalesInvoiceItems is not null
            if (model.SalesInvoiceItems == null || model.SalesInvoiceItems.Count == 0)
            {
                model.SalesInvoiceItems = new List<SalesInvoiceItemViewModel> { new SalesInvoiceItemViewModel() };
            }

            // Step 1: Validate ModelState
            if (!ModelState.IsValid)
            {
                this.AddErrorMessage("Please correct the validation errors");
                await PopulateDropdowns(model);
                return View(model);
            }

            // Convert ViewModel to Entity
            var invoice = new SalesInvoice
            {
                CompanyId = model.CompanyId > 0 ? model.CompanyId : 1, // Default to company 1 if not set
                CustomerId = model.CustomerId,
                InvoiceNumber = model.InvoiceNumber,
                InvoiceDate = model.InvoiceDate,
                DueDate = model.DueDate,
                Salesperson = model.Salesperson,
                WarehouseId = model.WarehouseId,
                ShippingMethod = model.ShippingMethod,
                TrackingNumber = model.TrackingNumber,
                PaymentTerms = model.PaymentTerms,
                ReferenceNumber = model.ReferenceNumber,
                Status = model.Status,
                IsPaid = false,
                IsVoided = false,
                Subtotal = model.Subtotal,
                DiscountAmount = model.DiscountAmount,
                ShippingCost = model.ShippingCost,
                SalesTaxAmount = model.SalesTaxAmount,
                GrandTotal = model.GrandTotal,
                AmountPaid = model.AmountPaid,
                BalanceDue = model.BalanceDue,
                AttachmentUrl = model.AttachmentUrl,
                InternalNotes = model.InternalNotes,
                CustomerNotes = model.CustomerNotes,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name ?? "System",
                UpdatedBy = User.Identity?.Name ?? "System"
            };

            foreach (var item in model.SalesInvoiceItems.Where(sii => sii.Quantity > 0))
            {
                invoice.SalesInvoiceItems.Add(new SalesInvoiceItem
                {
                    PaintItemId = item.PaintItemId,
                    SKU = item.SKU,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    DiscountPercent = item.DiscountPercent,
                    DiscountAmount = item.DiscountAmount,
                    TaxPercent = item.TaxPercent,
                    TaxAmount = item.TaxAmount,
                    LineTotal = item.LineTotal,
                    StockBefore = item.StockBefore,
                    StockAfter = item.StockAfter
                });
            }

            // Step 2: Business Validation
            var validationResult = await _validationService.ValidateSalesInvoiceAsync(invoice, cancellationToken);

            if (validationResult != null && !validationResult.IsValid)
            {
                // Add validation errors to ViewData for display
                ViewData["ValidationErrors"] = validationResult.Errors;
                ViewData["ValidationWarnings"] = validationResult.Warnings;
                await PopulateDropdowns(model);
                if (model.SalesInvoiceItems == null || model.SalesInvoiceItems.Count == 0)
                {
                    model.SalesInvoiceItems = new List<SalesInvoiceItemViewModel> { new SalesInvoiceItemViewModel() };
                }
                return View(model);
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
                    // Save invoice
                    _context.SalesInvoices.Add(invoice);
                    await _context.SaveChangesAsync(cancellationToken);

                    // Step 4: Process Inventory (reduce stock)
                    var inventorySuccess = await _inventoryService.ProcessSalesInvoiceInventoryAsync(invoice, cancellationToken);
                    
                    if (!inventorySuccess)
                    {
                        return TransactionResult.FailureResult(
                            "Failed to process inventory",
                            "Inventory update failed. Transaction will be rolled back."
                        );
                    }

                    // Step 5: Create Accounting Entries
                    var journalEntry = await _accountingService.CreateSalesInvoiceEntryAsync(invoice, cancellationToken);
                    
                    if (journalEntry == null)
                    {
                        return TransactionResult.FailureResult(
                            "Failed to create accounting entries",
                            "Accounting entry creation failed. Transaction will be rolled back."
                        );
                    }

                    _logger.LogInformation(
                        "Sales Invoice {InvoiceNumber} created successfully with Journal Entry {EntryNumber}",
                        invoice.InvoiceNumber,
                        journalEntry.EntryNumber
                    );

                    return TransactionResult.SuccessResult(
                        $"Sales Invoice {invoice.InvoiceNumber} created successfully",
                        new { InvoiceId = invoice.Id, JournalEntryId = journalEntry.Id }
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating sales invoice: {Message} - Inner: {InnerMessage}", ex.Message, ex.InnerException?.Message);
                    return TransactionResult.FailureResult(
                        "An error occurred while creating the invoice",
                        $"{ex.Message}. {ex.InnerException?.Message}"
                    );
                }
            }, cancellationToken);

            // Step 6: Handle Transaction Result
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
                if (model.SalesInvoiceItems == null || model.SalesInvoiceItems.Count == 0)
                {
                    model.SalesInvoiceItems = new List<SalesInvoiceItemViewModel> { new SalesInvoiceItemViewModel() };
                }
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in Create action: {Message}", ex.Message);
            await PopulateDropdowns(model);
            if (model.SalesInvoiceItems == null || model.SalesInvoiceItems.Count == 0)
            {
                model.SalesInvoiceItems = new List<SalesInvoiceItemViewModel> { new SalesInvoiceItemViewModel() };
            }
            return this.WithError("An unexpected error occurred. Please try again.", "Index");
        }
    }

    // GET: SalesInvoice/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var invoice = await _context.SalesInvoices
            .Include(si => si.SalesInvoiceItems)
            .FirstOrDefaultAsync(si => si.Id == id);

        if (invoice == null)
        {
            return NotFound();
        }

        // Load payment history separately
        var paymentInvoices = await _context.CustomerPaymentInvoices
            .Include(cpi => cpi.CustomerPayment)
            .Where(cpi => cpi.SalesInvoiceId == id)
            .ToListAsync();

        // Check and update overdue status
        await UpdateInvoiceStatusAsync(invoice);

        var model = new SalesInvoiceFormViewModel
        {
            Id = invoice.Id,
            CompanyId = invoice.CompanyId,
            CustomerId = invoice.CustomerId,
            InvoiceNumber = invoice.InvoiceNumber,
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            Salesperson = invoice.Salesperson,
            WarehouseId = invoice.WarehouseId,
            ShippingMethod = invoice.ShippingMethod,
            TrackingNumber = invoice.TrackingNumber,
            PaymentTerms = invoice.PaymentTerms,
            ReferenceNumber = invoice.ReferenceNumber,
            Status = invoice.Status,
            IsPaid = invoice.IsPaid,
            IsVoided = invoice.IsVoided,
            PaidDate = invoice.PaidDate,
            VoidedDate = invoice.VoidedDate,
            Subtotal = invoice.Subtotal,
            DiscountAmount = invoice.DiscountAmount,
            ShippingCost = invoice.ShippingCost,
            SalesTaxAmount = invoice.SalesTaxAmount,
            GrandTotal = invoice.GrandTotal,
            AmountPaid = invoice.AmountPaid,
            BalanceDue = invoice.BalanceDue,
            AttachmentUrl = invoice.AttachmentUrl,
            InternalNotes = invoice.InternalNotes,
            CustomerNotes = invoice.CustomerNotes,
            CustomerPaymentInvoices = paymentInvoices,
            SalesInvoiceItems = invoice.SalesInvoiceItems.Select(sii => new SalesInvoiceItemViewModel
            {
                Id = sii.Id,
                SalesInvoiceId = sii.SalesInvoiceId,
                SKU = sii.SKU,
                Description = sii.Description,
                PaintItemId = sii.PaintItemId,
                Quantity = sii.Quantity,
                Unit = sii.Unit,
                UnitPrice = sii.UnitPrice,
                DiscountPercent = sii.DiscountPercent,
                DiscountAmount = sii.DiscountAmount,
                TaxPercent = sii.TaxPercent,
                TaxAmount = sii.TaxAmount,
                LineTotal = sii.LineTotal,
                StockBefore = sii.StockBefore,
                StockAfter = sii.StockAfter
            }).ToList()
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    // POST: SalesInvoice/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SalesInvoiceFormViewModel model, CancellationToken cancellationToken)
    {
        try
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

            var invoice = await _context.SalesInvoices
                .Include(si => si.SalesInvoiceItems)
                .FirstOrDefaultAsync(si => si.Id == id, cancellationToken);

            if (invoice == null)
            {
                return NotFound();
            }

            // Prevent editing of posted invoices (would require reversal)
            if (invoice.Status != "Draft" && invoice.Status != "Pending")
            {
                this.AddWarningMessage("Editing posted invoices is restricted. Changes may not update inventory or accounting entries.");
            }

            invoice.CustomerId = model.CustomerId;
            invoice.InvoiceDate = model.InvoiceDate;
            invoice.DueDate = model.DueDate;
            invoice.Salesperson = model.Salesperson;
            invoice.WarehouseId = model.WarehouseId;
            invoice.ShippingMethod = model.ShippingMethod;
            invoice.TrackingNumber = model.TrackingNumber;
            invoice.PaymentTerms = model.PaymentTerms;
            invoice.ReferenceNumber = model.ReferenceNumber;
            invoice.Status = model.Status;
            invoice.Subtotal = model.Subtotal;
            invoice.DiscountAmount = model.DiscountAmount;
            invoice.ShippingCost = model.ShippingCost;
            invoice.SalesTaxAmount = model.SalesTaxAmount;
            invoice.GrandTotal = model.GrandTotal;
            invoice.AmountPaid = model.AmountPaid;
            invoice.BalanceDue = model.BalanceDue;
            invoice.AttachmentUrl = model.AttachmentUrl;
            invoice.InternalNotes = model.InternalNotes;
            invoice.CustomerNotes = model.CustomerNotes;
            invoice.UpdatedAtUtc = DateTime.UtcNow;
            invoice.UpdatedBy = User.Identity?.Name ?? "System";

            if (model.Status == "Paid" && !invoice.IsPaid)
            {
                invoice.IsPaid = true;
                invoice.PaidDate = DateTime.UtcNow;
            }

            if (model.Status == "Void" && !invoice.IsVoided)
            {
                invoice.IsVoided = true;
                invoice.VoidedDate = DateTime.UtcNow;
            }

            var existingItems = invoice.SalesInvoiceItems.ToList();
            _context.SalesInvoiceItems.RemoveRange(existingItems);

            foreach (var item in model.SalesInvoiceItems.Where(sii => sii.Quantity > 0))
            {
                invoice.SalesInvoiceItems.Add(new SalesInvoiceItem
                {
                    PaintItemId = item.PaintItemId,
                    SKU = item.SKU,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    UnitPrice = item.UnitPrice,
                    DiscountPercent = item.DiscountPercent,
                    DiscountAmount = item.DiscountAmount,
                    TaxPercent = item.TaxPercent,
                    TaxAmount = item.TaxAmount,
                    LineTotal = item.LineTotal,
                    StockBefore = item.StockBefore,
                    StockAfter = item.StockAfter
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Sales Invoice {InvoiceNumber} updated", invoice.InvoiceNumber);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating invoice: {Message}", ex.Message);
            await PopulateDropdowns(model);
            return this.WithError("An error occurred while updating the invoice", "Edit");
        }
    }

    // GET: SalesInvoice/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var invoice = await _context.SalesInvoices
            .Include(si => si.Customer)
            .FirstOrDefaultAsync(si => si.Id == id);

        if (invoice == null)
        {
            return NotFound();
        }

        return View(invoice);
    }

    // POST: SalesInvoice/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        try
        {
            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                var invoice = await _context.SalesInvoices
                    .Include(si => si.SalesInvoiceItems)
                    .FirstOrDefaultAsync(si => si.Id == id, cancellationToken);

                if (invoice == null)
                {
                    return TransactionResult.FailureResult("Invoice not found");
                }

                // Find associated journal entry
                var journalEntry = await _context.JournalEntries
                    .FirstOrDefaultAsync(je => je.TransactionType == "Sales Invoice" && je.ReferenceId == id, cancellationToken);

                // Reverse inventory movements
                if (invoice.SalesInvoiceItems != null && invoice.SalesInvoiceItems.Any())
                {
                    foreach (var item in invoice.SalesInvoiceItems)
                    {
                        await _inventoryService.IncreaseStockAsync(
                            Convert.ToInt32(item.PaintItemId),
                            invoice.WarehouseId ?? 0,
                            item.Quantity,
                            "Sales Invoice Reversal",
                            invoice.Id,
                            $"Reversal of Invoice: {invoice.InvoiceNumber}",
                            cancellationToken
                        );
                    }
                }

                // Reverse journal entry
                if (journalEntry != null)
                {
                    await _accountingService.ReverseJournalEntryAsync(
                        journalEntry.Id,
                        $"Invoice {invoice.InvoiceNumber} deleted",
                        cancellationToken
                    );
                }

                // Delete invoice
                _context.SalesInvoices.Remove(invoice);
                await _context.SaveChangesAsync(cancellationToken);

                return TransactionResult.SuccessResult($"Invoice {invoice.InvoiceNumber} deleted successfully");
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
            _logger.LogError(ex, "Error deleting invoice: {Message}", ex.Message);
            return this.WithError("An error occurred while deleting the invoice", "Index");
        }
    }

    // POST: SalesInvoice/ReceivePayment/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReceivePayment(int id, decimal paymentAmount, string paymentMethod, string paymentReference)
    {
        var invoice = await _context.SalesInvoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        // Validation: Cannot receive payment for voided invoices
        if (invoice.IsVoided)
        {
            this.AddErrorMessage("Cannot receive payment for a voided invoice");
            return RedirectToAction(nameof(Edit), new { id });
        }

        // Validation: Payment amount must be positive
        if (paymentAmount <= 0)
        {
            this.AddErrorMessage("Payment amount must be greater than zero");
            return RedirectToAction(nameof(Edit), new { id });
        }

        // Validation: Payment cannot exceed balance due
        if (paymentAmount > invoice.BalanceDue)
        {
            this.AddErrorMessage($"Payment amount cannot exceed balance due of {invoice.BalanceDue:C}");
            return RedirectToAction(nameof(Edit), new { id });
        }

        // Create CustomerPayment record
        var paymentNumber = $"PAY-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}";
        var customerPayment = new CustomerPayment
        {
            CompanyId = invoice.CompanyId,
            CustomerId = invoice.CustomerId,
            PaymentDate = DateTime.UtcNow,
            DepositAccount = "Undeposited Funds",
            PaymentMethod = paymentMethod,
            ReferenceNumber = string.IsNullOrEmpty(paymentReference) ? paymentNumber : paymentReference,
            PaymentNumber = paymentNumber,
            AmountReceived = paymentAmount,
            TotalApplied = paymentAmount,
            UnappliedAmount = 0,
            Status = "Completed",
            IsDeposited = false,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name ?? "System",
            UpdatedBy = User.Identity?.Name ?? "System"
        };

        _context.CustomerPayments.Add(customerPayment);
        await _context.SaveChangesAsync();

        // Create CustomerPaymentInvoice record to link payment to invoice
        var customerPaymentInvoice = new CustomerPaymentInvoice
        {
            CustomerPaymentId = customerPayment.Id,
            SalesInvoiceId = invoice.Id,
            InvoiceAmount = invoice.GrandTotal,
            AmountDue = invoice.BalanceDue,
            PaymentApplied = paymentAmount,
            RemainingBalance = invoice.BalanceDue - paymentAmount
        };

        _context.CustomerPaymentInvoices.Add(customerPaymentInvoice);

        // Update payment
        invoice.AmountPaid += paymentAmount;
        invoice.BalanceDue = invoice.GrandTotal - invoice.AmountPaid;

        // Update status based on payment
        if (invoice.BalanceDue <= 0)
        {
            invoice.Status = "Paid";
            invoice.IsPaid = true;
            invoice.PaidDate = DateTime.UtcNow;
        }
        else
        {
            invoice.Status = "Partially Paid";
        }

        invoice.UpdatedAtUtc = DateTime.UtcNow;
        invoice.UpdatedBy = User.Identity?.Name ?? "System";

        await _context.SaveChangesAsync();

        this.AddSuccessMessage($"Payment of {paymentAmount:C} received successfully. Receipt #: {paymentNumber}");
        return RedirectToAction(nameof(Edit), new { id });
    }

    // POST: SalesInvoice/EmailInvoice/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EmailInvoice(int id)
    {
        var invoice = await _context.SalesInvoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        invoice.Status = "Sent";
        invoice.UpdatedAtUtc = DateTime.UtcNow;
        invoice.UpdatedBy = User.Identity?.Name ?? "System";

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Edit), new { id });
    }

    // GET: SalesInvoice/PrintInvoice/5
    public async Task<IActionResult> PrintInvoice(int id)
    {
        var invoice = await _context.SalesInvoices
            .Include(si => si.Customer)
            .Include(si => si.Warehouse)
            .Include(si => si.SalesInvoiceItems)
                .ThenInclude(sii => sii.PaintItem)
            .FirstOrDefaultAsync(si => si.Id == id);

        if (invoice == null)
        {
            return NotFound();
        }

        return View(invoice);
    }

    // GET: SalesInvoice/PaymentReceipt/5
    public async Task<IActionResult> PaymentReceipt(int id)
    {
        var paymentInvoice = await _context.CustomerPaymentInvoices
            .Include(cpi => cpi.CustomerPayment)
                .ThenInclude(cp => cp.Customer)
            .Include(cpi => cpi.SalesInvoice)
            .FirstOrDefaultAsync(cpi => cpi.Id == id);

        if (paymentInvoice == null)
        {
            return NotFound();
        }

        return View(paymentInvoice);
    }

    // POST: SalesInvoice/VoidInvoice/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VoidInvoice(int id, string voidReason)
    {
        var invoice = await _context.SalesInvoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        // Validation: Cannot void already voided invoices
        if (invoice.IsVoided)
        {
            this.AddErrorMessage("Invoice is already voided");
            return RedirectToAction(nameof(Edit), new { id });
        }

        // Validation: Cannot void fully paid invoices without admin approval
        if (invoice.IsPaid)
        {
            this.AddErrorMessage("Cannot void a fully paid invoice. Please contact administrator");
            return RedirectToAction(nameof(Edit), new { id });
        }

        // Validation: Void reason is required
        if (string.IsNullOrWhiteSpace(voidReason))
        {
            this.AddErrorMessage("Void reason is required");
            return RedirectToAction(nameof(Edit), new { id });
        }

        invoice.Status = "Void";
        invoice.IsVoided = true;
        invoice.VoidedDate = DateTime.UtcNow;
        invoice.UpdatedAtUtc = DateTime.UtcNow;
        invoice.UpdatedBy = User.Identity?.Name ?? "System";
        invoice.InternalNotes = $"VOIDED: {voidReason}. {invoice.InternalNotes ?? ""}";

        await _context.SaveChangesAsync();

        this.AddSuccessMessage("Invoice has been voided successfully");
        return RedirectToAction(nameof(Edit), new { id });
    }

    private async Task<string> GenerateInvoiceNumber()
    {
        var lastInvoice = await _context.SalesInvoices
            .OrderByDescending(si => si.Id)
            .FirstOrDefaultAsync();

        if (lastInvoice == null)
        {
            return "INV-0001";
        }

        var lastNumber = lastInvoice.InvoiceNumber.Replace("INV-", "");
        if (int.TryParse(lastNumber, out int number))
        {
            return $"INV-{(number + 1):D4}";
        }

        return "INV-0001";
    }

    private async Task UpdateInvoiceStatusAsync(SalesInvoice invoice)
    {
        // Skip if already voided or paid
        if (invoice.IsVoided || invoice.IsPaid)
        {
            return;
        }

        // Check if invoice is overdue
        if (invoice.DueDate.HasValue && invoice.DueDate.Value < DateTime.UtcNow.Date)
        {
            // Only update to Overdue if not already overdue
            if (invoice.Status != "Overdue")
            {
                invoice.Status = "Overdue";
                invoice.UpdatedAtUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }

    private async Task PopulateDropdowns(SalesInvoiceFormViewModel model)
    {
        // Ensure all collections are initialized
        if (model.Customers == null)
        {
            model.Customers = new List<SalesInvoiceCustomerListItem>();
        }
        if (model.Warehouses == null)
        {
            model.Warehouses = new List<SalesInvoiceWarehouseListItem>();
        }
        if (model.PaintItems == null)
        {
            model.PaintItems = new List<SalesInvoicePaintItemListItem>();
        }
        if (model.ShippingMethods == null)
        {
            model.ShippingMethods = new List<string> { "Pickup", "Ground", "Express", "Freight", "Courier" };
        }
        if (model.PaymentTermsOptions == null)
        {
            model.PaymentTermsOptions = new List<string> { "Due on Receipt", "Net 15", "Net 30", "Net 45", "Net 60" };
        }
        if (model.Units == null)
        {
            model.Units = new List<string> { "Gallon", "Liter", "Quart", "Pint", "Each", "Box", "Case" };
        }

        // Populate dropdown lists from database
        model.Customers = await _context.Customers
            .OrderBy(c => c.BusinessName)
            .Select(c => new SalesInvoiceCustomerListItem { Id = c.Id, BusinessName = c.BusinessName })
            .ToListAsync();

        model.Warehouses = await _context.Warehouses
            .OrderBy(w => w.Name)
            .Select(w => new SalesInvoiceWarehouseListItem { Id = w.Id, Name = w.Name })
            .ToListAsync();

        model.PaintItems = await _context.PaintItems
            .OrderBy(p => p.Name)
            .Select(p => new SalesInvoicePaintItemListItem
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU ?? "",
                SellingPrice = p.SellingPrice ?? 0,
                UnitOfMeasure = p.UnitOfMeasure ?? "",
                CurrentStock = p.CurrentStock
            })
            .ToListAsync();
    }
}
