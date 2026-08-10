using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class VendorPayment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [ForeignKey("CompanyId")]
    public Company? Company { get; set; }

    [Required]
    public int VendorId { get; set; }

    [ForeignKey("VendorId")]
    public Vendor? Vendor { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(200)]
    public string BankAccount { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ReferenceNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PaymentNumber { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPaymentAmount { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalApplied { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnappliedAmount { get; set; } = 0;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    [Required]
    public bool IsPrinted { get; set; } = false;

    [Required]
    public bool IsReconciled { get; set; } = false;

    public DateTime? ReconciledDate { get; set; }

    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    [Required]
    public string CreatedBy { get; set; } = string.Empty;

    [Required]
    public string UpdatedBy { get; set; } = string.Empty;

    public List<VendorPaymentBill> VendorPaymentBills { get; set; } = new();
}
