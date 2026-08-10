using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class InventoryTransferItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int InventoryTransferId { get; set; }

    [ForeignKey("InventoryTransferId")]
    public InventoryTransfer? InventoryTransfer { get; set; }

    public int? PaintItemId { get; set; }

    [ForeignKey("PaintItemId")]
    public PaintItem? PaintItem { get; set; }

    [Required]
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Required]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SourceStockBefore { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SourceStockAfter { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DestinationStockBefore { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DestinationStockAfter { get; set; }
}
