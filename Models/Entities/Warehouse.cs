namespace PaintERP.Models.Entities;

public class Warehouse
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public Company? Company { get; set; }
    public ICollection<PaintItem> PaintItems { get; set; } = new List<PaintItem>();
}
