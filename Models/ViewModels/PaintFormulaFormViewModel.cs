using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class PaintFormulaFormViewModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FormulaName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string FormulaCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string PaintColor { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Finish { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ContainerSize { get; set; } = string.Empty;

    [Required]
    public decimal TotalFormulaCost { get; set; }

    public decimal ExpectedYield { get; set; }

    [Required]
    public decimal WastePercentage { get; set; }

    [Required]
    public decimal SellingPrice { get; set; }

    public decimal GrossMargin { get; set; }

    public int Version { get; set; } = 1;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    public List<PaintFormulaItemViewModel> FormulaItems { get; set; } = new();
}

public class PaintFormulaItemViewModel
{
    public int Id { get; set; }
    public int PaintFormulaId { get; set; }
    public int? PaintItemId { get; set; }
    public string RawMaterialName { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
    public decimal RequiredQuantity { get; set; }
    public string Unit { get; set; } = "Each";
    public decimal UnitCost { get; set; }
    public decimal InventoryAvailable { get; set; }
}
