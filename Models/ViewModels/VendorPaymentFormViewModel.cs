using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class VendorPaymentFormViewModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [Required]
    [Display(Name = "Vendor")]
    public int VendorId { get; set; }

    [Required]
    [Display(Name = "Payment Number")]
    [MaxLength(50)]
    public string PaymentNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Payment Date")]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Display(Name = "Payment Method")]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Bank Account")]
    [MaxLength(200)]
    public string BankAccount { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Reference Number")]
    [MaxLength(100)]
    public string ReferenceNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Total Payment Amount")]
    public decimal TotalPaymentAmount { get; set; } = 0;

    [Required]
    [Display(Name = "Total Applied")]
    public decimal TotalApplied { get; set; } = 0;

    [Required]
    [Display(Name = "Unapplied Amount")]
    public decimal UnappliedAmount { get; set; } = 0;

    [Required]
    [Display(Name = "Status")]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    // Bills
    public List<VendorPaymentBillViewModel> VendorPaymentBills { get; set; } = new List<VendorPaymentBillViewModel>();

    // Notes
    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    // Dropdown Options
    public List<VendorPaymentVendorListItem> Vendors { get; set; } = new List<VendorPaymentVendorListItem>();
    public List<string> PaymentMethods { get; } = new() { "Check", "ACH", "Wire", "Credit Card" };
    public List<string> BankAccounts { get; } = new() { "Operating Account", "Checking Account", "Savings Account" };
    public List<string> StatusOptions { get; } = new() { "Draft", "Recorded", "Sent", "Voided", "Reconciled" };
}

public class VendorPaymentBillViewModel
{
    public int Id { get; set; }
    public int VendorPaymentId { get; set; }

    [Required]
    public int BillId { get; set; }

    [Display(Name = "Bill Number")]
    public string BillNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Bill Amount")]
    public decimal BillAmount { get; set; }

    [Required]
    [Display(Name = "Amount Due")]
    public decimal AmountDue { get; set; }

    [Required]
    [Display(Name = "Payment Amount")]
    public decimal PaymentAmount { get; set; }

    [Required]
    [Display(Name = "Remaining Balance")]
    public decimal RemainingBalance { get; set; }

    [Display(Name = "Apply")]
    public bool IsSelected { get; set; } = false;
}

public class VendorPaymentVendorListItem
{
    public int Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
}

public class VendorPaymentBillListItem
{
    public int Id { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public DateTime BillDate { get; set; }
    public decimal BillAmount { get; set; }
    public decimal AmountDue { get; set; }
}
