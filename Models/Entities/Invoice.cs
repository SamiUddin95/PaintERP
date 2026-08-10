namespace PaintERP.Models.Entities;

public class Invoice
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int CustomerId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }

    public Company? Company { get; set; }
    public Customer? Customer { get; set; }
}
