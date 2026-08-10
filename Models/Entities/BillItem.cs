using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class BillItem
{
    public int Id { get; set; }
    public int BillId { get; set; }

    // Item Information
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    public int? WarehouseId { get; set; }
    public int? PaintItemId { get; set; }

    // Quantity and Pricing
    public decimal Quantity { get; set; }
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty; // Gallon, Liter, etc.
    public decimal UnitCost { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal LineTotal { get; set; }

    // Navigation
    public Bill? Bill { get; set; }
    public Warehouse? Warehouse { get; set; }
    public PaintItem? PaintItem { get; set; }
}
