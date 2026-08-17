namespace PaintERP.Models.Entities;

public class PaintProduction
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string ProductionNumber { get; set; } = string.Empty;
    public string Recipe { get; set; } = string.Empty;
    public string BatchNumber { get; set; } = string.Empty;
    public DateTime ProductionDate { get; set; }
    public int WarehouseId { get; set; }
    public decimal OutputQuantity { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public int? FinishedProductId { get; set; }
    public int? FormulaId { get; set; }
    public string FinishedProductDescription { get; set; } = string.Empty;
    public decimal MaterialCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal OverheadCost { get; set; }
    public decimal ProductionCost { get; set; }
    public decimal FinishedProductCost { get; set; }
    public decimal CostPerUnit { get; set; }
    public string? BatchLabel { get; set; }
    public string? QCStatus { get; set; }
    public string? QCReport { get; set; }
    public string? QCNotes { get; set; }
    public string? ProductionNotes { get; set; }
    public bool CreateNewItem { get; set; }
    public string? NewItemName { get; set; }
    public string? NewItemSKU { get; set; }
    public string? NewItemCategory { get; set; }
    public string? NewItemUnitOfMeasure { get; set; }
    public decimal? NewItemSellingPrice { get; set; }
    public decimal? NewItemCalculatedUnitCost { get; set; }
    public string? NewItemDescription { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;

    public Company? Company { get; set; }
    public Warehouse? Warehouse { get; set; }
    public PaintItem? FinishedProduct { get; set; }
    public PaintFormula? Formula { get; set; }
    public List<PaintProductionMaterial> Materials { get; set; } = new();
}
