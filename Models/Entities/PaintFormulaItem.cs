namespace PaintERP.Models.Entities;

public class PaintFormulaItem
{
    public int Id { get; set; }
    public int PaintFormulaId { get; set; }
    public int? PaintItemId { get; set; }
    public string RawMaterialName { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
    public decimal RequiredQuantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
    public decimal InventoryAvailable { get; set; }

    public PaintFormula? PaintFormula { get; set; }
    public PaintItem? PaintItem { get; set; }
}
