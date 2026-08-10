using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintERP.Models.Entities;

public class Vendor
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [MaxLength(50)]
    public string VendorId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    [MaxLength(50)]
    public string VendorType { get; set; } = "Supplier"; // Supplier, Manufacturer, Distributor, Contractor

    // Business Information
    [Required]
    [MaxLength(200)]
    public string BusinessName { get; set; } = string.Empty;
    [MaxLength(200)]
    public string? LegalBusinessName { get; set; }
    [MaxLength(200)]
    public string? DBA { get; set; }
    [MaxLength(100)]
    public string? VendorCategory { get; set; }
    [MaxLength(20)]
    public string? FederalTaxId { get; set; }
    [MaxLength(20)]
    public string? StateTaxId { get; set; }
    [MaxLength(50)]
    public string? SalesTaxExemptionNumber { get; set; }
    [MaxLength(50)]
    public string? BusinessLicenseNumber { get; set; }
    [MaxLength(100)]
    public string? Industry { get; set; }

    // Primary Contact
    [MaxLength(100)]
    public string? ContactFirstName { get; set; }
    [MaxLength(100)]
    public string? ContactLastName { get; set; }
    [MaxLength(100)]
    public string? ContactJobTitle { get; set; }
    [MaxLength(150)]
    public string? ContactEmail { get; set; }
    [MaxLength(20)]
    public string? ContactMobilePhone { get; set; }
    [MaxLength(20)]
    public string? ContactOfficePhone { get; set; }
    [MaxLength(10)]
    public string? ContactExtension { get; set; }

    // Business Address
    [MaxLength(50)]
    public string BusinessCountry { get; set; } = "USA";
    [MaxLength(200)]
    public string? BusinessStreetAddress { get; set; }
    [MaxLength(100)]
    public string? BusinessSuiteApt { get; set; }
    [MaxLength(100)]
    public string? BusinessCity { get; set; }
    [MaxLength(50)]
    public string? BusinessState { get; set; }
    [MaxLength(20)]
    public string? BusinessZipCode { get; set; }
    [MaxLength(100)]
    public string? BusinessCounty { get; set; }

    // Shipping Address
    public bool SameAsBusinessAddress { get; set; } = true;
    [MaxLength(200)]
    public string? ShippingStreetAddress { get; set; }
    [MaxLength(100)]
    public string? ShippingCity { get; set; }
    [MaxLength(50)]
    public string? ShippingState { get; set; }
    [MaxLength(20)]
    public string? ShippingZipCode { get; set; }

    // Financial Information
    public decimal OpeningBalance { get; set; }
    public decimal CreditLimit { get; set; }
    [MaxLength(50)]
    public string PaymentTerms { get; set; } = "Net 30"; // Due on Receipt, Net 15, Net 30, Net 45, Net 60
    [MaxLength(50)]
    public string PreferredPaymentMethod { get; set; } = "Check"; // ACH, Wire Transfer, Check, Credit Card
    [MaxLength(100)]
    public string? DefaultExpenseAccount { get; set; }
    [MaxLength(50)]
    public string? SalesTaxCode { get; set; }
    public bool Is1099Vendor { get; set; } = false;
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    // Bank Details
    [MaxLength(200)]
    public string? BankName { get; set; }
    [MaxLength(20)]
    public string? RoutingNumber { get; set; }
    [MaxLength(30)]
    public string? AccountNumber { get; set; }
    [MaxLength(50)]
    public string? AccountType { get; set; }
    [MaxLength(20)]
    public string? SwiftCode { get; set; }

    // Purchase Settings
    [MaxLength(100)]
    public string? DefaultWarehouse { get; set; }
    [MaxLength(100)]
    public string? PreferredShippingMethod { get; set; }
    public int LeadTimeDays { get; set; }
    public decimal MinimumOrderQuantity { get; set; }
    public decimal DefaultDiscountPercent { get; set; }
    public decimal DefaultTaxRate { get; set; }

    // Attachments
    [MaxLength(500)]
    public string? W9FormPath { get; set; }
    [MaxLength(500)]
    public string? VendorContractPath { get; set; }
    [MaxLength(500)]
    public string? InsuranceCertificatePath { get; set; }
    [MaxLength(500)]
    public string? AdditionalDocumentsPath { get; set; }

    // Notes
    [Column(TypeName = "ntext")]
    public string? InternalNotes { get; set; }

    // Dashboard Metrics
    public DateTime VendorSince { get; set; } = DateTime.UtcNow;
    public int TotalPurchaseOrders { get; set; }
    public int TotalBills { get; set; }
    public decimal OutstandingAmount { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public int AveragePaymentDays { get; set; }
    public int VendorRating { get; set; } = 5; // 1-5
    public bool IsPreferredVendor { get; set; } = false;

    // Audit
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;

    // Navigation
    public Company? Company { get; set; }
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
