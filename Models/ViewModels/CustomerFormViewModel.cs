using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class CustomerFormViewModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [Display(Name = "Customer ID")]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Display(Name = "Customer Status")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Customer Type")]
    [MaxLength(50)]
    public string CustomerType { get; set; } = "Commercial"; // Residential, Commercial, Contractor, Dealer, Wholesale, Retail

    // Business Information
    [Required]
    [Display(Name = "Business Name")]
    [MaxLength(200)]
    public string BusinessName { get; set; } = string.Empty;

    [Display(Name = "Legal Company Name")]
    [MaxLength(200)]
    public string? LegalCompanyName { get; set; }

    [Display(Name = "DBA (Doing Business As)")]
    [MaxLength(200)]
    public string? DBA { get; set; }

    [Display(Name = "Federal Tax ID (EIN)")]
    [MaxLength(20)]
    public string? FederalTaxId { get; set; }

    [Display(Name = "Resale Certificate Number")]
    [MaxLength(50)]
    public string? ResaleCertificateNumber { get; set; }

    [Display(Name = "Industry")]
    [MaxLength(100)]
    public string? Industry { get; set; }

    [Display(Name = "Customer Group")]
    [MaxLength(100)]
    public string? CustomerGroup { get; set; }

    [Display(Name = "Sales Representative")]
    [MaxLength(100)]
    public string? SalesRepresentative { get; set; }

    // Primary Contact
    [Display(Name = "First Name")]
    [MaxLength(100)]
    public string? ContactFirstName { get; set; }

    [Display(Name = "Last Name")]
    [MaxLength(100)]
    public string? ContactLastName { get; set; }

    [Display(Name = "Title")]
    [MaxLength(100)]
    public string? ContactTitle { get; set; }

    [Display(Name = "Email")]
    [MaxLength(150)]
    public string? ContactEmail { get; set; }

    [Display(Name = "Mobile")]
    [MaxLength(20)]
    public string? ContactMobile { get; set; }

    [Display(Name = "Office Phone")]
    [MaxLength(20)]
    public string? ContactOfficePhone { get; set; }

    [Display(Name = "Extension")]
    [MaxLength(10)]
    public string? ContactExtension { get; set; }

    // Billing Address
    [Display(Name = "Country")]
    [MaxLength(50)]
    public string BillingCountry { get; set; } = "USA";

    [Display(Name = "Street Address")]
    [MaxLength(200)]
    public string? BillingStreetAddress { get; set; }

    [Display(Name = "Suite")]
    [MaxLength(100)]
    public string? BillingSuite { get; set; }

    [Display(Name = "City")]
    [MaxLength(100)]
    public string? BillingCity { get; set; }

    [Display(Name = "State")]
    [MaxLength(50)]
    public string? BillingState { get; set; }

    [Display(Name = "ZIP Code")]
    [MaxLength(20)]
    public string? BillingZipCode { get; set; }

    [Display(Name = "County")]
    [MaxLength(100)]
    public string? BillingCounty { get; set; }

    // Shipping Address
    [Display(Name = "Same as Billing Address")]
    public bool SameAsBillingAddress { get; set; } = true;

    [Display(Name = "Shipping Street")]
    [MaxLength(200)]
    public string? ShippingStreetAddress { get; set; }

    [Display(Name = "City")]
    [MaxLength(100)]
    public string? ShippingCity { get; set; }

    [Display(Name = "State")]
    [MaxLength(50)]
    public string? ShippingState { get; set; }

    [Display(Name = "ZIP Code")]
    [MaxLength(20)]
    public string? ShippingZipCode { get; set; }

    // Financial Settings
    [Display(Name = "Opening Balance")]
    public decimal OpeningBalance { get; set; }

    [Display(Name = "Credit Limit")]
    public decimal CreditLimit { get; set; }

    [Display(Name = "Payment Terms")]
    [MaxLength(50)]
    public string PaymentTerms { get; set; } = "Net 30"; // Due on Receipt, Net 15, Net 30, Net 45, Net 60

    [Display(Name = "Preferred Payment Method")]
    [MaxLength(50)]
    public string PreferredPaymentMethod { get; set; } = "Check"; // ACH, Wire, Credit Card, Cash, Check

    [Display(Name = "Sales Tax Code")]
    [MaxLength(50)]
    public string? SalesTaxCode { get; set; }

    [Display(Name = "Tax Exempt")]
    public bool IsTaxExempt { get; set; } = false;

    [Display(Name = "Currency")]
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    [Display(Name = "Default Price List")]
    [MaxLength(100)]
    public string? DefaultPriceList { get; set; }

    [Display(Name = "Customer Discount %")]
    public decimal CustomerDiscountPercent { get; set; }

    // Delivery Information
    [Display(Name = "Preferred Warehouse")]
    [MaxLength(100)]
    public string? PreferredWarehouse { get; set; }

    [Display(Name = "Delivery Method")]
    [MaxLength(100)]
    public string? DeliveryMethod { get; set; }

    [Display(Name = "Delivery Instructions")]
    public string? DeliveryInstructions { get; set; }

    [Display(Name = "Receiving Hours")]
    [MaxLength(200)]
    public string? ReceivingHours { get; set; }

    [Display(Name = "Loading Dock Information")]
    [MaxLength(200)]
    public string? LoadingDockInfo { get; set; }

    // Paint Industry Information
    [Display(Name = "Preferred Paint Brand")]
    [MaxLength(100)]
    public string? PreferredPaintBrand { get; set; }

    [Display(Name = "Preferred Color Codes")]
    [MaxLength(200)]
    public string? PreferredColorCodes { get; set; }

    [Display(Name = "Favorite Products")]
    [MaxLength(500)]
    public string? FavoriteProducts { get; set; }

    [Display(Name = "Project Type")]
    [MaxLength(50)]
    public string? ProjectType { get; set; } // Interior, Exterior, Industrial, Automotive, Construction, Commercial, Residential

    // Documents
    [Display(Name = "Resale Certificate")]
    [MaxLength(500)]
    public string? ResaleCertificatePath { get; set; }

    [Display(Name = "Contract")]
    [MaxLength(500)]
    public string? ContractPath { get; set; }

    [Display(Name = "Credit Application")]
    [MaxLength(500)]
    public string? CreditApplicationPath { get; set; }

    [Display(Name = "Other Documents")]
    [MaxLength(500)]
    public string? OtherDocumentsPath { get; set; }

    // Notes
    [Display(Name = "Internal Notes")]
    public string? InternalNotes { get; set; }

    // Dashboard Metrics (read-only)
    public DateTime CustomerSince { get; set; } = DateTime.UtcNow;
    public decimal TotalSales { get; set; }
    public decimal OutstandingBalance { get; set; }
    public DateTime? LastInvoiceDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public decimal LifetimeRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public int CustomerRating { get; set; } = 5; // 1-5
    public bool IsVIP { get; set; } = false;

    // Dropdown options
    public List<string> CustomerTypes { get; } = new() { "Residential", "Commercial", "Contractor", "Dealer", "Wholesale", "Retail" };
    public List<string> PaymentTermsOptions { get; } = new() { "Due on Receipt", "Net 15", "Net 30", "Net 45", "Net 60" };
    public List<string> PaymentMethods { get; } = new() { "ACH", "Wire", "Credit Card", "Cash", "Check" };
    public List<string> ProjectTypes { get; } = new() { "Interior", "Exterior", "Industrial", "Automotive", "Construction", "Commercial", "Residential" };
    public List<string> USStates { get; } = new()
    {
        "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA",
        "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
        "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
        "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
        "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
    };
}
