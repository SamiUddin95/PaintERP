using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;

namespace PaintERP.Services;

public class InventoryService : IInventoryService
{
    private readonly PaintErpDbContext _context;

    public InventoryService(PaintErpDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryResult> ReduceStockAsync(int paintItemId, int warehouseId, decimal quantity, string transactionType, int transactionId, string notes = "", CancellationToken cancellationToken = default)
    {
        var paintItem = await _context.PaintItems
            .FirstOrDefaultAsync(p => p.Id == paintItemId && p.WarehouseId == warehouseId, cancellationToken);

        if (paintItem == null)
            return InventoryResult.FailureResult("Paint item not found in specified warehouse");

        var stockBefore = paintItem.CurrentStock;

        if (stockBefore < quantity)
            return InventoryResult.FailureResult($"Insufficient stock. Available: {stockBefore:0.##}, Required: {quantity:0.##}");

        // Log before changes
        var currentStockBefore = paintItem.CurrentStock;
        var availableStockBefore = paintItem.AvailableStock;

        // Update stock (CurrentStock is the authoritative decimal balance)
        paintItem.CurrentStock -= quantity;
        paintItem.AvailableStock -= quantity;
        paintItem.StockQuantity = (int)Math.Floor(paintItem.CurrentStock);
        paintItem.UpdatedAtUtc = DateTime.UtcNow;

        // Log the stock reduction
        System.Diagnostics.Debug.WriteLine($"Stock Reduction - Item: {paintItem.Name}");
        System.Diagnostics.Debug.WriteLine($"  BEFORE: CurrentStock={currentStockBefore:0.####}, AvailableStock={availableStockBefore:0.####}");
        System.Diagnostics.Debug.WriteLine($"  DEDUCT: {quantity:0.####}");
        System.Diagnostics.Debug.WriteLine($"  AFTER:  CurrentStock={paintItem.CurrentStock:0.####}, AvailableStock={paintItem.AvailableStock:0.####}");

        // Update inventory value
        await UpdateInventoryValuationAsync(paintItemId, cancellationToken);

        // Create stock ledger entry
        var ledgerEntry = new StockLedger
        {
            CompanyId = 1, // Default company
            WarehouseId = warehouseId,
            PaintItemId = paintItemId,
            TransactionDate = DateTime.UtcNow,
            TransactionType = transactionType,
            ReferenceNumber = transactionId.ToString(),
            Description = notes,
            OutQty = quantity,
            InQty = 0,
            RunningBalance = paintItem.CurrentStock,
            UserName = "System",
            Remarks = notes
        };

        _context.StockLedgers.Add(ledgerEntry);
        await _context.SaveChangesAsync(cancellationToken);

        // Verify after save
        System.Diagnostics.Debug.WriteLine($"Stock Reduction AFTER Save - Item: {paintItem.Name}, CurrentStock: {paintItem.CurrentStock:0.####}, AvailableStock: {paintItem.AvailableStock:0.####}");

        return InventoryResult.SuccessResult(stockBefore, paintItem.CurrentStock, paintItem.InventoryValue, "Stock reduced successfully");
    }

    public async Task<InventoryResult> IncreaseStockAsync(int paintItemId, int warehouseId, decimal quantity, string transactionType, int transactionId, string notes = "", CancellationToken cancellationToken = default)
    {
        var paintItem = await _context.PaintItems
            .FirstOrDefaultAsync(p => p.Id == paintItemId && p.WarehouseId == warehouseId, cancellationToken);

        if (paintItem == null)
            return InventoryResult.FailureResult("Paint item not found in specified warehouse");

        var stockBefore = paintItem.CurrentStock;

        // Update stock (CurrentStock is the authoritative decimal balance)
        paintItem.CurrentStock += quantity;
        paintItem.AvailableStock += quantity;
        paintItem.StockQuantity = (int)Math.Floor(paintItem.CurrentStock);
        paintItem.UpdatedAtUtc = DateTime.UtcNow;

        // Update inventory value
        await UpdateInventoryValuationAsync(paintItemId, cancellationToken);

        // Create stock ledger entry
        var ledgerEntry = new StockLedger
        {
            CompanyId = 1, // Default company
            WarehouseId = warehouseId,
            PaintItemId = paintItemId,
            TransactionDate = DateTime.UtcNow,
            TransactionType = transactionType,
            ReferenceNumber = transactionId.ToString(),
            Description = notes,
            InQty = quantity,
            OutQty = 0,
            RunningBalance = paintItem.CurrentStock,
            UserName = "System",
            Remarks = notes
        };

        _context.StockLedgers.Add(ledgerEntry);
        await _context.SaveChangesAsync(cancellationToken);

        return InventoryResult.SuccessResult(stockBefore, paintItem.CurrentStock, paintItem.InventoryValue, "Stock increased successfully");
    }

    public async Task<InventoryResult> TransferStockAsync(int paintItemId, int sourceWarehouseId, int destinationWarehouseId, decimal quantity, string transactionType, int transactionId, CancellationToken cancellationToken = default)
    {
        // Reduce from source
        var reduceResult = await ReduceStockAsync(paintItemId, sourceWarehouseId, quantity, transactionType, transactionId, "Transfer Out", cancellationToken);
        
        if (!reduceResult.Success)
            return reduceResult;

        // Increase in destination
        var increaseResult = await IncreaseStockAsync(paintItemId, destinationWarehouseId, quantity, transactionType, transactionId, "Transfer In", cancellationToken);
        
        if (!increaseResult.Success)
        {
            // Rollback source reduction
            await IncreaseStockAsync(paintItemId, sourceWarehouseId, quantity, "Rollback", transactionId, "Transfer failed - rollback", cancellationToken);
            return increaseResult;
        }

        return InventoryResult.SuccessResult(reduceResult.StockBefore, increaseResult.StockAfter, increaseResult.InventoryValue, "Stock transferred successfully");
    }

    public async Task<InventoryResult> AdjustStockAsync(int paintItemId, int warehouseId, decimal newQuantity, string reason, CancellationToken cancellationToken = default)
    {
        var paintItem = await _context.PaintItems
            .FirstOrDefaultAsync(p => p.Id == paintItemId && p.WarehouseId == warehouseId, cancellationToken);

        if (paintItem == null)
            return InventoryResult.FailureResult("Paint item not found in specified warehouse");

        var stockBefore = paintItem.StockQuantity;
        var difference = newQuantity - stockBefore;

        paintItem.StockQuantity = (int)newQuantity;
        paintItem.CurrentStock = newQuantity;
        paintItem.AvailableStock = newQuantity;
        paintItem.UpdatedAtUtc = DateTime.UtcNow;

        // Update inventory value
        await UpdateInventoryValuationAsync(paintItemId, cancellationToken);

        // Create stock ledger entry
        var ledgerEntry = new StockLedger
        {
            CompanyId = 1, // Default company
            WarehouseId = warehouseId,
            PaintItemId = paintItemId,
            TransactionDate = DateTime.UtcNow,
            TransactionType = "Stock Adjustment",
            ReferenceNumber = $"ADJ-{DateTime.UtcNow:yyyyMMddHHmmss}",
            Description = reason,
            InQty = difference > 0 ? difference : 0,
            OutQty = difference < 0 ? Math.Abs(difference) : 0,
            RunningBalance = paintItem.CurrentStock,
            UserName = "System",
            Remarks = reason
        };

        _context.StockLedgers.Add(ledgerEntry);
        await _context.SaveChangesAsync(cancellationToken);

        return InventoryResult.SuccessResult(stockBefore, paintItem.StockQuantity, paintItem.InventoryValue, "Stock adjusted successfully");
    }

    public async Task<decimal> GetAvailableStockAsync(int paintItemId, int warehouseId, CancellationToken cancellationToken = default)
    {
        var paintItem = await _context.PaintItems
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == paintItemId && p.WarehouseId == warehouseId, cancellationToken);

        return paintItem?.AvailableStock ?? 0;
    }

