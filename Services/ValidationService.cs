using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;

namespace PaintERP.Services;

public class ValidationService : IValidationService
{
    private readonly PaintErpDbContext _context;

    public ValidationService(PaintErpDbContext context)
    {
        _context = context;
    }

    public async Task<ValidationResult> ValidateSalesInvoiceAsync(SalesInvoice invoice, CancellationToken cancellationToken = default)
    {
        var result = ValidationResult.Success();

        // Mandatory field validation
        if (invoice.CustomerId <= 0)
            result.AddError("Customer is required");
        
        if (string.IsNullOrWhiteSpace(invoice.InvoiceNumber))
            result.AddError("Invoice number is required");

        if (invoice.InvoiceDate == default)
            result.AddError("Invoice date is required");

        if (invoice.GrandTotal <= 0)
            result.AddError("Invoice total must be greater than zero");

        // Duplicate prevention
        if (await IsDuplicateDocumentAsync("SalesInvoice", invoice.InvoiceNumber, invoice.Id, cancellationToken))
            result.AddError($"Invoice number '{invoice.InvoiceNumber}' already exists");

        // Date validation (no future dates for posting)
        if (invoice.InvoiceDate > DateTime.UtcNow.Date)
            result.AddError("Invoice date cannot be in the future");

        // Customer status validation
        if (!await IsCustomerActiveAsync(invoice.CustomerId, cancellationToken))
            result.AddError("Customer is inactive and cannot be invoiced");

        // Credit limit validation
        if (!await CheckCreditLimitAsync(invoice.CustomerId, invoice.GrandTotal, cancellationToken))
            result.AddWarning("This invoice will exceed customer's credit limit");

        // Inventory availability validation
        if (invoice.SalesInvoiceItems != null && invoice.SalesInvoiceItems.Any())
        {
            var insufficientItems = new List<string>();
            foreach (var item in invoice.SalesInvoiceItems)
            {
                if (!await IsInventoryAvailableAsync(item.PaintItemId ?? 0, invoice.WarehouseId ?? 0, item.Quantity, cancellationToken))
                {
                    insufficientItems.Add(item.Description ?? "Item");
                }
            }

            if (insufficientItems.Any())
            {
                if (insufficientItems.Count == 1)
                {
                    result.AddError($"The item '{insufficientItems[0]}' has insufficient stock. Please adjust the quantity or select a different item.");
                }
                else
                {
                    result.AddError($"The following items have insufficient stock: {string.Join(", ", insufficientItems)}. Please adjust the quantities or select different items.");
                }
            }
        }
        else
        {
            result.AddError("Invoice must have at least one line item");
        }

        return result;
    }

    public async Task<ValidationResult> ValidatePurchaseOrderAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        var result = ValidationResult.Success();

        // Mandatory field validation
        if (purchaseOrder.VendorId <= 0)
            result.AddError("Vendor is required");

        if (string.IsNullOrWhiteSpace(purchaseOrder.PONumber))
            result.AddError("PO number is required");

        if (purchaseOrder.OrderDate == default)
            result.AddError("Order date is required");

        if (purchaseOrder.TotalAmount <= 0)
            result.AddError("PO total must be greater than zero");

        // Duplicate prevention
        if (await IsDuplicateDocumentAsync("PurchaseOrder", purchaseOrder.PONumber, purchaseOrder.Id, cancellationToken))
            result.AddError($"PO number '{purchaseOrder.PONumber}' already exists");

        // Vendor status validation
        if (!await IsVendorActiveAsync(purchaseOrder.VendorId, cancellationToken))
            result.AddError("Vendor is inactive");

        // Line items validation
        if (purchaseOrder.PurchaseOrderItems == null || !purchaseOrder.PurchaseOrderItems.Any())
            result.AddError("Purchase order must have at least one line item");

