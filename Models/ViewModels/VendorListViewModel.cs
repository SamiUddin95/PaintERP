namespace PaintERP.Models.ViewModels;

public class VendorListViewModel
{
    public PaginatedList<VendorListItem> Vendors { get; set; } = null!;
    public string SearchTerm { get; set; } = string.Empty;
    public string FilterStatus { get; set; } = "All"; // All, Active, Inactive
    public string FilterType { get; set; } = "All"; // All, Supplier, Manufacturer, Distributor, Contractor
    public int TotalCount { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public decimal TotalOutstanding { get; set; }
}

public class VendorListItem
{
    public int Id { get; set; }
    public string VendorId { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string VendorType { get; set; } = string.Empty;
    public string VendorCategory { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string BusinessCity { get; set; } = string.Empty;
    public string BusinessState { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public decimal OutstandingAmount { get; set; }
    public DateTime VendorSince { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public int VendorRating { get; set; }
    public int Rating { get; set; }
    public bool IsPreferredVendor { get; set; }
}
