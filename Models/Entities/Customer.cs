using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class Customer
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    [MaxLength(50)]
    public string CustomerType { get; set; } = "Commercial"; // Residential, Commercial, Contractor, Dealer, Wholesale, Retail

    // Business Information
    [Required]
    [MaxLength(200)]
    public string BusinessName { get; set; } = string.Empty;
    [MaxLength(200)]
    public string? LegalCompanyName { get; set; }
    [MaxLength(200)]
    public string? DBA { get; set; }
    [MaxLength(20)]
    public string? FederalTaxId { get; set; }
    [MaxLength(50)]
    public string? ResaleCertificateNumber { get; set; }
    [MaxLength(100)]
    public string? Industry { get; set; }
    [MaxLength(100)]
    public string? CustomerGroup { get; set; }
    [MaxLength(100)]
    public string? SalesRepresentative { get; set; }

    // Primary Contact
    [MaxLength(100)]
    public string? ContactFirstName { get; set; }
    [MaxLength(100)]
    public string? ContactLastName { get; set; }
    [MaxLength(100)]
    public string? ContactTitle { get; set; }
    [MaxLength(150)]
    public string? ContactEmail { get; set; }
    [MaxLength(20)]
    public string? ContactMobile { get; set; }
    [MaxLength(20)]
    public string? ContactOfficePhone { get; set; }
    [MaxLength(10)]
    public string? ContactExtension { get; set; }

    // Billing Address
    [MaxLength(50)]
    public string BillingCountry { get; set; } = "USA";
    [MaxLength(200)]
    public string? BillingStreetAddress { get; set; }
    [MaxLength(100)]
    public string? BillingSuite { get; set; }
    [MaxLength(100)]
    public string? BillingCity { get; set; }
    [MaxLength(50)]
    public string? BillingState { get; set; }
    [MaxLength(20)]
    public string? BillingZipCode { get; set; }
    [MaxLength(100)]
    public string? BillingCounty { get; set; }

    // Shipping Address
    public bool SameAsBillingAddress { get; set; } = true;
    [MaxLength(200)]
    public string? ShippingStreetAddress { get; set; }
    [MaxLength(100)]
    public string? ShippingCity { get; set; }
    [MaxLength(50)]
    public string? ShippingState { get; set; }
    [MaxLength(20)]
    public string? ShippingZipCode { get; set; }

    // Financial Settings
    public decimal OpeningBalance { get; set; }
    public decimal CreditLimit { get; set; }
    [MaxLength(50)]
    public string PaymentTerms { get; set; } = "Net 30"; // Due on Receipt, Net 15, Net 30, Net 45, Net 60
    [MaxLength(50)]
    public string PreferredPaymentMethod { get; set; } = "Check"; // ACH, Wire, Credit Card, Cash, Check
    [MaxLength(50)]
    public string? SalesTaxCode { get; set; }
    public bool IsTaxExempt { get; set; } = false;
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";
    [MaxLength(100)]
    public string? DefaultPriceList { get; set; }
    public decimal CustomerDiscountPercent { get; set; }

    // Delivery Information
    [MaxLength(100)]
    public string? PreferredWarehouse { get; set; }
    [MaxLength(100)]
    public string? DeliveryMethod { get; set; }
    [Column(TypeName = "ntext")]
    public string? DeliveryInstructions { get; set; }
    [MaxLength(200)]
    public string? ReceivingHours { get; set; }
    [MaxLength(200)]
    public string? LoadingDockInfo { get; set; }

    // Paint Industry Information
    [MaxLength(100)]
    public string? PreferredPaintBrand { get; set; }
    [MaxLength(200)]
    public string? PreferredColorCodes { get; set; }
    [MaxLength(500)]
    public string? FavoriteProducts { get; set; }
    [MaxLength(50)]
    public string? ProjectType { get; set; } // Interior, Exterior, Industrial, Automotive, Construction, Commercial, Residential

    // Documents
    [MaxLength(500)]
    public string? ResaleCertificatePath { get; set; }
    [MaxLength(500)]
    public string? ContractPath { get; set; }
    [MaxLength(500)]
    public string? CreditApplicationPath { get; set; }
    [MaxLength(500)]
    public string? OtherDocumentsPath { get; set; }

    // Notes
    [Column(TypeName = "ntext")]
    public string? InternalNotes { get; set; }

    // Dashboard Metrics
    public DateTime CustomerSince { get; set; } = DateTime.UtcNow;
    public decimal TotalSales { get; set; }
    public decimal OutstandingBalance { get; set; }
    public DateTime? LastInvoiceDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public decimal LifetimeRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public int CustomerRating { get; set; } = 5; // 1-5
    public bool IsVIP { get; set; } = false;

    // Audit
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;

    // Navigation
    public Company? Company { get; set; }
    public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
