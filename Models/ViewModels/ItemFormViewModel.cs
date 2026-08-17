using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class ItemFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please select a warehouse")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid warehouse")]
    public int WarehouseId { get; set; }

    // Identification
    [Display(Name = "SKU")]
    [MaxLength(50)]
    public string? SKU { get; set; }

    [Display(Name = "UPC Barcode")]
    [MaxLength(50)]
    public string UPCBarcode { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Item Name")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Item Type")]
    [MaxLength(50)]
    public string ItemType { get; set; } = "Inventory"; // Inventory, Non Inventory, Service, Assembly, Raw Material, Finished Product

    [Display(Name = "Category")]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Display(Name = "Brand")]
    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [Display(Name = "Manufacturer")]
    [MaxLength(100)]
    public string Manufacturer { get; set; } = string.Empty;

    // Warehouse & Location
    [Display(Name = "Warehouse")]
    [MaxLength(100)]
    public string WarehouseName { get; set; } = string.Empty;

    [Display(Name = "Default Bin")]
    [MaxLength(50)]
    public string DefaultBin { get; set; } = string.Empty;

    [Display(Name = "Unit of Measure")]
    [MaxLength(20)]
    public string UnitOfMeasure { get; set; } = "GAL"; // GAL, QT, PT, L, ML

    [Display(Name = "Purchase Unit")]
    [MaxLength(20)]
    public string PurchaseUnit { get; set; } = "GAL";

    [Display(Name = "Sales Unit")]
    [MaxLength(20)]
    public string SalesUnit { get; set; } = "GAL";

    [Display(Name = "Cost Method")]
    [MaxLength(20)]
    public string CostMethod { get; set; } = "Average Cost"; // FIFO, Average Cost

    // Pricing
    [Display(Name = "Purchase Price")]
    public decimal PurchasePrice { get; set; }

    [Display(Name = "Selling Price")]
    public decimal SellingPrice { get; set; }

    [Display(Name = "MSRP")]
    public decimal MSRP { get; set; }

    // Inventory Levels
    [Display(Name = "Minimum Stock")]
    public decimal MinimumStock { get; set; }

    [Display(Name = "Maximum Stock")]
    public decimal MaximumStock { get; set; }

    [Display(Name = "Reorder Point")]
    public decimal ReorderPoint { get; set; }

    [Display(Name = "Preferred Vendor")]
    public int? PreferredVendorId { get; set; }

    // Accounting
    [Display(Name = "Sales Tax Category")]
    [MaxLength(50)]
    public string SalesTaxCategory { get; set; } = string.Empty;

    [Display(Name = "Inventory Asset Account")]
    [MaxLength(100)]
    public string InventoryAssetAccount { get; set; } = string.Empty;

    [Display(Name = "COGS Account")]
    [MaxLength(100)]
    public string COGSAccount { get; set; } = string.Empty;

    [Display(Name = "Income Account")]
    [MaxLength(100)]
    public string IncomeAccount { get; set; } = string.Empty;

    // Physical Properties
    [Display(Name = "Weight (lbs)")]
    public decimal Weight { get; set; }

    [Display(Name = "Dimensions")]
    [MaxLength(100)]
    public string Dimensions { get; set; } = string.Empty;

    [Display(Name = "Hazardous Material")]
    public bool IsHazardousMaterial { get; set; } = false;

    [Display(Name = "Lot Tracking")]
    public bool LotTracking { get; set; } = false;

    [Display(Name = "Batch Tracking")]
    public bool BatchTracking { get; set; } = false;

    [Display(Name = "Expiration Date")]
    public DateTime? ExpirationDate { get; set; }

    // Paint-specific (legacy fields)
    [Display(Name = "Color Family")]
    [MaxLength(100)]
    public string ColorFamily { get; set; } = string.Empty;

    [Display(Name = "Color Hex")]
    [MaxLength(20)]
    public string ColorHex { get; set; } = "#2266CC";

    [Display(Name = "Unit Cost")]
    public decimal UnitCost { get; set; }

    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }

    [Display(Name = "Reorder Level")]
    public int ReorderLevel { get; set; }

    // Documents
    [Display(Name = "Image Upload")]
    [MaxLength(500)]
    public string? ImagePath { get; set; }

    [Display(Name = "Attachments")]
    [MaxLength(500)]
    public string? AttachmentsPath { get; set; }

    // Notes
    [Display(Name = "Notes")]
    public string Notes { get; set; } = string.Empty;

    // Dashboard Metrics (read-only)
    public decimal CurrentStock { get; set; }
    public decimal AvailableStock { get; set; }
    public decimal ReservedStock { get; set; }
    public decimal InventoryValue { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public DateTime? LastSaleDate { get; set; }

    // Dropdown options
    public List<string> ItemTypes { get; } = new() { "Inventory", "Non Inventory", "Service", "Assembly", "Raw Material", "Finished Product" };
    public List<string> CostMethods { get; } = new() { "FIFO", "Average Cost" };
    public List<string> UnitsOfMeasure { get; } = new() { "GAL", "QT", "PT", "L", "ML", "OZ", "LB", "KG" };
    public List<string> Vendors { get; set; } = new();
    public List<Entities.Warehouse> Warehouses { get; set; } = new();
}
