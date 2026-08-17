using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;

namespace PaintERP.Services;

public class UnitConversionService
{
    private readonly PaintErpDbContext _context;

    public UnitConversionService(PaintErpDbContext context)
    {
        _context = context;
    }

    public async Task<decimal?> GetConversionFactorAsync(string fromUnit, string toUnit, CancellationToken cancellationToken = default)
    {
        if (string.Equals(fromUnit, toUnit, StringComparison.OrdinalIgnoreCase))
            return 1m;

        var conversion = await _context.UnitConversions
            .FirstOrDefaultAsync(uc => 
                uc.FromUnit.Equals(fromUnit, StringComparison.OrdinalIgnoreCase) &&
                uc.ToUnit.Equals(toUnit, StringComparison.OrdinalIgnoreCase) &&
                uc.IsActive, cancellationToken);

        return conversion?.ConversionFactor;
    }

    public async Task<decimal?> ConvertQuantityAsync(decimal quantity, string fromUnit, string toUnit, CancellationToken cancellationToken = default)
    {
        var factor = await GetConversionFactorAsync(fromUnit, toUnit, cancellationToken);
        if (factor == null)
            return null;

        return quantity * factor.Value;
    }

    public async Task<decimal?> ConvertUnitCostAsync(decimal unitCost, string fromUnit, string toUnit, CancellationToken cancellationToken = default)
    {
        if (string.Equals(fromUnit, toUnit, StringComparison.OrdinalIgnoreCase))
            return unitCost;

        // To convert unit cost, we need the inverse of the quantity conversion factor
        // Example: If 1 L = 1000 mL, then cost per mL = cost per L / 1000
        var factor = await GetConversionFactorAsync(fromUnit, toUnit, cancellationToken);
        if (factor == null)
            return null;

        return unitCost / factor.Value;
    }

    public async Task<decimal?> ConvertToBaseUnitAsync(decimal quantity, string fromUnit, CancellationToken cancellationToken = default)
    {
        // Common base units: L for volume, KG for weight
        var baseUnit = GetBaseUnit(fromUnit);
        if (baseUnit == null)
            return null;

        return await ConvertQuantityAsync(quantity, fromUnit, baseUnit, cancellationToken);
    }

    public async Task<decimal?> ConvertFromBaseUnitAsync(decimal quantity, string toUnit, CancellationToken cancellationToken = default)
    {
        var baseUnit = GetBaseUnit(toUnit);
        if (baseUnit == null)
            return null;

        return await ConvertQuantityAsync(quantity, baseUnit, toUnit, cancellationToken);
    }

    private string? GetBaseUnit(string unit)
    {
        var upperUnit = unit.ToUpperInvariant();
        return upperUnit switch
        {
            "ML" or "L" => "L",
            "PT" or "QT" or "GAL" => "GAL",
            "G" or "KG" => "KG",
            "LB" or "OZ" => "LB",
            _ => null
        };
    }

    public async Task SeedDefaultConversionsAsync(CancellationToken cancellationToken = default)
    {
        if (await _context.UnitConversions.AnyAsync(cancellationToken))
            return;

        var conversions = new List<UnitConversion>
        {
            // Volume conversions
            new UnitConversion { FromUnit = "L", ToUnit = "mL", ConversionFactor = 1000m, Category = "Volume", CreatedBy = "System" },
            new UnitConversion { FromUnit = "mL", ToUnit = "L", ConversionFactor = 0.001m, Category = "Volume", CreatedBy = "System" },
            new UnitConversion { FromUnit = "GAL", ToUnit = "QT", ConversionFactor = 4m, Category = "Volume", CreatedBy = "System" },
            new UnitConversion { FromUnit = "QT", ToUnit = "GAL", ConversionFactor = 0.25m, Category = "Volume", CreatedBy = "System" },
            new UnitConversion { FromUnit = "GAL", ToUnit = "PT", ConversionFactor = 8m, Category = "Volume", CreatedBy = "System" },
            new UnitConversion { FromUnit = "PT", ToUnit = "GAL", ConversionFactor = 0.125m, Category = "Volume", CreatedBy = "System" },
            new UnitConversion { FromUnit = "GAL", ToUnit = "L", ConversionFactor = 3.78541m, Category = "Volume", CreatedBy = "System" },
            new UnitConversion { FromUnit = "L", ToUnit = "GAL", ConversionFactor = 0.264172m, Category = "Volume", CreatedBy = "System" },
            new UnitConversion { FromUnit = "QT", ToUnit = "L", ConversionFactor = 0.946353m, Category = "Volume", CreatedBy = "System" },
            new UnitConversion { FromUnit = "L", ToUnit = "QT", ConversionFactor = 1.05669m, Category = "Volume", CreatedBy = "System" },
            
            // Weight conversions
            new UnitConversion { FromUnit = "KG", ToUnit = "G", ConversionFactor = 1000m, Category = "Weight", CreatedBy = "System" },
            new UnitConversion { FromUnit = "G", ToUnit = "KG", ConversionFactor = 0.001m, Category = "Weight", CreatedBy = "System" },
            new UnitConversion { FromUnit = "LB", ToUnit = "OZ", ConversionFactor = 16m, Category = "Weight", CreatedBy = "System" },
            new UnitConversion { FromUnit = "OZ", ToUnit = "LB", ConversionFactor = 0.0625m, Category = "Weight", CreatedBy = "System" },
            new UnitConversion { FromUnit = "KG", ToUnit = "LB", ConversionFactor = 2.20462m, Category = "Weight", CreatedBy = "System" },
            new UnitConversion { FromUnit = "LB", ToUnit = "KG", ConversionFactor = 0.453592m, Category = "Weight", CreatedBy = "System" },
        };

        await _context.UnitConversions.AddRangeAsync(conversions, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
