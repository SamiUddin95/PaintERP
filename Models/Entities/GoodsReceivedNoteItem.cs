namespace PaintERP.Models.Entities;

public class GoodsReceivedNoteItem
{
    public int Id { get; set; }
    public int GoodsReceivedNoteId { get; set; }
    public int PurchaseOrderItemId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? PaintItemId { get; set; }
    public decimal QuantityOrdered { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal QuantityPreviouslyReceived { get; set; }
    public decimal QuantityRemaining { get; set; }
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

    public GoodsReceivedNote? GoodsReceivedNote { get; set; }
    public PurchaseOrderItem? PurchaseOrderItem { get; set; }
    public PaintItem? PaintItem { get; set; }
}
