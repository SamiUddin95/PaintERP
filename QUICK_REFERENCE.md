# Paint ERP - Quick Reference Card

## 🎯 Service Injection

```csharp
public class YourController : Controller
{
    private readonly IValidationService _validationService;
    private readonly IInventoryService _inventoryService;
    private readonly IAccountingService _accountingService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    
    public YourController(
        IValidationService validationService,
        IInventoryService inventoryService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService)
    {
        _validationService = validationService;
        _inventoryService = inventoryService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
    }
}
```

---

## ✅ Validation

```csharp
// Validate Sales Invoice
var result = await _validationService.ValidateSalesInvoiceAsync(invoice, cancellationToken);

// Validate Purchase Order
var result = await _validationService.ValidatePurchaseOrderAsync(po, cancellationToken);

// Validate Bill
var result = await _validationService.ValidateBillAsync(bill, cancellationToken);

// Validate Production
var result = await _validationService.ValidateProductionAsync(production, cancellationToken);

// Validate Inventory Transfer
var result = await _validationService.ValidateInventoryTransferAsync(transfer, cancellationToken);

// Validate Customer Payment
var result = await _validationService.ValidateCustomerPaymentAsync(payment, cancellationToken);

// Validate Vendor Payment
var result = await _validationService.ValidateVendorPaymentAsync(payment, cancellationToken);

// Check for duplicates
var isDuplicate = await _validationService.IsDuplicateDocumentAsync("SalesInvoice", invoiceNumber, excludeId, cancellationToken);

// Check customer status
var isActive = await _validationService.IsCustomerActiveAsync(customerId, cancellationToken);

// Check credit limit
var withinLimit = await _validationService.CheckCreditLimitAsync(customerId, amount, cancellationToken);

// Check inventory availability
var available = await _validationService.IsInventoryAvailableAsync(paintItemId, warehouseId, quantity, cancellationToken);
```

---

## 📦 Inventory Management

```csharp
// Reduce stock (Sales)
var result = await _inventoryService.ReduceStockAsync(
    paintItemId, warehouseId, quantity, 
    "Sales Invoice", invoiceId, 
    "Invoice: INV-001", cancellationToken);

// Increase stock (Purchase)
var result = await _inventoryService.IncreaseStockAsync(
    paintItemId, warehouseId, quantity, 
    "Purchase Order", poId, 
    "PO: PO-001", cancellationToken);

// Transfer stock
var result = await _inventoryService.TransferStockAsync(
    paintItemId, sourceWarehouseId, destWarehouseId, quantity,
    "Transfer", transferId, cancellationToken);

// Adjust stock
var result = await _inventoryService.AdjustStockAsync(
    paintItemId, warehouseId, newQuantity, 
    "Physical count adjustment", cancellationToken);

// Process sales invoice inventory
var success = await _inventoryService.ProcessSalesInvoiceInventoryAsync(invoice, cancellationToken);

// Process purchase order inventory
var success = await _inventoryService.ProcessPurchaseOrderInventoryAsync(po, cancellationToken);

// Process production inventory
var success = await _inventoryService.ProcessProductionInventoryAsync(production, cancellationToken);

// Process inventory transfer
var success = await _inventoryService.ProcessInventoryTransferAsync(transfer, cancellationToken);
```

---

## 💰 Accounting Integration

```csharp
// Create journal entry for sales invoice
var entry = await _accountingService.CreateSalesInvoiceEntryAsync(invoice, cancellationToken);

// Create journal entry for purchase order
var entry = await _accountingService.CreatePurchaseOrderEntryAsync(po, cancellationToken);

// Create journal entry for bill
var entry = await _accountingService.CreateBillEntryAsync(bill, cancellationToken);

// Create journal entry for customer payment
var entry = await _accountingService.CreateCustomerPaymentEntryAsync(payment, cancellationToken);

// Create journal entry for vendor payment
var entry = await _accountingService.CreateVendorPaymentEntryAsync(payment, cancellationToken);

// Create journal entry for production
var entry = await _accountingService.CreateProductionEntryAsync(production, cancellationToken);

// Reverse a journal entry
var success = await _accountingService.ReverseJournalEntryAsync(entryId, "Reason", cancellationToken);
```

---

## 🔄 Transaction Management

```csharp
// Execute in transaction
var result = await _transactionService.ExecuteInTransactionAsync(async () =>
{
    // Your business logic here
    _context.Add(entity);
    await _context.SaveChangesAsync();
    
    await _inventoryService.ProcessAsync(entity);
    await _accountingService.CreateEntryAsync(entity);
    
    return TransactionResult.SuccessResult("Success message");
}, cancellationToken);

// Execute with typed result
var result = await _transactionService.ExecuteInTransactionAsync<Invoice>(async () =>
{
    // Your business logic
    return TransactionResult<Invoice>.SuccessResult(invoice, "Created");
}, cancellationToken);
```

