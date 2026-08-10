namespace PaintERP.Models.Entities;

public class PurchaseOrder
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int VendorId { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public int? WarehouseId { get; set; }
    public string? PaymentTerms { get; set; }
    public string? ReferenceNumber { get; set; }
    public string Status { get; set; } = "Draft";
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? InternalNotes { get; set; }
    public string? VendorNotes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Fields from database schema
    public string Buyer { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public bool IsCancelled { get; set; }
    public bool IsFullyReceived { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal AmountReceived { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public int? ConvertedBillId { get; set; }

    public Company? Company { get; set; }
    public Vendor? Vendor { get; set; }
    public Warehouse? Warehouse { get; set; }
    public List<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new();
    public List<GoodsReceivedNote> GoodsReceivedNotes { get; set; } = new();
}