    public async Task<decimal> CalculateStockValueAsync(int paintItemId, int warehouseId, CancellationToken cancellationToken = default)
    {
        var paintItem = await _context.PaintItems
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == paintItemId && p.WarehouseId == warehouseId, cancellationToken);

        if (paintItem == null) return 0;

        return paintItem.CurrentStock * paintItem.UnitCost;
    }

    public async Task UpdateInventoryValuationAsync(int paintItemId, CancellationToken cancellationToken = default)
    {
        var paintItem = await _context.PaintItems
            .FirstOrDefaultAsync(p => p.Id == paintItemId, cancellationToken);

        if (paintItem != null)
        {
            paintItem.InventoryValue = paintItem.CurrentStock * paintItem.UnitCost;
            paintItem.UpdatedAtUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ProcessSalesInvoiceInventoryAsync(SalesInvoice invoice, CancellationToken cancellationToken = default)
    {
        if (invoice.SalesInvoiceItems == null || !invoice.SalesInvoiceItems.Any())
            return false;

        foreach (var item in invoice.SalesInvoiceItems)
        {
            var result = await ReduceStockAsync(
                item.PaintItemId ?? 0,
                invoice.WarehouseId ?? 0,
                item.Quantity,
                "Sales Invoice",
                invoice.Id,
                $"Sales Invoice: {invoice.InvoiceNumber}",
                cancellationToken
            );

            if (!result.Success)
                return false;

            // Update item stock tracking
            item.StockBefore = result.StockBefore;
            item.StockAfter = result.StockAfter;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ProcessPurchaseOrderInventoryAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        if (purchaseOrder.PurchaseOrderItems == null || !purchaseOrder.PurchaseOrderItems.Any())
            return false;

        foreach (var item in purchaseOrder.PurchaseOrderItems)
        {
            var result = await IncreaseStockAsync(
                item.PaintItemId ?? 0,
                purchaseOrder.WarehouseId ?? 0,
                item.QuantityReceived,
                "Purchase Order",
                purchaseOrder.Id,
                $"PO: {purchaseOrder.PONumber}",
                cancellationToken
            );

            if (!result.Success)
                return false;
        }

        return true;
    }

    public async Task<bool> ProcessProductionInventoryAsync(PaintProduction production, CancellationToken cancellationToken = default)
    {
        // Consume raw materials
        if (production.Materials != null && production.Materials.Any())
        {
            foreach (var material in production.Materials)
            {
                var result = await ReduceStockAsync(
                    material.PaintItemId ?? 0,
                    production.WarehouseId,
                    material.ConsumedQuantity,
                    "Production",
                    production.Id,
                    $"Production: {production.ProductionNumber}",
                    cancellationToken
                );

                if (!result.Success)
                    return false;

                material.StockBefore = result.StockBefore;
                material.StockAfter = result.StockAfter;
            }
        }

        // Add finished goods
        if (production.FinishedProductId.HasValue && production.FinishedProductId > 0)
        {
            var result = await IncreaseStockAsync(
                production.FinishedProductId.Value,
                production.WarehouseId,
                production.OutputQuantity,
                "Production",
                production.Id,
                $"Production: {production.ProductionNumber} - Finished Goods",
                cancellationToken
            );

            if (!result.Success)
                return false;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ProcessInventoryTransferAsync(InventoryTransfer transfer, CancellationToken cancellationToken = default)
    {
        if (transfer.InventoryTransferItems == null || !transfer.InventoryTransferItems.Any())
            return false;

        foreach (var item in transfer.InventoryTransferItems)
        {
            var result = await TransferStockAsync(
                item.PaintItemId ?? 0,
                transfer.SourceWarehouseId,
                transfer.DestinationWarehouseId,
                item.Quantity,
                "Inventory Transfer",
                transfer.Id,
                cancellationToken
            );

            if (!result.Success)
                return false;

            // Update transfer item tracking
            item.SourceStockBefore = result.StockBefore;
            item.SourceStockAfter = result.StockAfter;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
