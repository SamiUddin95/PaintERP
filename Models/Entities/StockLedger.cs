using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class StockLedger
{
    [Key]
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    public Company? Company { get; set; }

    public int? WarehouseId { get; set; }

    [ForeignKey("WarehouseId")]
    public Warehouse? Warehouse { get; set; }

    public int? PaintItemId { get; set; }

    [ForeignKey("PaintItemId")]
    public PaintItem? PaintItem { get; set; }

    [Required]
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(100)]
    public string TransactionType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ReferenceNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal InQty { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal OutQty { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal RunningBalance { get; set; } = 0;

    [Required]
    [MaxLength(200)]
    public string UserName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Remarks { get; set; }
}
