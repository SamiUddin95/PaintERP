using System.ComponentModel.DataAnnotations;
using PaintERP.Models.Entities;

namespace PaintERP.Models.ViewModels;

public class SalesInvoiceFormViewModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Payment History
    public List<CustomerPaymentInvoice> CustomerPaymentInvoices { get; set; } = new();

    // Header
    [Required]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }

    [Required]
    [Display(Name = "Invoice Number")]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Invoice Date")]
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "Due Date")]
    public DateTime? DueDate { get; set; }

    [Required]
    [Display(Name = "Salesperson")]
    [MaxLength(100)]
    public string Salesperson { get; set; } = string.Empty;

    [Display(Name = "Warehouse")]
    public int? WarehouseId { get; set; }

    [Required]
    [Display(Name = "Shipping Method")]
    [MaxLength(100)]
    public string ShippingMethod { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Tracking Number")]
    [MaxLength(100)]
    public string TrackingNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Payment Terms")]
    [MaxLength(50)]
    public string PaymentTerms { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Reference Number")]
    [MaxLength(50)]
    public string ReferenceNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Status")]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    [Required]
    public bool IsPaid { get; set; } = false;

    [Required]
    public bool IsVoided { get; set; } = false;

    public DateTime? PaidDate { get; set; }

    public DateTime? VoidedDate { get; set; }

    // Line Items
    public List<SalesInvoiceItemViewModel> SalesInvoiceItems { get; set; } = new List<SalesInvoiceItemViewModel>();

    // Summary
    [Required]
    [Display(Name = "Subtotal")]
    public decimal Subtotal { get; set; } = 0;

    [Required]
    [Display(Name = "Discount")]
    public decimal DiscountAmount { get; set; } = 0;

    [Required]
    [Display(Name = "Shipping")]
    public decimal ShippingCost { get; set; } = 0;

    [Required]
    [Display(Name = "Sales Tax")]
    public decimal SalesTaxAmount { get; set; } = 0;

    [Required]
    [Display(Name = "Grand Total")]
    public decimal GrandTotal { get; set; } = 0;

    [Required]
    [Display(Name = "Amount Paid")]
    public decimal AmountPaid { get; set; } = 0;

    [Required]
    [Display(Name = "Balance Due")]
    public decimal BalanceDue { get; set; } = 0;

    // Notes & Attachments
    [Display(Name = "Attachment URL")]
    [MaxLength(500)]
    public string? AttachmentUrl { get; set; }

    [Display(Name = "Customer Notes")]
    public string? CustomerNotes { get; set; }

    [Display(Name = "Internal Notes")]
    public string? InternalNotes { get; set; }

    // Dropdown Options
    public List<SalesInvoiceCustomerListItem> Customers { get; set; } = new List<SalesInvoiceCustomerListItem>();
    public List<SalesInvoiceWarehouseListItem> Warehouses { get; set; } = new List<SalesInvoiceWarehouseListItem>();
    public List<SalesInvoicePaintItemListItem> PaintItems { get; set; } = new List<SalesInvoicePaintItemListItem>();
    public List<string> StatusOptions { get; set; } = new() { "Draft", "Sent", "Paid", "Partially Paid", "Overdue", "Void" };
    public List<string> PaymentTermsOptions { get; set; } = new() { "Due on Receipt", "Net 15", "Net 30", "Net 45", "Net 60" };
    public List<string> ShippingMethods { get; set; } = new() { "Pickup", "Ground", "Express", "Freight", "Courier" };
    public List<string> Units { get; set; } = new() { "Gallon", "Liter", "Quart", "Pint", "Each", "Box", "Case" };
}

public class SalesInvoiceItemViewModel
{
    public int Id { get; set; }
    public int SalesInvoiceId { get; set; }

    [Display(Name = "SKU")]
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;

    [Display(Name = "Description")]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Paint Item")]
    public int? PaintItemId { get; set; }

    [Required]
    [Display(Name = "Qty")]
    public decimal Quantity { get; set; }

    [Required]
    [Display(Name = "Unit")]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Price")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Display(Name = "Discount %")]
    public decimal DiscountPercent { get; set; } = 0;

    [Required]
    [Display(Name = "Discount Amount")]
    public decimal DiscountAmount { get; set; } = 0;

    [Required]
    [Display(Name = "Sales Tax %")]
    public decimal TaxPercent { get; set; } = 0;

    [Required]
    [Display(Name = "Tax Amount")]
    public decimal TaxAmount { get; set; } = 0;

    [Required]
    [Display(Name = "Line Total")]
    public decimal LineTotal { get; set; }

    [Display(Name = "Stock Before")]
    public decimal StockBefore { get; set; }

    [Display(Name = "Stock After")]
    public decimal StockAfter { get; set; }
}

public class SalesInvoiceCustomerListItem
{
    public int Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
}

public class SalesInvoiceWarehouseListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SalesInvoicePaintItemListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
}

public class SalesInvoiceListItem
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal BalanceDue { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
}
