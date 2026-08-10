using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class InventoryTransfer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    public Company? Company { get; set; }

    [Required]
    [MaxLength(50)]
    public string TransferNumber { get; set; } = string.Empty;

    [Required]
    public int SourceWarehouseId { get; set; }

    [ForeignKey("SourceWarehouseId")]
    public Warehouse? SourceWarehouse { get; set; }

    [Required]
    public int DestinationWarehouseId { get; set; }

    [ForeignKey("DestinationWarehouseId")]
    public Warehouse? DestinationWarehouse { get; set; }

    [Required]
    public DateTime TransferDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    [MaxLength(50)]
    public string? TrackingNumber { get; set; }

    [MaxLength(100)]
    public string? ShippedBy { get; set; }

    [MaxLength(100)]
    public string? ReceivedBy { get; set; }

    public DateTime? ApprovedAtUtc { get; set; }

    public DateTime? ShippedAtUtc { get; set; }

    public DateTime? ReceivedAtUtc { get; set; }

    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    [Required]
    public string CreatedBy { get; set; } = string.Empty;

    [Required]
    public string UpdatedBy { get; set; } = string.Empty;

    public List<InventoryTransferItem> InventoryTransferItems { get; set; } = new();
}
