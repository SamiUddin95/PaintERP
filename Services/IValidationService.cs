using PaintERP.Models.Entities;

namespace PaintERP.Services;

public interface IValidationService
{
    Task<ValidationResult> ValidateSalesInvoiceAsync(SalesInvoice invoice, CancellationToken cancellationToken = default);
    Task<ValidationResult> ValidatePurchaseOrderAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);
    Task<ValidationResult> ValidateBillAsync(Bill bill, CancellationToken cancellationToken = default);
    Task<ValidationResult> ValidateGoodsReceivedNoteAsync(GoodsReceivedNote grn, CancellationToken cancellationToken = default);
    Task<ValidationResult> ValidateProductionAsync(PaintProduction production, CancellationToken cancellationToken = default);
    Task<ValidationResult> ValidateInventoryTransferAsync(InventoryTransfer transfer, CancellationToken cancellationToken = default);
    Task<ValidationResult> ValidateCustomerPaymentAsync(CustomerPayment payment, CancellationToken cancellationToken = default);
    Task<ValidationResult> ValidateVendorPaymentAsync(VendorPayment payment, CancellationToken cancellationToken = default);
    Task<bool> IsDuplicateDocumentAsync(string documentType, string documentNumber, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> IsCustomerActiveAsync(int customerId, CancellationToken cancellationToken = default);
    Task<bool> IsVendorActiveAsync(int vendorId, CancellationToken cancellationToken = default);
    Task<bool> CheckCreditLimitAsync(int customerId, decimal amount, CancellationToken cancellationToken = default);
    Task<bool> IsInventoryAvailableAsync(int paintItemId, int warehouseId, decimal quantity, CancellationToken cancellationToken = default);
    Task<bool> IsFormulaValidAsync(int formulaId, CancellationToken cancellationToken = default);
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();

    public static ValidationResult Success() => new() { IsValid = true };
    
    public static ValidationResult Failure(params string[] errors) => new() 
    { 
        IsValid = false, 
        Errors = errors.ToList() 
    };

    public void AddError(string error)
    {
        IsValid = false;
        Errors.Add(error);
    }

    public void AddWarning(string warning)
    {
        Warnings.Add(warning);
    }
}
