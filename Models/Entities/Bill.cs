using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class Bill
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [Required]
    public int VendorId { get; set; }
    [MaxLength(50)]
    public string BillNumber { get; set; } = string.Empty;
    [MaxLength(50)]
    public string VendorInvoiceNumber { get; set; } = string.Empty;
    public int? PurchaseOrderId { get; set; }
    public int? GRNId { get; set; }
    [Required]
    public DateTime BillDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public int? WarehouseId { get; set; }
    [MaxLength(50)]
    public string PaymentTerms { get; set; } = "Net 30";
    [MaxLength(100)]
    public string ShippingMethod { get; set; } = string.Empty;
    [MaxLength(50)]
    public string ReferenceNumber { get; set; } = string.Empty;
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";
    [MaxLength(50)]
    public string TaxCode { get; set; } = string.Empty;

    // Status
    [MaxLength(50)]
    public string Status { get; set; } = "Open"; // Open, Approved, Paid, Void
    public bool IsApproved { get; set; } = false;
    public bool IsVoid { get; set; } = false;
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidDate { get; set; }

    // Financial Summary
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ShippingCharges { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal OtherCharges { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal AmountPaid { get; set; } = 0;
    public decimal BalanceDue { get; set; }

    // Attachments
    [MaxLength(500)]
    public string? AttachmentPath { get; set; }

    // Notes
    [Column(TypeName = "ntext")]
    public string? InternalNotes { get; set; }
    [Column(TypeName = "ntext")]
    public string? VendorNotes { get; set; }

    // Audit
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;

    // Navigation
    public Company? Company { get; set; }
    public Vendor? Vendor { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public GoodsReceivedNote? GoodsReceivedNote { get; set; }
    public Warehouse? Warehouse { get; set; }
    public ICollection<BillItem> BillItems { get; set; } = new List<BillItem>();
    public ICollection<BillPayment> BillPayments { get; set; } = new List<BillPayment>();
}
