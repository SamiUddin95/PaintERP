namespace PaintERP.Models.Entities;

public class SalesOrder
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Open";

    public Company? Company { get; set; }
    public Customer? Customer { get; set; }
}
