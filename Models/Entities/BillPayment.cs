using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class BillPayment
{
    public int Id { get; set; }
    public int BillId { get; set; }
    public int CompanyId { get; set; }

    // Payment Information
    [Required]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    [MaxLength(50)]
    public string PaymentNumber { get; set; } = string.Empty;
    [MaxLength(50)]
    public string ReferenceNumber { get; set; } = string.Empty;
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty; // Check, ACH, Wire, Credit Card
    [MaxLength(50)]
    public string PaymentAccount { get; set; } = string.Empty;
    public decimal PaymentAmount { get; set; }
    public decimal DiscountTaken { get; set; }
    public decimal WriteOffAmount { get; set; }

    // Bank Details
    [MaxLength(200)]
    public string BankName { get; set; } = string.Empty;
    [MaxLength(20)]
    public string CheckNumber { get; set; } = string.Empty;

    // Notes
    [Column(TypeName = "ntext")]
    public string? Notes { get; set; }

    // Audit
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;

    // Navigation
    public Bill? Bill { get; set; }
    public Company? Company { get; set; }
}