        return result;
    }

    public async Task<ValidationResult> ValidateBillAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        var result = ValidationResult.Success();

        // Mandatory field validation
        if (bill.VendorId <= 0)
            result.AddError("Vendor is required");

        if (string.IsNullOrWhiteSpace(bill.BillNumber))
            result.AddError("Bill number is required");

        if (bill.BillDate == default)
            result.AddError("Bill date is required");

        if (bill.GrandTotal <= 0)
            result.AddError("Bill total must be greater than zero");

        // Duplicate prevention
        if (await IsDuplicateDocumentAsync("Bill", bill.BillNumber, bill.Id, cancellationToken))
            result.AddError($"Bill number '{bill.BillNumber}' already exists");

        // Vendor status validation
        if (!await IsVendorActiveAsync(bill.VendorId, cancellationToken))
            result.AddError("Vendor is inactive");

        // Line items validation
        if (bill.BillItems == null || !bill.BillItems.Any())
            result.AddError("Bill must have at least one line item");

        return result;
    }

    public async Task<ValidationResult> ValidateGoodsReceivedNoteAsync(GoodsReceivedNote grn, CancellationToken cancellationToken = default)
    {
        var result = ValidationResult.Success();

        // Mandatory field validation
        if (grn.PurchaseOrderId <= 0)
            result.AddError("Purchase Order is required");

        if (string.IsNullOrWhiteSpace(grn.GRNNumber))
            result.AddError("GRN number is required");

        if (grn.GRNDate == default)
            result.AddError("GRN date is required");

        if (grn.VendorId <= 0)
            result.AddError("Vendor is required");

        if (grn.WarehouseId <= 0)
            result.AddError("Warehouse is required");

        // Duplicate prevention
        if (await IsDuplicateDocumentAsync("GRN", grn.GRNNumber, grn.Id, cancellationToken))
            result.AddError($"GRN number '{grn.GRNNumber}' already exists");

        // Vendor status validation - skip for GRNs since vendor comes from approved PO
        // if (!await IsVendorActiveAsync(grn.VendorId, cancellationToken))
        //     result.AddError("Vendor is inactive");

        // Line items validation
        if (grn.GoodsReceivedNoteItems == null || !grn.GoodsReceivedNoteItems.Any())
            result.AddError("GRN must have at least one line item");

        // Validate quantities don't exceed ordered quantities
        if (grn.GoodsReceivedNoteItems != null)
        {
            foreach (var item in grn.GoodsReceivedNoteItems)
            {
                if (item.QuantityReceived <= 0)
                    result.AddError($"Item '{item.Description}' must have a received quantity greater than zero");

                if (item.QuantityReceived > item.QuantityRemaining)
                    result.AddError($"Item '{item.Description}' received quantity ({item.QuantityReceived}) exceeds remaining quantity ({item.QuantityRemaining})");

                // Ensure Unit is not empty
                if (string.IsNullOrWhiteSpace(item.Unit))
                    item.Unit = "Each";
            }
        }

        return result;
    }

    public async Task<ValidationResult> ValidateProductionAsync(PaintProduction production, CancellationToken cancellationToken = default)
    {
        var result = ValidationResult.Success();

        // Mandatory field validation
        if (string.IsNullOrWhiteSpace(production.ProductionNumber))
            result.AddError("Production number is required");

        if (production.ProductionDate == default)
            result.AddError("Production date is required");

        if (production.OutputQuantity <= 0)
            result.AddError("Output quantity must be greater than zero");

        if (production.WarehouseId <= 0)
            result.AddError("Warehouse is required");

        // Formula validation
        if (production.FormulaId.HasValue && production.FormulaId > 0)
        {
            if (!await IsFormulaValidAsync(production.FormulaId.Value, cancellationToken))
                result.AddError("Selected formula is not active or does not exist");
        }

        // Duplicate prevention
        if (await IsDuplicateDocumentAsync("Production", production.ProductionNumber, production.Id, cancellationToken))
            result.AddError($"Production number '{production.ProductionNumber}' already exists");

        // Material availability validation
        if (production.Materials != null && production.Materials.Any())
        {
            foreach (var material in production.Materials)
            {
                if (!await IsInventoryAvailableAsync(material.PaintItemId ?? 0, production.WarehouseId, material.ConsumedQuantity, cancellationToken))
                    result.AddError($"Insufficient inventory for material: {material.MaterialName}");
            }
        }

        return result;
    }

    public async Task<ValidationResult> ValidateInventoryTransferAsync(InventoryTransfer transfer, CancellationToken cancellationToken = default)
    {
        var result = ValidationResult.Success();

        // Mandatory field validation
        if (string.IsNullOrWhiteSpace(transfer.TransferNumber))
            result.AddError("Transfer number is required");

        if (transfer.TransferDate == default)
            result.AddError("Transfer date is required");

        if (transfer.SourceWarehouseId <= 0)
            result.AddError("Source warehouse is required");

        if (transfer.DestinationWarehouseId <= 0)
            result.AddError("Destination warehouse is required");

        if (transfer.SourceWarehouseId == transfer.DestinationWarehouseId)
            result.AddError("Source and destination warehouses must be different");

        // Duplicate prevention
        if (await IsDuplicateDocumentAsync("InventoryTransfer", transfer.TransferNumber, transfer.Id, cancellationToken))
            result.AddError($"Transfer number '{transfer.TransferNumber}' already exists");

        // Inventory availability validation
        if (transfer.InventoryTransferItems != null && transfer.InventoryTransferItems.Any())
        {
            foreach (var item in transfer.InventoryTransferItems)
            {
                if (!await IsInventoryAvailableAsync(item.PaintItemId ?? 0, transfer.SourceWarehouseId, item.Quantity, cancellationToken))
                    result.AddError($"Insufficient inventory in source warehouse for item ID: {item.PaintItemId ?? 0}");
            }
        }
        else
        {
            result.AddError("Transfer must have at least one line item");
        }

        return result;
    }

    public async Task<ValidationResult> ValidateCustomerPaymentAsync(CustomerPayment payment, CancellationToken cancellationToken = default)
    {
        var result = ValidationResult.Success();

        // Mandatory field validation
        if (payment.CustomerId <= 0)
            result.AddError("Customer is required");

        if (string.IsNullOrWhiteSpace(payment.PaymentNumber))
            result.AddError("Payment number is required");

        if (payment.PaymentDate == default)
            result.AddError("Payment date is required");

        if (payment.AmountReceived <= 0)
            result.AddError("Payment amount must be greater than zero");

        // Duplicate prevention
        if (await IsDuplicateDocumentAsync("CustomerPayment", payment.PaymentNumber, payment.Id, cancellationToken))
            result.AddError($"Payment number '{payment.PaymentNumber}' already exists");

        // Customer status validation
        if (!await IsCustomerActiveAsync(payment.CustomerId, cancellationToken))
            result.AddError("Customer is inactive");

        return result;
    }

    public async Task<ValidationResult> ValidateVendorPaymentAsync(VendorPayment payment, CancellationToken cancellationToken = default)
    {
        var result = ValidationResult.Success();

        // Mandatory field validation
        if (payment.VendorId <= 0)
            result.AddError("Vendor is required");

        if (string.IsNullOrWhiteSpace(payment.PaymentNumber))
            result.AddError("Payment number is required");

        if (payment.PaymentDate == default)
            result.AddError("Payment date is required");

        if (payment.TotalPaymentAmount <= 0)
            result.AddError("Payment amount must be greater than zero");

        // Duplicate prevention
        if (await IsDuplicateDocumentAsync("VendorPayment", payment.PaymentNumber, payment.Id, cancellationToken))
            result.AddError($"Payment number '{payment.PaymentNumber}' already exists");

        // Vendor status validation
        if (!await IsVendorActiveAsync(payment.VendorId, cancellationToken))
            result.AddError("Vendor is inactive");

        return result;
    }

    public async Task<bool> IsDuplicateDocumentAsync(string documentType, string documentNumber, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return documentType switch
        {
            "SalesInvoice" => await _context.SalesInvoices
                .AnyAsync(si => si.InvoiceNumber == documentNumber && si.Id != (excludeId ?? 0), cancellationToken),
            
            "PurchaseOrder" => await _context.PurchaseOrders
                .AnyAsync(po => po.PONumber == documentNumber && po.Id != (excludeId ?? 0), cancellationToken),
            
            "Bill" => await _context.Bills
                .AnyAsync(b => b.BillNumber == documentNumber && b.Id != (excludeId ?? 0), cancellationToken),
            
            "Production" => await _context.PaintProductions
                .AnyAsync(p => p.ProductionNumber == documentNumber && p.Id != (excludeId ?? 0), cancellationToken),
            
            "InventoryTransfer" => await _context.InventoryTransfers
                .AnyAsync(it => it.TransferNumber == documentNumber && it.Id != (excludeId ?? 0), cancellationToken),
            
            "CustomerPayment" => await _context.CustomerPayments
                .AnyAsync(cp => cp.PaymentNumber == documentNumber && cp.Id != (excludeId ?? 0), cancellationToken),
            
            "VendorPayment" => await _context.VendorPayments
                .AnyAsync(vp => vp.PaymentNumber == documentNumber && vp.Id != (excludeId ?? 0), cancellationToken),
            
            _ => false
        };
    }

    public async Task<bool> IsCustomerActiveAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
        
        return customer?.IsActive ?? false;
    }

    public async Task<bool> IsVendorActiveAsync(int vendorId, CancellationToken cancellationToken = default)
    {
        var vendor = await _context.Vendors
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == vendorId, cancellationToken);
        
        return vendor?.IsActive ?? false;
    }

    public async Task<bool> CheckCreditLimitAsync(int customerId, decimal amount, CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
        
        if (customer == null) return false;
        
        var currentBalance = customer.OutstandingBalance;
        var creditLimit = customer.CreditLimit;
        
        // If no credit limit set, allow transaction
        if (creditLimit <= 0) return true;
        
        return (currentBalance + amount) <= creditLimit;
    }

    public async Task<bool> IsInventoryAvailableAsync(int paintItemId, int warehouseId, decimal quantity, CancellationToken cancellationToken = default)
    {
        var paintItem = await _context.PaintItems
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == paintItemId && p.WarehouseId == warehouseId, cancellationToken);
        
        if (paintItem == null) return false;
        
        return paintItem.StockQuantity >= quantity;
    }

    public async Task<bool> IsFormulaValidAsync(int formulaId, CancellationToken cancellationToken = default)
    {
        var formula = await _context.PaintFormulas
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == formulaId, cancellationToken);
        
        return formula?.Status == "Active" || formula?.Status == "Approved";
    }
}