---

## 📢 Notifications

```csharp
// Add messages
_notificationService.AddSuccessMessage("Operation completed");
_notificationService.AddErrorMessage("Operation failed");
_notificationService.AddWarningMessage("Warning message");
_notificationService.AddInfoMessage("Info message");

// Add validation errors
_notificationService.AddValidationErrors(validationResult);

// Controller extensions
this.AddSuccessMessage("Success");
this.AddErrorMessage("Error");
this.AddWarningMessage("Warning");
this.AddInfoMessage("Info");

// Redirect with message
return this.WithSuccess("Created successfully", "Index");
return this.WithError("Failed to create", "Index");
return this.WithWarning("Warning message", "Index");
return this.WithInfo("Info message", "Index");
```

---

## 🎨 Complete Controller Pattern

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(SalesInvoice invoice, CancellationToken cancellationToken)
{
    try
    {
        // 1. Model validation
        if (!ModelState.IsValid)
        {
            this.AddErrorMessage("Please correct validation errors");
            return View(invoice);
        }

        // 2. Business validation
        var validation = await _validationService.ValidateSalesInvoiceAsync(invoice, cancellationToken);
        if (!validation.IsValid)
        {
            _notificationService.AddValidationErrors(validation);
            return this.WithValidationErrors(validation);
        }

        // 3. Execute in transaction
        var result = await _transactionService.ExecuteInTransactionAsync(async () =>
        {
            // Set audit fields
            invoice.CreatedAtUtc = DateTime.UtcNow;
            invoice.CreatedBy = User.Identity?.Name ?? "System";

            // Save
            _context.SalesInvoices.Add(invoice);
            await _context.SaveChangesAsync(cancellationToken);

            // Process inventory
            var inventorySuccess = await _inventoryService.ProcessSalesInvoiceInventoryAsync(invoice, cancellationToken);
            if (!inventorySuccess)
                return TransactionResult.FailureResult("Inventory update failed");

            // Create accounting entry
            var entry = await _accountingService.CreateSalesInvoiceEntryAsync(invoice, cancellationToken);
            if (entry == null)
                return TransactionResult.FailureResult("Accounting entry failed");

            return TransactionResult.SuccessResult($"Invoice {invoice.InvoiceNumber} created");
        }, cancellationToken);

        // 4. Return result
        return result.Success 
            ? this.WithSuccess(result.Message, "Index")
            : this.WithError(result.Message, "Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating invoice");
        return this.WithError("An unexpected error occurred", "Index");
    }
}
```

---

## 📊 Account Codes Reference

| Code | Account Name | Type |
|------|-------------|------|
| 1000 | Cash/Bank | Asset |
| 1200 | Accounts Receivable | Asset |
| 1300 | Raw Materials Inventory | Asset |
| 1310 | Finished Goods Inventory | Asset |
| 1400 | Input Tax | Asset |
| 2000 | Accounts Payable | Liability |
| 2300 | Sales Tax Payable | Liability |
| 4000 | Sales Revenue | Revenue |
| 5000 | Cost of Goods Sold | Expense |
| 5100 | Direct Labor | Expense |
| 5200 | Manufacturing Overhead | Expense |

---

## 🔍 Common Queries

```csharp
// Get available stock
var stock = await _inventoryService.GetAvailableStockAsync(paintItemId, warehouseId, cancellationToken);

// Calculate stock value
var value = await _inventoryService.CalculateStockValueAsync(paintItemId, warehouseId, cancellationToken);

// Update inventory valuation
await _inventoryService.UpdateInventoryValuationAsync(paintItemId, cancellationToken);

// Generate entry number
var entryNumber = await _accountingService.GenerateEntryNumberAsync(cancellationToken);
```

---

## ⚡ Performance Tips

1. Use `.AsNoTracking()` for read-only queries
2. Use indexes for frequently queried fields
3. Use pagination for large datasets
4. Use transactions for multi-step operations
5. Log errors but don't expose details to users

---

## 🐛 Debugging

```csharp
// Check validation result
if (!validationResult.IsValid)
{
    foreach (var error in validationResult.Errors)
        Console.WriteLine($"Error: {error}");
    
    foreach (var warning in validationResult.Warnings)
        Console.WriteLine($"Warning: {warning}");
}

// Check transaction result
if (!transactionResult.Success)
{
    Console.WriteLine($"Message: {transactionResult.Message}");
    foreach (var error in transactionResult.Errors)
        Console.WriteLine($"Error: {error}");
}

// Check inventory result
if (!inventoryResult.Success)
{
    Console.WriteLine($"Message: {inventoryResult.Message}");
    Console.WriteLine($"Stock Before: {inventoryResult.StockBefore}");
    Console.WriteLine($"Stock After: {inventoryResult.StockAfter}");
}
```

---

**Quick Tip:** Always wrap database operations in transactions and validate before saving!
