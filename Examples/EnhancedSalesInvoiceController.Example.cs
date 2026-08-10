using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Helpers;
using PaintERP.Models.Entities;
using PaintERP.Services;

namespace PaintERP.Controllers;

/// <summary>
/// EXAMPLE: Enhanced Sales Invoice Controller with full validation, inventory management, and accounting integration
/// This demonstrates how to properly use all the new services
/// </summary>
public class EnhancedSalesInvoiceControllerExample : Controller
{
    private readonly PaintErpDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IInventoryService _inventoryService;
    private readonly IAccountingService _accountingService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<EnhancedSalesInvoiceControllerExample> _logger;

    public EnhancedSalesInvoiceControllerExample(
        PaintErpDbContext context,
        IValidationService validationService,
        IInventoryService inventoryService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService,
        ILogger<EnhancedSalesInvoiceControllerExample> logger)
    {
        _context = context;
        _validationService = validationService;
        _inventoryService = inventoryService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _logger = logger;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SalesInvoice invoice, CancellationToken cancellationToken)
    {
        try
        {
            // Step 1: Validate ModelState
            if (!ModelState.IsValid)
            {
                this.AddErrorMessage("Please correct the validation errors");
                return View(invoice);
            }

            // Step 2: Business Validation
            var validationResult = await _validationService.ValidateSalesInvoiceAsync(invoice, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                _notificationService.AddValidationErrors(validationResult);
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
                    // Set audit fields
                    invoice.CreatedAtUtc = DateTime.UtcNow;
                    invoice.UpdatedAtUtc = DateTime.UtcNow;
                    invoice.CreatedBy = User.Identity?.Name ?? "System";
                    invoice.UpdatedBy = User.Identity?.Name ?? "System";

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
                    _logger.LogError(ex, "Error creating sales invoice: {Message}", ex.Message);
                    return TransactionResult.FailureResult(
                        "An error occurred while creating the invoice",
                        ex.Message
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
                return View(invoice);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in Create action: {Message}", ex.Message);
            return this.WithError("An unexpected error occurred. Please try again.", "Index");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
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
}
