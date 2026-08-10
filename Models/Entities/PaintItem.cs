using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class PaintItem
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }

    // Identification
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;
    [MaxLength(50)]
    public string UPCBarcode { get; set; } = string.Empty;
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(50)]
    public string ItemType { get; set; } = "Inventory"; // Inventory, Non Inventory, Service, Assembly, Raw Material, Finished Product
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Manufacturer { get; set; } = string.Empty;

    // Warehouse & Location
    [MaxLength(100)]
    public string WarehouseName { get; set; } = string.Empty;
    [MaxLength(50)]
    public string DefaultBin { get; set; } = string.Empty;
    [MaxLength(20)]
    public string UnitOfMeasure { get; set; } = "GAL"; // GAL, QT, PT, L, ML
    [MaxLength(20)]
    public string PurchaseUnit { get; set; } = "GAL";
    [MaxLength(20)]
    public string SalesUnit { get; set; } = "GAL";
    [MaxLength(20)]
    public string CostMethod { get; set; } = "Average Cost"; // FIFO, Average Cost

    // Pricing
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal MSRP { get; set; }

    // Inventory Levels
    public decimal MinimumStock { get; set; }
    public decimal MaximumStock { get; set; }
    public decimal ReorderPoint { get; set; }
    public int? PreferredVendorId { get; set; }

    // Accounting
    [MaxLength(50)]
    public string SalesTaxCategory { get; set; } = string.Empty;
    [MaxLength(100)]
    public string InventoryAssetAccount { get; set; } = string.Empty;
    [MaxLength(100)]
    public string COGSAccount { get; set; } = string.Empty;
    [MaxLength(100)]
    public string IncomeAccount { get; set; } = string.Empty;

    // Physical Properties
    public decimal Weight { get; set; }
    [MaxLength(100)]
    public string Dimensions { get; set; } = string.Empty;
    public bool IsHazardousMaterial { get; set; } = false;
    public bool LotTracking { get; set; } = false;
    public bool BatchTracking { get; set; } = false;
    public DateTime? ExpirationDate { get; set; }

    // Paint-specific (legacy fields)
    [MaxLength(100)]
    public string ColorFamily { get; set; } = string.Empty;
    [MaxLength(20)]
    public string ColorHex { get; set; } = "#2266CC";
    public decimal UnitCost { get; set; }
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; }

    // Documents
    [MaxLength(500)]
    public string? ImagePath { get; set; }
    [MaxLength(500)]
    public string? AttachmentsPath { get; set; }

    // Notes
    [Column(TypeName = "ntext")]
    public string Notes { get; set; } = string.Empty;

    // Dashboard Metrics (read-only)
    public decimal CurrentStock { get; set; }
    public decimal AvailableStock { get; set; }
    public decimal ReservedStock { get; set; }
    public decimal InventoryValue { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public DateTime? LastSaleDate { get; set; }

    // Audit
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;

    // Navigation
    public Warehouse? Warehouse { get; set; }
    public Vendor? PreferredVendor { get; set; }
}
