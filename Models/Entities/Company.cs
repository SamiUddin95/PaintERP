namespace PaintERP.Models.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;

    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    public ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
    public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
