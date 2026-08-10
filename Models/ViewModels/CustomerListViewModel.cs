namespace PaintERP.Models.ViewModels;

public class CustomerListViewModel
{
    public PaginatedList<CustomerListItem> Customers { get; set; } = null!;
    public string SearchTerm { get; set; } = string.Empty;
    public string FilterStatus { get; set; } = "All"; // All, Active, Inactive
    public string FilterType { get; set; } = "All"; // All, Residential, Commercial, Contractor, Dealer, Wholesale, Retail
    public int TotalCount { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public decimal TotalOutstanding { get; set; }
    public decimal TotalLifetimeRevenue { get; set; }
}

public class CustomerListItem
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string CustomerType { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string BillingCity { get; set; } = string.Empty;
    public string BillingState { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public decimal OutstandingBalance { get; set; }
    public decimal LifetimeRevenue { get; set; }
    public DateTime CustomerSince { get; set; }
    public DateTime? LastInvoiceDate { get; set; }
    public int CustomerRating { get; set; }
    public int Rating { get; set; }
    public bool IsVIP { get; set; }
}
