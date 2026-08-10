using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class SalesInvoice
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    public Company? Company { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string Salesperson { get; set; } = string.Empty;

    public int? WarehouseId { get; set; }

    [ForeignKey("WarehouseId")]
    public Warehouse? Warehouse { get; set; }

    [Required]
    [MaxLength(100)]
    public string ShippingMethod { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string TrackingNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PaymentTerms { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ReferenceNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    [Required]
    public bool IsPaid { get; set; } = false;

    [Required]
    public bool IsVoided { get; set; } = false;

    public DateTime? PaidDate { get; set; }

    public DateTime? VoidedDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingCost { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SalesTaxAmount { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GrandTotal { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BalanceDue { get; set; } = 0;

    [MaxLength(500)]
    public string? AttachmentUrl { get; set; }

    public string? InternalNotes { get; set; }

    public string? CustomerNotes { get; set; }

    [Required]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    [Required]
    public string CreatedBy { get; set; } = string.Empty;

    [Required]
    public string UpdatedBy { get; set; } = string.Empty;

    public List<SalesInvoiceItem> SalesInvoiceItems { get; set; } = new();
}
