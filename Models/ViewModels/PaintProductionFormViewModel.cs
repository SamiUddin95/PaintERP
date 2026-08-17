using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class PaintProductionFormViewModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    [Required]
    [Display(Name = "Production Number")]
    [MaxLength(50)]
    public string ProductionNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Recipe")]
    [MaxLength(200)]
    public string Recipe { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Batch Number")]
    [MaxLength(50)]
    public string BatchNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Production Date")]
    public DateTime ProductionDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Display(Name = "Warehouse")]
    public int WarehouseId { get; set; }

    [Required]
    [Display(Name = "Output Quantity")]
    public decimal OutputQuantity { get; set; }

    [Display(Name = "Status")]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [Display(Name = "Finished Product")]
    public int? FinishedProductId { get; set; }

    [Display(Name = "Create New Item")]
    public bool CreateNewItem { get; set; } = false;

    public NewItemDetailsViewModel NewItemDetails { get; set; } = new NewItemDetailsViewModel();

    [Display(Name = "Formula")]
    public int? FormulaId { get; set; }

    [Display(Name = "Finished Product Description")]
    [MaxLength(500)]
    public string FinishedProductDescription { get; set; } = string.Empty;

    [Display(Name = "Material Cost")]
    public decimal MaterialCost { get; set; }

    [Display(Name = "Labor Cost")]
    public decimal LaborCost { get; set; }

    [Display(Name = "Overhead Cost")]
    public decimal OverheadCost { get; set; }

    [Display(Name = "Production Cost")]
    public decimal ProductionCost { get; set; }

    [Display(Name = "Finished Product Cost")]
    public decimal FinishedProductCost { get; set; }

    [Display(Name = "Cost Per Unit")]
    public decimal CostPerUnit { get; set; }

    [Display(Name = "Batch Label")]
    [MaxLength(50)]
    public string? BatchLabel { get; set; }

    [Display(Name = "QC Status")]
    [MaxLength(50)]
    public string? QCStatus { get; set; }

    [Display(Name = "QC Report")]
    public string? QCReport { get; set; }

    [Display(Name = "QC Notes")]
    public string? QCNotes { get; set; }

    [Display(Name = "Production Notes")]
    public string? ProductionNotes { get; set; }

    public List<PaintProductionMaterialViewModel> Materials { get; set; } = new List<PaintProductionMaterialViewModel>();

    public List<PaintProductionWarehouseListItem> Warehouses { get; set; } = new List<PaintProductionWarehouseListItem>();
    public List<PaintProductionPaintItemListItem> PaintItems { get; set; } = new List<PaintProductionPaintItemListItem>();
    public List<PaintProductionFormulaListItem> Formulas { get; set; } = new List<PaintProductionFormulaListItem>();
    public List<string> StatusOptions { get; } = new() { "Pending", "In Progress", "Completed", "On Hold", "Cancelled" };
    public List<string> QCStatusOptions { get; } = new() { "Pending", "Passed", "Failed", "In Review" };
}

public class PaintProductionMaterialViewModel
{
    public int Id { get; set; }
    public int PaintProductionId { get; set; }

    [Display(Name = "Material Name")]
    [MaxLength(100)]
    public string MaterialName { get; set; } = string.Empty;

    [Display(Name = "Material Type")]
    [MaxLength(50)]
    public string MaterialType { get; set; } = string.Empty;

    [Display(Name = "Paint Item")]
    public int? PaintItemId { get; set; }

    [Display(Name = "Required Quantity")]
    public decimal RequiredQuantity { get; set; }

    [Display(Name = "Consumed Quantity")]
    public decimal ConsumedQuantity { get; set; }

    [Display(Name = "Unit")]
    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    [Display(Name = "Unit Cost")]
    public decimal UnitCost { get; set; }

    [Display(Name = "Total Cost")]
    public decimal TotalCost { get; set; }

    [Display(Name = "Percentage in Mix")]
    public decimal? PercentageInMix { get; set; }

    [Display(Name = "Stock Before")]
    public decimal StockBefore { get; set; }

    [Display(Name = "Stock After")]
    public decimal StockAfter { get; set; }
}

public class PaintProductionWarehouseListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class PaintProductionPaintItemListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
    public int WarehouseId { get; set; }
    public decimal CurrentStock { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
}

public class PaintProductionFormulaListItem
{
    public int Id { get; set; }
    public string FormulaName { get; set; } = string.Empty;
}

public class NewItemDetailsViewModel
{
    // Validated conditionally in the controller (only when CreateNewItem is checked)
    [Display(Name = "Item Name")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "SKU")]
    [MaxLength(50)]
    public string? SKU { get; set; }

    [Display(Name = "Category")]
    [MaxLength(100)]
    public string Category { get; set; } = "Finished Product";

    [Display(Name = "Unit of Measure")]
    [MaxLength(20)]
    public string UnitOfMeasure { get; set; } = "GAL";

    [Display(Name = "Selling Price")]
    public decimal? SellingPrice { get; set; }

    [Display(Name = "Description")]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Calculated Unit Cost")]
    public decimal? CalculatedUnitCost { get; set; }
}
