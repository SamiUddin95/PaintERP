using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Helpers;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;
using PaintERP.Services;

namespace PaintERP.Controllers;

// [Authorize] - TODO: Enable when authentication is implemented
public class PaintProductionController : Controller
{
    private readonly PaintErpDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IInventoryService _inventoryService;
    private readonly IAccountingService _accountingService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<PaintProductionController> _logger;

    public PaintProductionController(
        PaintErpDbContext context,
        IValidationService validationService,
        IInventoryService inventoryService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService,
        ILogger<PaintProductionController> logger)
    {
        _context = context;
        _validationService = validationService;
        _inventoryService = inventoryService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _logger = logger;
    }

    // GET: PaintProduction
    public async Task<IActionResult> Index()
    {
        var productions = await _context.PaintProductions
            .Include(p => p.Warehouse)
            .Include(p => p.FinishedProduct)
            .OrderByDescending(p => p.ProductionDate)
            .Select(p => new PaintProductionListItem
            {
                Id = p.Id,
                ProductionNumber = p.ProductionNumber,
                BatchNumber = p.BatchNumber,
                Recipe = p.Recipe,
                ProductionDate = p.ProductionDate,
                OutputQuantity = p.OutputQuantity,
                Status = p.Status,
                WarehouseName = p.Warehouse.Name,
                FinishedProduct = p.FinishedProduct.Name
            })
            .ToListAsync();

        return View(productions);
    }

    // GET: PaintProduction/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var production = await _context.PaintProductions
            .Include(p => p.Warehouse)
            .Include(p => p.FinishedProduct)
            .Include(p => p.Materials)
                .ThenInclude(m => m.PaintItem)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (production == null)
        {
            return NotFound();
        }

