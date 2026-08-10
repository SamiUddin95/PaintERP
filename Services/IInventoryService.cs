using PaintERP.Models.Entities;

namespace PaintERP.Services;

public interface IInventoryService
{
    Task<InventoryResult> ReduceStockAsync(int paintItemId, int warehouseId, decimal quantity, string transactionType, int transactionId, string notes = "", CancellationToken cancellationToken = default);
    Task<InventoryResult> IncreaseStockAsync(int paintItemId, int warehouseId, decimal quantity, string transactionType, int transactionId, string notes = "", CancellationToken cancellationToken = default);
    Task<InventoryResult> TransferStockAsync(int paintItemId, int sourceWarehouseId, int destinationWarehouseId, decimal quantity, string transactionType, int transactionId, CancellationToken cancellationToken = default);
    Task<InventoryResult> AdjustStockAsync(int paintItemId, int warehouseId, decimal newQuantity, string reason, CancellationToken cancellationToken = default);
    Task<decimal> GetAvailableStockAsync(int paintItemId, int warehouseId, CancellationToken cancellationToken = default);
    Task<decimal> CalculateStockValueAsync(int paintItemId, int warehouseId, CancellationToken cancellationToken = default);
    Task UpdateInventoryValuationAsync(int paintItemId, CancellationToken cancellationToken = default);
    Task<bool> ProcessSalesInvoiceInventoryAsync(SalesInvoice invoice, CancellationToken cancellationToken = default);
    Task<bool> ProcessPurchaseOrderInventoryAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);
    Task<bool> ProcessProductionInventoryAsync(PaintProduction production, CancellationToken cancellationToken = default);
    Task<bool> ProcessInventoryTransferAsync(InventoryTransfer transfer, CancellationToken cancellationToken = default);
}

public class InventoryResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal StockBefore { get; set; }
    public decimal StockAfter { get; set; }
    public decimal InventoryValue { get; set; }

    public static InventoryResult SuccessResult(decimal stockBefore, decimal stockAfter, decimal inventoryValue, string message = "")
    {
        return new InventoryResult
        {
            Success = true,
            StockBefore = stockBefore,
            StockAfter = stockAfter,
            InventoryValue = inventoryValue,
            Message = message
        };
    }

    public static InventoryResult FailureResult(string message)
    {
        return new InventoryResult
        {
            Success = false,
            Message = message
        };
    }
}
