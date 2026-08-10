using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class BillFormViewModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [Required]
    [Display(Name = "Vendor")]
    public int VendorId { get; set; }

    [Display(Name = "Bill Number")]
    [MaxLength(50)]
    public string BillNumber { get; set; } = string.Empty;

    [Display(Name = "Vendor Invoice Number")]
    [MaxLength(50)]
    public string VendorInvoiceNumber { get; set; } = string.Empty;

    [Display(Name = "Purchase Order")]
    public int? PurchaseOrderId { get; set; }

    [Required]
    [Display(Name = "Bill Date")]
    public DateTime BillDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "Due Date")]
    public DateTime? DueDate { get; set; }

    [Display(Name = "Warehouse")]
    public int? WarehouseId { get; set; }

    [Display(Name = "Payment Terms")]
    [MaxLength(50)]
    public string PaymentTerms { get; set; } = "Net 30";

    [Display(Name = "Shipping Method")]
    [MaxLength(100)]
    public string ShippingMethod { get; set; } = string.Empty;

    [Display(Name = "Reference Number")]
    [MaxLength(50)]
    public string ReferenceNumber { get; set; } = string.Empty;

    [Display(Name = "Currency")]
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    [Display(Name = "Tax Code")]
    [MaxLength(50)]
    public string TaxCode { get; set; } = string.Empty;

    // Status
    [Display(Name = "Status")]
    [MaxLength(50)]
    public string Status { get; set; } = "Open";
    public bool IsApproved { get; set; } = false;
    public bool IsVoid { get; set; } = false;
    public bool IsPaid { get; set; } = false;

    // Financial Summary
    [Display(Name = "Subtotal")]
    public decimal Subtotal { get; set; }

    [Display(Name = "Discount Amount")]
    public decimal DiscountAmount { get; set; }

    [Display(Name = "Shipping Charges")]
    public decimal ShippingCharges { get; set; }

    [Display(Name = "Tax Amount")]
    public decimal TaxAmount { get; set; }

    [Display(Name = "Other Charges")]
    public decimal OtherCharges { get; set; }

    [Display(Name = "Grand Total")]
    public decimal GrandTotal { get; set; }

    [Display(Name = "Balance Due")]
    public decimal BalanceDue { get; set; }

    // Attachments
    [Display(Name = "Attachment")]
    [MaxLength(500)]
    public string? AttachmentPath { get; set; }

    // Notes
    [Display(Name = "Internal Notes")]
    public string? InternalNotes { get; set; }

    [Display(Name = "Vendor Notes")]
    public string? VendorNotes { get; set; }

    // Line Items
    public List<BillItemViewModel> BillItems { get; set; } = new List<BillItemViewModel>();

    // Right Sidebar Data
    [Display(Name = "Vendor Balance")]
    public decimal VendorBalance { get; set; }

    [Display(Name = "Last Purchase Date")]
    public DateTime? LastPurchaseDate { get; set; }

    [Display(Name = "Last Purchase Amount")]
    public decimal LastPurchaseAmount { get; set; }

    [Display(Name = "Vendor Credit")]
    public decimal VendorCredit { get; set; }

    public List<PaymentHistoryViewModel> PaymentHistory { get; set; } = new List<PaymentHistoryViewModel>();

    // Dropdown Options
    public List<BillVendorListItem> Vendors { get; set; } = new List<BillVendorListItem>();
    public List<BillWarehouseListItem> Warehouses { get; set; } = new List<BillWarehouseListItem>();
    public List<BillPurchaseOrderListItem> PurchaseOrders { get; set; } = new List<BillPurchaseOrderListItem>();
    public List<BillPaintItemListItem> PaintItems { get; set; } = new List<BillPaintItemListItem>();
    public List<string> PaymentTermsOptions { get; } = new() { "Due on Receipt", "Net 15", "Net 30", "Net 45", "Net 60" };
    public List<string> ShippingMethods { get; } = new() { "Ground", "Air", "Freight", "Pickup" };
    public List<string> Currencies { get; } = new() { "USD", "EUR", "GBP", "CAD" };
    public List<string> Units { get; } = new() { "Gallon", "Liter", "Quart", "Pint", "Each", "Box", "Case" };
}

public class BillItemViewModel
{
    public int Id { get; set; }
    public int BillId { get; set; }

    [Display(Name = "SKU")]
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;

    [Display(Name = "Description")]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Warehouse")]
    public int? WarehouseId { get; set; }

    [Display(Name = "Paint Item")]
    public int? PaintItemId { get; set; }

    [Required]
    [Display(Name = "Quantity")]
    public decimal Quantity { get; set; }

    [Display(Name = "Unit")]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Unit Cost")]
    public decimal UnitCost { get; set; }

    [Display(Name = "Discount %")]
    public decimal DiscountPercent { get; set; }

    [Display(Name = "Discount Amount")]
    public decimal DiscountAmount { get; set; }

    [Display(Name = "Tax %")]
    public decimal TaxPercent { get; set; }

    [Display(Name = "Tax Amount")]
    public decimal TaxAmount { get; set; }

    [Display(Name = "Line Total")]
    public decimal LineTotal { get; set; }
}

public class PaymentHistoryViewModel
{
    public int Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentNumber { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal PaymentAmount { get; set; }
    public decimal BalanceAfter { get; set; }
}

public class BillVendorListItem
{
    public int Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string VendorId { get; set; } = string.Empty;
}

public class BillWarehouseListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class BillPurchaseOrderListItem
{
    public int Id { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}

public class BillPaintItemListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
    public string Unit { get; set; } = string.Empty;
}
