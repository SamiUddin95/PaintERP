using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class CustomerPaymentFormViewModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [Required]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }

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
    [Display(Name = "Deposit Account")]
    [MaxLength(200)]
    public string DepositAccount { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Reference Number")]
    [MaxLength(100)]
    public string ReferenceNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Amount Received")]
    public decimal AmountReceived { get; set; } = 0;

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

    // Invoices
    public List<CustomerPaymentInvoiceViewModel> CustomerPaymentInvoices { get; set; } = new List<CustomerPaymentInvoiceViewModel>();

    // Notes
    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    // Dropdown Options
    public List<CustomerPaymentCustomerListItem> Customers { get; set; } = new List<CustomerPaymentCustomerListItem>();
    public List<string> PaymentMethods { get; } = new() { "Check", "ACH", "Wire", "Cash", "Credit Card" };
    public List<string> DepositAccounts { get; } = new() { "Operating Account", "Checking Account", "Savings Account", "Petty Cash" };
    public List<string> StatusOptions { get; } = new() { "Draft", "Recorded", "Deposited", "Voided" };
}

public class CustomerPaymentInvoiceViewModel
{
    public int Id { get; set; }
    public int CustomerPaymentId { get; set; }

    [Required]
    public int SalesInvoiceId { get; set; }

    [Display(Name = "Invoice Number")]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Invoice Amount")]
    public decimal InvoiceAmount { get; set; }

    [Required]
    [Display(Name = "Amount Due")]
    public decimal AmountDue { get; set; }

    [Required]
    [Display(Name = "Payment Applied")]
    public decimal PaymentApplied { get; set; }

    [Required]
    [Display(Name = "Remaining Balance")]
    public decimal RemainingBalance { get; set; }

    [Display(Name = "Apply")]
    public bool IsSelected { get; set; } = false;
}

public class CustomerPaymentCustomerListItem
{
    public int Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
}

public class CustomerPaymentInvoiceListItem
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal InvoiceAmount { get; set; }
    public decimal AmountDue { get; set; }
}
