using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class VendorFormViewModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [Display(Name = "Vendor ID")]
    [MaxLength(50)]
    public string VendorId { get; set; } = string.Empty;

    [Display(Name = "Vendor Status")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Vendor Type")]
    [MaxLength(50)]
    public string VendorType { get; set; } = "Supplier";

    // Business Information
    [Required]
    [Display(Name = "Business Name")]
    [MaxLength(200)]
    public string BusinessName { get; set; } = string.Empty;

    [Display(Name = "Legal Business Name")]
    [MaxLength(200)]
    public string? LegalBusinessName { get; set; }

    [Display(Name = "DBA (Doing Business As)")]
    [MaxLength(200)]
    public string? DBA { get; set; }

    [Display(Name = "Vendor Category")]
    [MaxLength(100)]
    public string? VendorCategory { get; set; }

    [Display(Name = "Federal Tax ID (EIN)")]
    [MaxLength(20)]
    public string? FederalTaxId { get; set; }

    [Display(Name = "State Tax ID")]
    [MaxLength(20)]
    public string? StateTaxId { get; set; }

    [Display(Name = "Sales Tax Exemption Number")]
    [MaxLength(50)]
    public string? SalesTaxExemptionNumber { get; set; }

    [Display(Name = "Business License Number")]
    [MaxLength(50)]
    public string? BusinessLicenseNumber { get; set; }

    [Display(Name = "Industry")]
    [MaxLength(100)]
    public string? Industry { get; set; }

    // Primary Contact
    [Display(Name = "First Name")]
    [MaxLength(100)]
    public string? ContactFirstName { get; set; }

    [Display(Name = "Last Name")]
    [MaxLength(100)]
    public string? ContactLastName { get; set; }

    [Display(Name = "Job Title")]
    [MaxLength(100)]
    public string? ContactJobTitle { get; set; }

    [Display(Name = "Email Address")]
    [MaxLength(150)]
    public string? ContactEmail { get; set; }

    [Display(Name = "Mobile Phone")]
    [MaxLength(20)]
    public string? ContactMobilePhone { get; set; }

    [Display(Name = "Office Phone")]
    [MaxLength(20)]
    public string? ContactOfficePhone { get; set; }

    [Display(Name = "Extension")]
    [MaxLength(10)]
    public string? ContactExtension { get; set; }

    // Business Address
    [Display(Name = "Country")]
    [MaxLength(50)]
    public string BusinessCountry { get; set; } = "USA";

    [Display(Name = "Street Address")]
    [MaxLength(200)]
    public string? BusinessStreetAddress { get; set; }

    [Display(Name = "Suite / Apt")]
    [MaxLength(100)]
    public string? BusinessSuiteApt { get; set; }

    [Display(Name = "City")]
    [MaxLength(100)]
    public string? BusinessCity { get; set; }

    [Display(Name = "State")]
    [MaxLength(50)]
    public string? BusinessState { get; set; }

    [Display(Name = "ZIP Code")]
    [MaxLength(20)]
    public string? BusinessZipCode { get; set; }

    [Display(Name = "County")]
    [MaxLength(100)]
    public string? BusinessCounty { get; set; }

    // Shipping Address
    [Display(Name = "Same as Business Address")]
    public bool SameAsBusinessAddress { get; set; } = true;

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

    // Financial Information
    [Display(Name = "Opening Balance")]
    public decimal OpeningBalance { get; set; }

    [Display(Name = "Credit Limit")]
    public decimal CreditLimit { get; set; }

    [Display(Name = "Payment Terms")]
    [MaxLength(50)]
    public string PaymentTerms { get; set; } = "Net 30";

    [Display(Name = "Preferred Payment Method")]
    [MaxLength(50)]
    public string PreferredPaymentMethod { get; set; } = "Check";

    [Display(Name = "Default Expense Account")]
    [MaxLength(100)]
    public string? DefaultExpenseAccount { get; set; }

    [Display(Name = "Sales Tax Code")]
    [MaxLength(50)]
    public string? SalesTaxCode { get; set; }

    [Display(Name = "1099 Vendor")]
    public bool Is1099Vendor { get; set; } = false;

    [Display(Name = "Currency")]
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    // Bank Details
    [Display(Name = "Bank Name")]
    [MaxLength(200)]
    public string? BankName { get; set; }

    [Display(Name = "Routing Number")]
    [MaxLength(20)]
    public string? RoutingNumber { get; set; }

    [Display(Name = "Account Number")]
    [MaxLength(30)]
    public string? AccountNumber { get; set; }

    [Display(Name = "Account Type")]
    [MaxLength(50)]
    public string? AccountType { get; set; }

    [Display(Name = "SWIFT Code")]
    [MaxLength(20)]
    public string? SwiftCode { get; set; }

    // Purchase Settings
    [Display(Name = "Default Warehouse")]
    [MaxLength(100)]
    public string? DefaultWarehouse { get; set; }

    [Display(Name = "Preferred Shipping Method")]
    [MaxLength(100)]
    public string? PreferredShippingMethod { get; set; }

    [Display(Name = "Lead Time (Days)")]
    public int LeadTimeDays { get; set; }

    [Display(Name = "Minimum Order Quantity")]
    public decimal MinimumOrderQuantity { get; set; }

    [Display(Name = "Default Discount %")]
    public decimal DefaultDiscountPercent { get; set; }

    [Display(Name = "Default Tax Rate")]
    public decimal DefaultTaxRate { get; set; }

    // Attachments
    [Display(Name = "W-9 Form")]
    [MaxLength(500)]
    public string? W9FormPath { get; set; }

    [Display(Name = "Vendor Contract")]
    [MaxLength(500)]
    public string? VendorContractPath { get; set; }

    [Display(Name = "Insurance Certificate")]
    [MaxLength(500)]
    public string? InsuranceCertificatePath { get; set; }

    [Display(Name = "Additional Documents")]
    [MaxLength(500)]
    public string? AdditionalDocumentsPath { get; set; }

    // Notes
    [Display(Name = "Internal Notes")]
    public string? InternalNotes { get; set; }

    // Dashboard Metrics (read-only)
    public DateTime VendorSince { get; set; } = DateTime.UtcNow;
    public int TotalPurchaseOrders { get; set; }
    public int TotalBills { get; set; }
    public decimal OutstandingAmount { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public int AveragePaymentDays { get; set; }
    public int VendorRating { get; set; } = 5;
    public bool IsPreferredVendor { get; set; } = false;

    // Dropdown options
    public List<string> VendorTypes { get; } = new() { "Supplier", "Manufacturer", "Distributor", "Contractor" };
    public List<string> PaymentTermsOptions { get; } = new() { "Due on Receipt", "Net 15", "Net 30", "Net 45", "Net 60" };
    public List<string> PaymentMethods { get; } = new() { "ACH", "Wire Transfer", "Check", "Credit Card" };
    public List<string> AccountTypes { get; } = new() { "Checking", "Savings" };
    public List<string> USStates { get; } = new()
    {
        "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA",
        "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
        "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
        "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
        "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
    };
}
