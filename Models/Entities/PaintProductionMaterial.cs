namespace PaintERP.Models.Entities;

public class PaintProductionMaterial
{
    public int Id { get; set; }
    public int PaintProductionId { get; set; }
    public int? PaintItemId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
    public decimal RequiredQuantity { get; set; }
    public decimal ConsumedQuantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal? PercentageInMix { get; set; }
    public decimal StockBefore { get; set; }
    public decimal StockAfter { get; set; }

    public PaintProduction? PaintProduction { get; set; }
    public PaintItem? PaintItem { get; set; }
}
