namespace PaintERP.Models.Entities;

public class UnitConversion
{
    public int Id { get; set; }
    public string FromUnit { get; set; } = string.Empty; // e.g., "L"
    public string ToUnit { get; set; } = string.Empty; // e.g., "mL"
    public decimal ConversionFactor { get; set; } // e.g., 1000 (1 L = 1000 mL)
    public string Category { get; set; } = string.Empty; // e.g., "Volume", "Weight"
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
}
