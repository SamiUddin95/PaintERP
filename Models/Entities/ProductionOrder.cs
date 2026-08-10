namespace PaintERP.Models.Entities;

public class ProductionOrder
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public DateTime ProductionDate { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Status { get; set; } = "Scheduled";

    public Company? Company { get; set; }
}
