using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class InventoryTransferFormViewModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [Required]
    [Display(Name = "Transfer Number")]
    [MaxLength(50)]
    public string TransferNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Transfer Date")]
    public DateTime TransferDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Display(Name = "Source Warehouse")]
    public int SourceWarehouseId { get; set; }

    [Required]
    [Display(Name = "Destination Warehouse")]
    public int DestinationWarehouseId { get; set; }

    [Required]
    [Display(Name = "Status")]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    [Display(Name = "Tracking Number")]
    [MaxLength(50)]
    public string? TrackingNumber { get; set; }

    [Display(Name = "Shipped By")]
    [MaxLength(100)]
    public string? ShippedBy { get; set; }

    [Display(Name = "Received By")]
    [MaxLength(100)]
    public string? ReceivedBy { get; set; }

    public DateTime? ApprovedAtUtc { get; set; }
    public DateTime? ShippedAtUtc { get; set; }
    public DateTime? ReceivedAtUtc { get; set; }

    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    // Line Items
    public List<InventoryTransferItemViewModel> InventoryTransferItems { get; set; } = new List<InventoryTransferItemViewModel>();

    // Dropdown Options
    public List<InventoryTransferWarehouseListItem> Warehouses { get; set; } = new List<InventoryTransferWarehouseListItem>();
    public List<InventoryTransferPaintItemListItem> PaintItems { get; set; } = new List<InventoryTransferPaintItemListItem>();
    public List<string> StatusOptions { get; } = new() { "Draft", "Approved", "Shipped", "In Transit", "Received", "Cancelled" };
}

public class InventoryTransferItemViewModel
{
    public int Id { get; set; }
    public int InventoryTransferId { get; set; }

    [Display(Name = "SKU")]
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;

    [Display(Name = "Description")]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Paint Item")]
    public int? PaintItemId { get; set; }

    [Required]
    [Display(Name = "Quantity")]
    public decimal Quantity { get; set; }

    [Required]
    [Display(Name = "Unit")]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Display(Name = "Batch Number")]
    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    [Display(Name = "Source Stock Before")]
    public decimal SourceStockBefore { get; set; }

    [Display(Name = "Source Stock After")]
    public decimal SourceStockAfter { get; set; }

    [Display(Name = "Destination Stock Before")]
    public decimal DestinationStockBefore { get; set; }

    [Display(Name = "Destination Stock After")]
    public decimal DestinationStockAfter { get; set; }
}

public class InventoryTransferWarehouseListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class InventoryTransferPaintItemListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
}
