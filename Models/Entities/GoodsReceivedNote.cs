namespace PaintERP.Models.Entities;

public class GoodsReceivedNote
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int PurchaseOrderId { get; set; }
    public string GRNNumber { get; set; } = string.Empty;
    public DateTime GRNDate { get; set; }
    public int? WarehouseId { get; set; }
    public int VendorId { get; set; }
    public string? ReferenceNumber { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Received, Verified, Posted
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
    public DateTime? ReceivedDate { get; set; }
    public string? ReceivedBy { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public string? VerifiedBy { get; set; }
    public bool IsPosted { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? PostedBy { get; set; }
    public int? ConvertedBillId { get; set; }

    public Company? Company { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public Vendor? Vendor { get; set; }
    public Warehouse? Warehouse { get; set; }
    public List<GoodsReceivedNoteItem> GoodsReceivedNoteItems { get; set; } = new();
}
