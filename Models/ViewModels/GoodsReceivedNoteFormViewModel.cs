namespace PaintERP.Models.ViewModels;

public class GoodsReceivedNoteFormViewModel
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public string GRNNumber { get; set; } = string.Empty;
    public DateTime GRNDate { get; set; } = DateTime.Today;
    public int? WarehouseId { get; set; }
    public int VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string? ReferenceNumber { get; set; }
    public string Status { get; set; } = "Draft";
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? InternalNotes { get; set; }
    public string? VendorNotes { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }

    // Dropdown lists
    public List<GRNWarehouseListItem> Warehouses { get; set; } = new();
    public List<GRNPOListItem> PurchaseOrders { get; set; } = new();
    public List<GoodsReceivedNoteItemViewModel> GoodsReceivedNoteItems { get; set; } = new();
}

public class GoodsReceivedNoteItemViewModel
{
    public int Id { get; set; }
    public int PurchaseOrderItemId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? PaintItemId { get; set; }
    public decimal QuantityOrdered { get; set; }
    public decimal QuantityPreviouslyReceived { get; set; }
    public decimal QuantityRemaining { get; set; }
    public decimal QuantityReceived { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal LineTotal { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Location { get; set; }
    public string? Remarks { get; set; }
}

public class GRNWarehouseListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class GRNPOListItem
{
    public int Id { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsFullyReceived { get; set; }
    public int VendorId { get; set; }
    public int? WarehouseId { get; set; }
}

public class GRNListItem
{
    public int Id { get; set; }
    public string GRNNumber { get; set; } = string.Empty;
    public string PONumber { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public DateTime GRNDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ItemsCount { get; set; }
    public bool IsPosted { get; set; }
}
