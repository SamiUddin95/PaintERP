using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class SalesInvoiceItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SalesInvoiceId { get; set; }

    [ForeignKey("SalesInvoiceId")]
    public SalesInvoice? SalesInvoice { get; set; }

    [Required]
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public int? PaintItemId { get; set; }

    [ForeignKey("PaintItemId")]
    public PaintItem? PaintItem { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Required]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal DiscountPercent { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal TaxPercent { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal StockBefore { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal StockAfter { get; set; }
}
