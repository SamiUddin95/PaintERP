namespace PaintERP.Models.Entities;

public class PaintFormula
{
    public int Id { get; set; }
    public int? CompanyId { get; set; }
    public string FormulaName { get; set; } = string.Empty;
    public string FormulaCode { get; set; } = string.Empty;
    public string PaintColor { get; set; } = string.Empty;
    public string Finish { get; set; } = string.Empty;
    public string ContainerSize { get; set; } = string.Empty;
    public decimal TotalFormulaCost { get; set; }
    public decimal ExpectedYield { get; set; }
    public decimal WastePercentage { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal GrossMargin { get; set; }
    public int Version { get; set; }
    public int? ParentFormulaId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;

    public Company? Company { get; set; }
    public PaintFormula? ParentFormula { get; set; }
    public List<PaintFormulaItem> Items { get; set; } = new();
}
