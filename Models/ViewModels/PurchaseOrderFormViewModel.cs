using System.ComponentModel.DataAnnotations;

namespace PaintERP.Models.ViewModels;

public class PurchaseOrderFormViewModel
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    // Header
    [Required]
    [Display(Name = "Vendor")]
    public int VendorId { get; set; }

    [Display(Name = "PO Number")]
    [MaxLength(50)]
    public string PONumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Order Date")]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "Expected Delivery Date")]
    public DateTime? ExpectedDeliveryDate { get; set; }

    [Display(Name = "Warehouse")]
    public int? WarehouseId { get; set; }

    [Display(Name = "Payment Terms")]
    [MaxLength(50)]
    public string PaymentTerms { get; set; } = "Net 30";

    [Display(Name = "Reference Number")]
    [MaxLength(50)]
    public string ReferenceNumber { get; set; } = string.Empty;

    // Fields from database schema
    [Required]
    [Display(Name = "Buyer")]
    [MaxLength(100)]
    public string Buyer { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Shipping Address")]
    [MaxLength(500)]
    public string ShippingAddress { get; set; } = string.Empty;

    // Status
    [Display(Name = "Status")]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    [Display(Name = "Approved")]
    public bool IsApproved { get; set; }

    [Display(Name = "Cancelled")]
    public bool IsCancelled { get; set; }

    [Display(Name = "Fully Received")]
    public bool IsFullyReceived { get; set; }

    // Financial Summary
    [Display(Name = "Subtotal")]
    public decimal Subtotal { get; set; }

    [Display(Name = "Discount Amount")]
    public decimal DiscountAmount { get; set; }

    [Display(Name = "Tax Amount")]
    public decimal TaxAmount { get; set; }

    [Display(Name = "Shipping Cost")]
    public decimal ShippingCost { get; set; }

    [Display(Name = "Amount Received")]
    public decimal AmountReceived { get; set; }

    [Display(Name = "Total Amount")]
    public decimal TotalAmount { get; set; }

    // Additional fields from database
    public DateTime? ApprovedDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public int? ConvertedBillId { get; set; }

    // Notes
    [Display(Name = "Internal Notes")]
    public string? InternalNotes { get; set; }

    [Display(Name = "Vendor Notes")]
    public string? VendorNotes { get; set; }

    // Line Items
    public List<PurchaseOrderItemViewModel> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItemViewModel>();

    // Dropdown Options
    public List<PurchaseOrderVendorListItem> Vendors { get; set; } = new List<PurchaseOrderVendorListItem>();
    public List<PurchaseOrderWarehouseListItem> Warehouses { get; set; } = new List<PurchaseOrderWarehouseListItem>();
    public List<PurchaseOrderPaintItemListItem> PaintItems { get; set; } = new List<PurchaseOrderPaintItemListItem>();
    public List<string> PaymentTermsOptions { get; } = new() { "Due on Receipt", "Net 15", "Net 30", "Net 45", "Net 60" };
    public List<string> ShippingMethods { get; } = new() { "Ground", "Air", "Freight", "Pickup" };
    public List<string> Currencies { get; } = new() { "USD", "EUR", "GBP", "CAD" };
    public List<string> Units { get; } = new() { "Gallon", "Liter", "Quart", "Pint", "Each", "Box", "Case" };
}

public class PurchaseOrderItemViewModel
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }

    [Display(Name = "SKU")]
    [MaxLength(50)]
    public string? SKU { get; set; }

    [Display(Name = "Description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Display(Name = "Paint Item")]
    public int? PaintItemId { get; set; }

    [Required]
    [Display(Name = "Qty Ordered")]
    public decimal QuantityOrdered { get; set; }

    [Required]
    [Display(Name = "Qty Received")]
    public decimal QuantityReceived { get; set; }

    [Display(Name = "Qty Pending")]
    public decimal QuantityPending { get; set; }

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

    [Display(Name = "Fully Received")]
    public bool IsFullyReceived { get; set; }
}

public class PurchaseOrderVendorListItem
{
    public int Id { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string VendorId { get; set; } = string.Empty;
}

public class PurchaseOrderWarehouseListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class PurchaseOrderPaintItemListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
    public string Unit { get; set; } = string.Empty;
}
