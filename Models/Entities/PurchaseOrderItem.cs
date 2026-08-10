namespace PaintERP.Models.Entities;

public class PurchaseOrderItem
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? PaintItemId { get; set; }
    public decimal QuantityOrdered { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal QuantityPending { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal LineTotal { get; set; }
    public bool IsFullyReceived { get; set; }

    public PurchaseOrder? PurchaseOrder { get; set; }
    public PaintItem? PaintItem { get; set; }
}
