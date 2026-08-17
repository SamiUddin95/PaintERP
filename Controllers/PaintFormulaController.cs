using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;
using PaintERP.Services;

namespace PaintERP.Controllers;

public class PaintFormulaController(PaintErpDbContext context, UnitConversionService unitConversionService) : Controller
{
    private readonly PaintErpDbContext _context = context;
    private readonly UnitConversionService _unitConversionService = unitConversionService;

    // GET: PaintFormula
    public async Task<IActionResult> Index()
    {
        var formulas = await _context.PaintFormulas
            .Include(pf => pf.Company)
            .OrderBy(pf => pf.FormulaCode)
            .ToListAsync();

        return View(formulas);
    }

    // GET: PaintFormula/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var formula = await _context.PaintFormulas
            .Include(pf => pf.Company)
            .Include(pf => pf.ParentFormula)
            .Include(pf => pf.Items)
                .ThenInclude(fi => fi.PaintItem)
            .FirstOrDefaultAsync(pf => pf.Id == id);

        if (formula == null)
        {
            return NotFound();
        }

        return View(formula);
    }

    // GET: PaintFormula/Create
    public IActionResult Create()
    {
        var viewModel = new PaintFormulaFormViewModel
        {
            FormulaCode = GenerateFormulaCode(),
            Status = "Draft",
            Version = 1,
            WastePercentage = 0,
            SellingPrice = 0,
            TotalFormulaCost = 0
        };

        ViewBag.PaintItems = _context.PaintItems.ToList();
        return View(viewModel);
    }

    // POST: PaintFormula/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaintFormulaFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.PaintItems = _context.PaintItems.ToList();
            return View(viewModel);
        }

        var formula = new PaintFormula
        {
            FormulaName = viewModel.FormulaName,
            FormulaCode = viewModel.FormulaCode,
            PaintColor = viewModel.PaintColor,
            Finish = viewModel.Finish,
            ContainerSize = viewModel.ContainerSize,
            TotalFormulaCost = viewModel.TotalFormulaCost,
            ExpectedYield = viewModel.ExpectedYield,
            WastePercentage = viewModel.WastePercentage,
            SellingPrice = viewModel.SellingPrice,
            GrossMargin = viewModel.GrossMargin,
            Version = viewModel.Version,
            Status = viewModel.Status,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name ?? "System",
            UpdatedBy = User.Identity?.Name ?? "System"
        };

        _context.PaintFormulas.Add(formula);
        await _context.SaveChangesAsync();

        // Add formula items
        if (viewModel.FormulaItems != null && viewModel.FormulaItems.Any())
        {
            foreach (var item in viewModel.FormulaItems)
            {
                // Get the source item to check unit conversion
                PaintItem? sourceItem = null;
                if (item.PaintItemId.HasValue)
                {
                    sourceItem = await _context.PaintItems.FindAsync(item.PaintItemId.Value);
                }

                decimal unitCostToUse = item.UnitCost;
                string unitToUse = item.Unit;

                // Convert unit cost if unit differs from item's unit
                if (sourceItem != null && !string.IsNullOrWhiteSpace(item.Unit) &&
                    !string.Equals(item.Unit, sourceItem.UnitOfMeasure, StringComparison.OrdinalIgnoreCase))
                {
                    if (item.UnitCost <= 0)
                    {
                        var convertedUnitCost = await _unitConversionService.ConvertUnitCostAsync(
                            sourceItem.UnitCost, sourceItem.UnitOfMeasure, item.Unit);
                        if (convertedUnitCost.HasValue)
                        {
                            unitCostToUse = convertedUnitCost.Value;
                        }
                        else
                        {
                            unitCostToUse = sourceItem.UnitCost;
                        }
                    }
                }
                else if (sourceItem != null && string.IsNullOrWhiteSpace(item.Unit))
                {
                    // Default to item's unit if not specified
                    unitToUse = sourceItem.UnitOfMeasure;
                    unitCostToUse = sourceItem.UnitCost;
                }

                var formulaItem = new PaintFormulaItem
                {
                    PaintFormulaId = formula.Id,
                    PaintItemId = item.PaintItemId,
                    RawMaterialName = item.RawMaterialName,
                    Percentage = item.Percentage,
                    RequiredQuantity = item.RequiredQuantity,
                    Unit = unitToUse,
                    UnitCost = unitCostToUse,
                    InventoryAvailable = item.InventoryAvailable
                };
                _context.PaintFormulaItems.Add(formulaItem);
            }
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private string GenerateFormulaCode()
    {
        var year = DateTime.Now.ToString("yy");
        var month = DateTime.Now.ToString("MM");
        var random = new Random().Next(1000, 9999);
        return $"FM-{year}{month}-{random}";
    }

    // GET: PaintFormula/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var formula = await _context.PaintFormulas.FindAsync(id);
        if (formula == null)
        {
            return NotFound();
        }

        return View(formula);
    }

    // POST: PaintFormula/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PaintFormula formula)
    {
        if (id != formula.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(formula);
        }

        var existingFormula = await _context.PaintFormulas.FindAsync(id);
        if (existingFormula == null)
        {
            return NotFound();
        }

        existingFormula.FormulaName = formula.FormulaName;
        existingFormula.FormulaCode = formula.FormulaCode;
        existingFormula.PaintColor = formula.PaintColor;
        existingFormula.Finish = formula.Finish;
        existingFormula.ContainerSize = formula.ContainerSize;
        existingFormula.TotalFormulaCost = formula.TotalFormulaCost;
        existingFormula.ExpectedYield = formula.ExpectedYield;
        existingFormula.WastePercentage = formula.WastePercentage;
        existingFormula.SellingPrice = formula.SellingPrice;
        existingFormula.GrossMargin = formula.GrossMargin;
        existingFormula.Version = formula.Version;
        existingFormula.ParentFormulaId = formula.ParentFormulaId;
        existingFormula.Status = formula.Status;
        existingFormula.UpdatedAtUtc = DateTime.UtcNow;
        existingFormula.UpdatedBy = User.Identity?.Name ?? "System";

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: PaintFormula/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var formula = await _context.PaintFormulas
            .Include(pf => pf.Company)
            .FirstOrDefaultAsync(pf => pf.Id == id);

        if (formula == null)
        {
            return NotFound();
        }

        return View(formula);
    }

    // POST: PaintFormula/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var formula = await _context.PaintFormulas.FindAsync(id);
        if (formula != null)
        {
            _context.PaintFormulas.Remove(formula);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