        return View(production);
    }

    // GET: PaintProduction/Create
    public async Task<IActionResult> Create()
    {
        // Ensure default company exists
        if (!await _context.Companies.AnyAsync())
        {
            _context.Companies.Add(new Company
            {
                Name = "USA Paint ERP",
                Industry = "Industrial Coatings",
                Country = "USA",
                PrimaryColor = "#0A5C9E",
                LogoUrl = "/images/logos/painterp-logo.svg"
            });
            await _context.SaveChangesAsync();
        }

        var defaultCompanyId = await _context.Companies.Select(c => c.Id).FirstOrDefaultAsync();

        var model = new PaintProductionFormViewModel
        {
            CompanyId = defaultCompanyId, // Use default company automatically - no user selection needed
            ProductionDate = DateTime.UtcNow,
            ProductionNumber = await GenerateProductionNumber(),
            Materials = new List<PaintProductionMaterialViewModel> { new PaintProductionMaterialViewModel() },
            Warehouses = await _context.Warehouses
                .OrderBy(w => w.Name)
                .Select(w => new PaintProductionWarehouseListItem { Id = w.Id, Name = w.Name })
                .ToListAsync(),
            PaintItems = await _context.PaintItems
                .OrderBy(p => p.Name)
                .Select(p => new PaintProductionPaintItemListItem { Id = p.Id, Name = p.Name, SKU = p.SKU ?? "", UnitCost = p.UnitCost })
                .ToListAsync(),
            Formulas = await _context.PaintFormulas
                .OrderBy(f => f.FormulaName)
                .Select(f => new PaintProductionFormulaListItem { Id = f.Id, FormulaName = f.FormulaName })
                .ToListAsync()
        };

        return View(model);
    }

    // POST: PaintProduction/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaintProductionFormViewModel model, CancellationToken cancellationToken)
    {
        try
        {
            // Step 1: Validate ModelState
            if (!ModelState.IsValid)
            {
                this.AddErrorMessage("Please correct the validation errors");
                await PopulateDropdowns(model);
                return View(model);
            }

            // Ensure default company exists
            if (!await _context.Companies.AnyAsync(cancellationToken))
            {
                _context.Companies.Add(new Company
                {
                    Name = "USA Paint ERP",
                    Industry = "Industrial Coatings",
                    Country = "USA",
                    PrimaryColor = "#0A5C9E",
                    LogoUrl = "/images/logos/painterp-logo.svg"
                });
                await _context.SaveChangesAsync(cancellationToken);
            }

            var defaultCompanyId = await _context.Companies.Select(c => c.Id).FirstOrDefaultAsync(cancellationToken);

            var production = new PaintProduction
            {
                CompanyId = defaultCompanyId, // Use default company automatically - no user selection needed
                ProductionNumber = model.ProductionNumber,
                Recipe = model.Recipe,
                BatchNumber = model.BatchNumber,
                ProductionDate = model.ProductionDate,
                WarehouseId = model.WarehouseId,
                OutputQuantity = model.OutputQuantity,
                Status = model.Status,
                FinishedProductId = model.FinishedProductId,
                FormulaId = model.FormulaId,
                FinishedProductDescription = model.FinishedProductDescription,
                MaterialCost = model.MaterialCost,
                LaborCost = model.LaborCost,
                OverheadCost = model.OverheadCost,
                ProductionCost = model.ProductionCost,
                FinishedProductCost = model.FinishedProductCost,
                CostPerUnit = model.CostPerUnit,
                BatchLabel = model.BatchLabel,
                QCStatus = model.QCStatus,
                QCReport = model.QCReport,
                QCNotes = model.QCNotes,
                ProductionNotes = model.ProductionNotes,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name ?? "System",
                UpdatedBy = User.Identity?.Name ?? "System"
            };

            // Add production materials
            foreach (var material in model.Materials)
            {
                production.Materials.Add(new PaintProductionMaterial
                {
                    PaintItemId = material.PaintItemId,
                    MaterialName = material.MaterialName,
                    MaterialType = material.MaterialType,
                    RequiredQuantity = material.RequiredQuantity,
                    ConsumedQuantity = material.ConsumedQuantity,
                    UnitCost = material.UnitCost,
                    TotalCost = material.TotalCost,
                    PercentageInMix = material.PercentageInMix,
                    StockBefore = material.StockBefore,
                    StockAfter = material.StockAfter
                });
            }

            // Step 2: Business Validation
            var validationResult = await _validationService.ValidateProductionAsync(production, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                _notificationService.AddValidationErrors(validationResult);
                await PopulateDropdowns(model);
                return this.WithValidationErrors(validationResult);
            }

            // Display warnings if any
            foreach (var warning in validationResult.Warnings)
            {
                this.AddWarningMessage(warning);
            }

            // Step 3: Execute in Transaction (ensures atomicity)
            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                try
                {
                    // Save production
                    _context.PaintProductions.Add(production);
                    await _context.SaveChangesAsync(cancellationToken);

                    // Process inventory - consume raw materials
                    if (production.Materials != null && production.Materials.Any())
                    {
                        foreach (var material in production.Materials)
                        {
                            if (material.PaintItemId.HasValue && material.ConsumedQuantity > 0)
                            {
                                await _inventoryService.ReduceStockAsync(
                                    material.PaintItemId.Value,
                                    production.WarehouseId,
                                    material.ConsumedQuantity,
                                    "Paint Production",
                                    production.Id,
                                    $"Production: {production.ProductionNumber}",
                                    cancellationToken
                                );
                            }
                        }
                    }

                    // Add finished product to inventory
                    if (production.FinishedProductId.HasValue && production.OutputQuantity > 0)
                    {
                        await _inventoryService.IncreaseStockAsync(
                            production.FinishedProductId.Value,
                            production.WarehouseId,
                            production.OutputQuantity,
                            "Paint Production Output",
                            production.Id,
                            $"Production: {production.ProductionNumber}",
                            cancellationToken
                        );
                    }

                    _logger.LogInformation(
                        "Paint Production {ProductionNumber} created successfully",
                        production.ProductionNumber
                    );

                    return TransactionResult.SuccessResult(
                        $"Paint Production {production.ProductionNumber} created successfully",
                        new { ProductionId = production.Id }
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating paint production: {Message}", ex.Message);
                    return TransactionResult.FailureResult(
                        "An error occurred while creating the paint production",
                        ex.Message
                    );
                }
            }, cancellationToken);

            // Step 4: Handle Transaction Result
            if (transactionResult.Success)
            {
                return this.WithSuccess(transactionResult.Message, "Index");
            }
            else
            {
                foreach (var error in transactionResult.Errors)
                {
                    this.AddErrorMessage(error);
                }
                await PopulateDropdowns(model);
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in Create action: {Message}", ex.Message);
            await PopulateDropdowns(model);
            return this.WithError("An unexpected error occurred. Please try again.", "Index");
        }
    }

    // GET: PaintProduction/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var production = await _context.PaintProductions
            .Include(p => p.Materials)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (production == null)
        {
            return NotFound();
        }

        var model = new PaintProductionFormViewModel
        {
            Id = production.Id,
            CompanyId = production.CompanyId,
            ProductionNumber = production.ProductionNumber,
            Recipe = production.Recipe,
            BatchNumber = production.BatchNumber,
            ProductionDate = production.ProductionDate,
            WarehouseId = production.WarehouseId,
            OutputQuantity = production.OutputQuantity,
            Status = production.Status,
            FinishedProductId = production.FinishedProductId,
            FormulaId = production.FormulaId,
            FinishedProductDescription = production.FinishedProductDescription,
            MaterialCost = production.MaterialCost,
            LaborCost = production.LaborCost,
            OverheadCost = production.OverheadCost,
            ProductionCost = production.ProductionCost,
            FinishedProductCost = production.FinishedProductCost,
            CostPerUnit = production.CostPerUnit,
            BatchLabel = production.BatchLabel,
            QCStatus = production.QCStatus,
            QCReport = production.QCReport,
            QCNotes = production.QCNotes,
            ProductionNotes = production.ProductionNotes,
            Materials = production.Materials.Select(m => new PaintProductionMaterialViewModel
            {
                Id = m.Id,
                PaintProductionId = m.PaintProductionId,
                PaintItemId = m.PaintItemId,
                MaterialName = m.MaterialName,
                MaterialType = m.MaterialType,
                RequiredQuantity = m.RequiredQuantity,
                ConsumedQuantity = m.ConsumedQuantity,
                UnitCost = m.UnitCost,
                TotalCost = m.TotalCost,
                PercentageInMix = m.PercentageInMix,
                StockBefore = m.StockBefore,
                StockAfter = m.StockAfter
            }).ToList()
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    private async Task PopulateDropdowns(PaintProductionFormViewModel model)
    {
        model.Warehouses = await _context.Warehouses
            .OrderBy(w => w.Name)
            .Select(w => new PaintProductionWarehouseListItem { Id = w.Id, Name = w.Name })
            .ToListAsync();

        model.PaintItems = await _context.PaintItems
            .OrderBy(p => p.Name)
            .Select(p => new PaintProductionPaintItemListItem { Id = p.Id, Name = p.Name, SKU = p.SKU ?? "", UnitCost = p.UnitCost })
            .ToListAsync();

        model.Formulas = await _context.PaintFormulas
            .OrderBy(f => f.FormulaName)
            .Select(f => new PaintProductionFormulaListItem { Id = f.Id, FormulaName = f.FormulaName })
            .ToListAsync();
    }

    // POST: PaintProduction/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PaintProductionFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model);
            return View(model);
        }

        var production = await _context.PaintProductions
            .Include(p => p.Materials)
            .FirstOrDefaultAsync(p => p.Id == model.Id);

        if (production == null)
        {
            return NotFound();
        }

        production.Recipe = model.Recipe;
        production.BatchNumber = model.BatchNumber;
        production.ProductionDate = model.ProductionDate;
        production.WarehouseId = model.WarehouseId;
        production.OutputQuantity = model.OutputQuantity;
        production.Status = model.Status;
        production.FinishedProductId = model.FinishedProductId;
        production.FormulaId = model.FormulaId;
        production.FinishedProductDescription = model.FinishedProductDescription;
        production.MaterialCost = model.MaterialCost;
        production.LaborCost = model.LaborCost;
        production.OverheadCost = model.OverheadCost;
        production.ProductionCost = model.ProductionCost;
        production.FinishedProductCost = model.FinishedProductCost;
        production.CostPerUnit = model.CostPerUnit;
        production.BatchLabel = model.BatchLabel;
        production.QCStatus = model.QCStatus;
        production.QCReport = model.QCReport;
        production.QCNotes = model.QCNotes;
        production.ProductionNotes = model.ProductionNotes;
        production.UpdatedAtUtc = DateTime.UtcNow;
        production.UpdatedBy = User.Identity?.Name ?? "System";

        var existingMaterials = production.Materials.ToList();
        _context.PaintProductionMaterials.RemoveRange(existingMaterials);

        foreach (var material in model.Materials)
        {
            var productionMaterial = new PaintProductionMaterial
            {
                PaintProductionId = production.Id,
                PaintItemId = material.PaintItemId,
                MaterialName = material.MaterialName,
                MaterialType = material.MaterialType,
                RequiredQuantity = material.RequiredQuantity,
                ConsumedQuantity = material.ConsumedQuantity,
                UnitCost = material.UnitCost,
                TotalCost = material.TotalCost,
                PercentageInMix = material.PercentageInMix,
                StockBefore = material.StockBefore,
                StockAfter = material.StockAfter
            };
            _context.PaintProductionMaterials.Add(productionMaterial);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: PaintProduction/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var production = await _context.PaintProductions
            .Include(p => p.Warehouse)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (production == null)
        {
            return NotFound();
        }

        return View(production);
    }

    // POST: PaintProduction/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        try
        {
            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                var production = await _context.PaintProductions
                    .Include(p => p.Materials)
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

                if (production == null)
                {
                    return TransactionResult.FailureResult("Paint production not found");
                }

                // Reverse inventory movements - restore raw materials
                if (production.Materials != null && production.Materials.Any())
                {
                    foreach (var material in production.Materials)
                    {
                        if (material.PaintItemId.HasValue && material.ConsumedQuantity > 0)
                        {
                            await _inventoryService.IncreaseStockAsync(
                                material.PaintItemId.Value,
                                production.WarehouseId,
                                material.ConsumedQuantity,
                                "Paint Production Reversal",
                                production.Id,
                                $"Reversal of Production: {production.ProductionNumber}",
                                cancellationToken
                            );
                        }
                    }
                }

                // Reverse finished product inventory
                if (production.FinishedProductId.HasValue && production.OutputQuantity > 0)
                {
                    await _inventoryService.ReduceStockAsync(
                        production.FinishedProductId.Value,
                        production.WarehouseId,
                        production.OutputQuantity,
                        "Paint Production Output Reversal",
                        production.Id,
                        $"Reversal of Production: {production.ProductionNumber}",
                        cancellationToken
                    );
                }

                // Delete production
                _context.PaintProductions.Remove(production);
                await _context.SaveChangesAsync(cancellationToken);

                return TransactionResult.SuccessResult($"Paint Production {production.ProductionNumber} deleted successfully");
            }, cancellationToken);

            if (transactionResult.Success)
            {
                return this.WithSuccess(transactionResult.Message, "Index");
            }
            else
            {
                return this.WithError(transactionResult.Message, "Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting paint production: {Message}", ex.Message);
            return this.WithError("An error occurred while deleting the paint production", "Index");
        }
    }

    // POST: PaintProduction/Start/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Start(int id)
    {
        var production = await _context.PaintProductions.FindAsync(id);
        if (production != null)
        {
            production.Status = "In Progress";
            production.StartedAtUtc = DateTime.UtcNow;
            production.UpdatedAtUtc = DateTime.UtcNow;
            production.UpdatedBy = User.Identity?.Name ?? "System";
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    // POST: PaintProduction/Complete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
        var production = await _context.PaintProductions.FindAsync(id);
        if (production != null)
        {
            production.Status = "Completed";
            production.CompletedAtUtc = DateTime.UtcNow;
            production.UpdatedAtUtc = DateTime.UtcNow;
            production.UpdatedBy = User.Identity?.Name ?? "System";
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    private async Task<string> GenerateProductionNumber()
    {
        var lastProduction = await _context.PaintProductions
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        if (lastProduction == null)
        {
            return "PR-0001";
        }

        var lastNumber = lastProduction.ProductionNumber.Replace("PR-", "");
        if (int.TryParse(lastNumber, out int number))
        {
            return $"PR-{(number + 1):D4}";
        }

        return "PR-0001";
    }
}

public class PaintProductionListItem
{
    public int Id { get; set; }
    public string ProductionNumber { get; set; } = string.Empty;
    public string BatchNumber { get; set; } = string.Empty;
    public string Recipe { get; set; } = string.Empty;
    public DateTime ProductionDate { get; set; }
    public decimal OutputQuantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public string FinishedProduct { get; set; } = string.Empty;
}
