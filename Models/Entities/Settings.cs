using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

[Table("AppSettings")]
public class AppSettings
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    // Company Information
    [Required]
    [MaxLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? CompanyAddress { get; set; }

    [MaxLength(50)]
    public string? CompanyPhone { get; set; }

    [MaxLength(150)]
    public string? CompanyEmail { get; set; }

    [MaxLength(20)]
    public string? CompanyTaxId { get; set; }

    [MaxLength(500)]
    public string? CompanyLogoUrl { get; set; }

    [Required]
    [MaxLength(20)]
    public string FiscalYearStart { get; set; } = "01-01";

    [Required]
    [MaxLength(20)]
    public string FiscalYearEnd { get; set; } = "12-31";

    // Accounting & Tax
    [Required]
    [MaxLength(50)]
    public string DefaultCurrency { get; set; } = "USD";

    [Required]
    [MaxLength(50)]
    public string DefaultPaymentTerms { get; set; } = "Net 30";

    [Required]
    [MaxLength(50)]
    public string AccountingMethod { get; set; } = "Accrual";

    [MaxLength(50)]
    public string? DefaultTaxCode { get; set; }

    [Required]
    public decimal DefaultTaxPercent { get; set; }

    [Required]
    [MaxLength(500)]
    public string ShippingCarriers { get; set; } = "UPS;FedEx;USPS;DHL";

    [Required]
    [MaxLength(500)]
    public string Currencies { get; set; } = "USD";

    [MaxLength(1000)]
    public string? ApprovalWorkflow { get; set; }

    // Email & Invoice Templates
    [Column(TypeName = "nvarchar(max)")]
    public string? InvoiceEmailTemplate { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? PaymentReceiptTemplate { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? PurchaseOrderEmailTemplate { get; set; }

    // Number Sequences
    [Required]
    [MaxLength(50)]
    public string BarcodeType { get; set; } = "Code128";

    [Required]
    [MaxLength(50)]
    public string InvoiceNumberPrefix { get; set; } = "INV-";

    [Required]
    [MaxLength(50)]
    public string PurchaseOrderNumberPrefix { get; set; } = "PO-";

    [Required]
    [MaxLength(50)]
    public string BillNumberPrefix { get; set; } = "BILL-";

    [Required]
    [MaxLength(50)]
    public string TransferNumberPrefix { get; set; } = "IT-";

    // API Integrations
    [MaxLength(500)]
    public string? QuickBooksApiKey { get; set; }

    [MaxLength(500)]
    public string? ShopifyApiKey { get; set; }

    [MaxLength(500)]
    public string? AmazonApiKey { get; set; }

    [MaxLength(500)]
    public string? UpsApiKey { get; set; }

    [MaxLength(500)]
    public string? FedExApiKey { get; set; }

    [MaxLength(500)]
    public string? UspsApiKey { get; set; }
}
