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
    private readonly UnitConversionService _unitConversionService;
    private readonly ILogger<PaintProductionController> _logger;

    public PaintProductionController(
        PaintErpDbContext context,
        IValidationService validationService,
        IInventoryService inventoryService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService,
        UnitConversionService unitConversionService,
        ILogger<PaintProductionController> logger)
    {
        _context = context;
        _validationService = validationService;
        _inventoryService = inventoryService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _unitConversionService = unitConversionService;
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
            BatchNumber = $"BATCH-{DateTime.UtcNow:yyyyMMdd-HHmmss}",
            Materials = new List<PaintProductionMaterialViewModel> { new PaintProductionMaterialViewModel() },
            Warehouses = await _context.Warehouses
                .OrderBy(w => w.Name)
                .Select(w => new PaintProductionWarehouseListItem { Id = w.Id, Name = w.Name })
                .ToListAsync(),
            PaintItems = await GetPaintItemListAsync(),
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

            // Step 2: Normalize material lines - drop empty rows and derive quantities/costs from the item master
            var selectedItemIds = model.Materials
                .Where(m => m.PaintItemId.HasValue && m.PaintItemId > 0)
                .Select(m => m.PaintItemId!.Value)
                .Distinct()
                .ToList();

            var itemLookup = await _context.PaintItems
                .Where(p => selectedItemIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, cancellationToken);

            var materialLines = new List<PaintProductionMaterialViewModel>();

            foreach (var line in model.Materials)
            {
                if (!line.PaintItemId.HasValue || line.PaintItemId <= 0)
                {
                    continue;
                }

                if (!itemLookup.TryGetValue(line.PaintItemId.Value, out var sourceItem))
                {
                    ModelState.AddModelError("", "One of the selected raw materials no longer exists. Please re-select it.");
                    continue;
                }

                // A single-step production consumes exactly what it requires
                if (line.ConsumedQuantity <= 0)
                {
                    line.ConsumedQuantity = line.RequiredQuantity;
                }

                if (line.RequiredQuantity <= 0)
                {
                    line.RequiredQuantity = line.ConsumedQuantity;
                }

                if (line.ConsumedQuantity <= 0)
                {
                    ModelState.AddModelError("", $"Quantity for '{sourceItem.Name}' must be greater than zero");
                    continue;
                }

                // Convert consumed quantity to the item's unit if different
                decimal consumedQuantityInItemUnit = line.ConsumedQuantity;
                decimal unitCostInUserUnit = line.UnitCost;

                if (!string.IsNullOrWhiteSpace(line.Unit) && !string.Equals(line.Unit, sourceItem.UnitOfMeasure, StringComparison.OrdinalIgnoreCase))
                {
                    // Convert quantity to item's unit for stock deduction
                    var convertedQuantity = await _unitConversionService.ConvertQuantityAsync(
                        line.ConsumedQuantity, line.Unit, sourceItem.UnitOfMeasure, cancellationToken);
                    if (convertedQuantity.HasValue)
                    {
                        consumedQuantityInItemUnit = convertedQuantity.Value;
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Cannot convert {line.Unit} to {sourceItem.UnitOfMeasure} for '{sourceItem.Name}'. Please add the conversion or use the same unit.");
                        continue;
                    }

                    // Convert unit cost from item's unit to user's unit for cost calculation
                    if (line.UnitCost <= 0)
                    {
                        var convertedUnitCost = await _unitConversionService.ConvertUnitCostAsync(
                            sourceItem.UnitCost, sourceItem.UnitOfMeasure, line.Unit, cancellationToken);
                        if (convertedUnitCost.HasValue)
                        {
                            unitCostInUserUnit = convertedUnitCost.Value;
                        }
                        else
                        {
                            // Fallback to item's unit cost if conversion fails
                            unitCostInUserUnit = sourceItem.UnitCost;
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(line.Unit))
                {
                    // Default to item's unit if not specified
                    line.Unit = sourceItem.UnitOfMeasure;
                    unitCostInUserUnit = sourceItem.UnitCost;
                }

                if (line.UnitCost <= 0)
                {
                    line.UnitCost = unitCostInUserUnit;
                }

                line.MaterialName = string.IsNullOrWhiteSpace(line.MaterialName) ? sourceItem.Name : line.MaterialName;
                line.TotalCost = decimal.Round(line.ConsumedQuantity * line.UnitCost, 2);
                line.StockBefore = sourceItem.CurrentStock;
                line.StockAfter = sourceItem.CurrentStock - consumedQuantityInItemUnit;

                if (sourceItem.WarehouseId != model.WarehouseId)
                {
                    ModelState.AddModelError("", $"'{sourceItem.Name}' is stocked in warehouse '{sourceItem.WarehouseName}'. Select that warehouse or transfer the stock first.");
                }
                else if (sourceItem.CurrentStock < consumedQuantityInItemUnit)
                {
                    ModelState.AddModelError("", $"Insufficient stock for '{sourceItem.Name}'. Available: {sourceItem.CurrentStock:0.##} {sourceItem.UnitOfMeasure}, required: {consumedQuantityInItemUnit:0.##} {sourceItem.UnitOfMeasure} (entered: {line.ConsumedQuantity:0.##} {line.Unit})");
                }

                materialLines.Add(line);
            }

            if (materialLines.Count == 0)
            {
                ModelState.AddModelError("", "Add at least one raw material with a quantity greater than zero");
            }

            if (model.OutputQuantity <= 0)
            {
                ModelState.AddModelError(nameof(model.OutputQuantity), "Output quantity must be greater than zero");
            }

            // Step 3: Resolve the finished product target - an existing item or a brand new one
            PaintItem? existingFinishedItem = null;

            if (model.CreateNewItem)
            {
                if (string.IsNullOrWhiteSpace(model.NewItemDetails.Name))
                {
                    ModelState.AddModelError("NewItemDetails.Name", "Item name is required when creating a new item");
                }
                else if (await _context.PaintItems.AnyAsync(p => p.Name == model.NewItemDetails.Name && p.WarehouseId == model.WarehouseId, cancellationToken))
                {
                    ModelState.AddModelError("NewItemDetails.Name", $"An item named '{model.NewItemDetails.Name}' already exists in this warehouse");
                }

                if (!string.IsNullOrWhiteSpace(model.NewItemDetails.SKU) &&
                    await _context.PaintItems.AnyAsync(p => p.SKU == model.NewItemDetails.SKU, cancellationToken))
                {
                    ModelState.AddModelError("NewItemDetails.SKU", $"SKU '{model.NewItemDetails.SKU}' is already in use");
                }
            }
            else if (!model.FinishedProductId.HasValue || model.FinishedProductId <= 0)
            {
                ModelState.AddModelError(nameof(model.FinishedProductId), "Select a finished product or tick 'Create New Item from Production'");
            }
            else
            {
                existingFinishedItem = await _context.PaintItems
                    .FirstOrDefaultAsync(p => p.Id == model.FinishedProductId.Value, cancellationToken);

                if (existingFinishedItem == null)
                {
                    ModelState.AddModelError(nameof(model.FinishedProductId), "Selected finished product no longer exists");
                }
                else if (existingFinishedItem.WarehouseId != model.WarehouseId)
                {
                    ModelState.AddModelError(nameof(model.FinishedProductId), $"'{existingFinishedItem.Name}' belongs to warehouse '{existingFinishedItem.WarehouseName}'. Select that warehouse instead.");
                }
            }

            var targetWarehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Id == model.WarehouseId, cancellationToken);

            if (targetWarehouse == null)
            {
                ModelState.AddModelError(nameof(model.WarehouseId), "Select a valid warehouse");
            }

            if (!ModelState.IsValid)
            {
                this.AddErrorMessage("Please correct the validation errors");
                if (model.Materials.Count == 0)
                {
                    model.Materials.Add(new PaintProductionMaterialViewModel());
                }
                await PopulateDropdowns(model);
                return View(model);
            }

            // Step 4: Recalculate costs on the server so the ledger cannot be tampered with from the browser
            model.MaterialCost = decimal.Round(materialLines.Sum(m => m.TotalCost), 2);
            model.ProductionCost = decimal.Round(model.MaterialCost + model.LaborCost + model.OverheadCost, 2);
            model.FinishedProductCost = model.ProductionCost;
            model.CostPerUnit = decimal.Round(model.ProductionCost / model.OutputQuantity, 4);

            if (string.IsNullOrWhiteSpace(model.FinishedProductDescription))
            {
                model.FinishedProductDescription = model.CreateNewItem
                    ? model.NewItemDetails.Name
                    : existingFinishedItem?.Name ?? model.Recipe;
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

            var currentUser = User.Identity?.Name ?? "System";

            // Build the production object first
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
                FinishedProductId = model.CreateNewItem ? null : model.FinishedProductId,
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
                CreatedBy = currentUser,
                UpdatedBy = currentUser
            };

            // Build (but do not persist yet) the new finished-goods item so it is created inside the transaction
            PaintItem? newItem = null;

            if (model.CreateNewItem)
            {
                // Cost of the new item is the production cost spread over the produced quantity
                var calculatedUnitCost = decimal.Round(model.ProductionCost / model.OutputQuantity, 4);
                model.NewItemDetails.CalculatedUnitCost = calculatedUnitCost;

                newItem = new PaintItem
                {
                    WarehouseId = model.WarehouseId,
                    SKU = string.IsNullOrWhiteSpace(model.NewItemDetails.SKU) ? GenerateSKU() : model.NewItemDetails.SKU,
                    Name = model.NewItemDetails.Name,
                    ItemType = "Finished Product",
                    Category = string.IsNullOrWhiteSpace(model.NewItemDetails.Category) ? "Finished Product" : model.NewItemDetails.Category,
                    WarehouseName = targetWarehouse!.Name,
                    UnitOfMeasure = model.NewItemDetails.UnitOfMeasure,
                    SourceProductionId = production.Id, // Track that this item was created from this production
                    PurchaseUnit = model.NewItemDetails.UnitOfMeasure,
                    SalesUnit = model.NewItemDetails.UnitOfMeasure,
                    CostMethod = "Average Cost",
                    PurchasePrice = calculatedUnitCost,
                    SellingPrice = model.NewItemDetails.SellingPrice,
                    UnitCost = calculatedUnitCost,
                    StockQuantity = 0,
                    CurrentStock = 0,
                    AvailableStock = 0,
                    ReservedStock = 0,
                    InventoryValue = 0,
                    ReorderLevel = 0,
                    Notes = model.NewItemDetails.Description,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow,
                    CreatedBy = currentUser,
                    UpdatedBy = currentUser
                };
            }

            // Add production materials from the normalized lines
            foreach (var material in materialLines)
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

            // Step 5: Business Validation
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
                    // Save production first to get its Id for foreign key references
                    _context.PaintProductions.Add(production);
                    await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Saved production {ProductionNumber} with Id {ProductionId}",
                        production.ProductionNumber, production.Id
                    );

                    // Now create the new finished-goods item with the correct SourceProductionId
                    if (newItem != null)
                    {
                        newItem.SourceProductionId = production.Id; // Set after production has an Id
                        _context.PaintItems.Add(newItem);
                        await _context.SaveChangesAsync(cancellationToken);

                        production.FinishedProductId = newItem.Id;

                        _logger.LogInformation(
                            "Created item {ItemName} (Id {ItemId}, SKU {Sku}) from production {ProductionNumber}",
                            newItem.Name, newItem.Id, newItem.SKU, production.ProductionNumber
                        );
                    }

                    // Consume raw materials - each toner is deducted from its inventory
                    foreach (var material in production.Materials)
                    {
                        if (!material.PaintItemId.HasValue || material.ConsumedQuantity <= 0)
                        {
                            continue;
                        }

                        // Get the source item to check its unit and convert if needed
                        var sourceItem = await _context.PaintItems
                            .FirstOrDefaultAsync(p => p.Id == material.PaintItemId.Value, cancellationToken);

                        if (sourceItem == null)
                        {
                            return TransactionResult.FailureResult(
                                $"Raw material '{material.MaterialName}' no longer exists",
                                "Item not found"
                            );
                        }

                        // Convert consumed quantity to item's unit if different
                        decimal quantityToDeduct = material.ConsumedQuantity;
                        if (!string.IsNullOrWhiteSpace(material.Unit) && !string.Equals(material.Unit, sourceItem.UnitOfMeasure, StringComparison.OrdinalIgnoreCase))
                        {
                            var convertedQuantity = await _unitConversionService.ConvertQuantityAsync(
                                material.ConsumedQuantity, material.Unit, sourceItem.UnitOfMeasure, cancellationToken);
                            if (convertedQuantity.HasValue)
                            {
                                quantityToDeduct = convertedQuantity.Value;
                                _logger.LogInformation(
                                    "Converted quantity for {Material}: {OriginalQty} {FromUnit} -> {ConvertedQty} {ToUnit}",
                                    sourceItem.Name, material.ConsumedQuantity, material.Unit, quantityToDeduct, sourceItem.UnitOfMeasure
                                );
                            }
                            else
                            {
                                return TransactionResult.FailureResult(
                                    $"Cannot convert {material.Unit} to {sourceItem.UnitOfMeasure} for '{sourceItem.Name}'",
                                    "Conversion not found"
                                );
                            }
                        }

                        _logger.LogInformation(
                            "Reducing stock for {Material}: ItemId={ItemId}, WarehouseId={WarehouseId}, QtyToDeduct={Qty}, ItemUnit={ItemUnit}, CurrentStock={Stock}",
                            sourceItem.Name, material.PaintItemId.Value, production.WarehouseId, quantityToDeduct, sourceItem.UnitOfMeasure, sourceItem.CurrentStock
                        );

                        var reduceResult = await _inventoryService.ReduceStockAsync(
                            material.PaintItemId.Value,
                            production.WarehouseId,
                            quantityToDeduct,
                            "Paint Production",
                            production.Id,
                            $"Production: {production.ProductionNumber}",
                            cancellationToken
                        );

                        if (!reduceResult.Success)
                        {
                            return TransactionResult.FailureResult(
                                $"Could not consume '{material.MaterialName}'",
                                reduceResult.Message
                            );
                        }

                        material.StockBefore = reduceResult.StockBefore;
                        material.StockAfter = reduceResult.StockAfter;
                    }

                    // Add the produced quantity of the finished product to inventory
                    if (production.FinishedProductId.HasValue && production.OutputQuantity > 0)
                    {
                        var increaseResult = await _inventoryService.IncreaseStockAsync(
                            production.FinishedProductId.Value,
                            production.WarehouseId,
                            production.OutputQuantity,
                            "Paint Production Output",
                            production.Id,
                            $"Production: {production.ProductionNumber}",
                            cancellationToken
                        );

                        if (!increaseResult.Success)
                        {
                            return TransactionResult.FailureResult(
                                "Could not add the finished product to inventory",
                                increaseResult.Message
                            );
                        }
                    }

                    await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Paint Production {ProductionNumber} created successfully",
                        production.ProductionNumber
                    );

                    var successMessage = newItem != null
                        ? $"Production {production.ProductionNumber} completed. Item '{newItem.Name}' ({newItem.SKU}) created and {production.OutputQuantity:0.##} added to inventory."
                        : $"Production {production.ProductionNumber} completed and inventory updated.";

                    return TransactionResult.SuccessResult(
                        successMessage,
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
                _logger.LogError("Transaction failed. Message: {Message}, Errors: {Errors}",
                    transactionResult.Message,
                    string.Join(", ", transactionResult.Errors));

                if (!string.IsNullOrWhiteSpace(transactionResult.Message))
                {
                    this.AddErrorMessage(transactionResult.Message);
                }

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

    // POST: PaintProduction/Complete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
        var production = await _context.PaintProductions.FindAsync(id);
        if (production == null)
        {
            return NotFound();
        }

        if (production.Status == "Completed")
        {
            this.AddWarningMessage("Production is already completed.");
            return RedirectToAction("Edit", new { id = id });
        }

        production.Status = "Completed";
        production.CompletedAtUtc = DateTime.UtcNow;
        production.UpdatedAtUtc = DateTime.UtcNow;
        production.UpdatedBy = User.Identity?.Name ?? "System";

        await _context.SaveChangesAsync();

        this.AddSuccessMessage($"Production {production.ProductionNumber} has been completed.");
        return RedirectToAction("Edit", new { id = id });
    }

    // POST: PaintProduction/OnHold/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnHold(int id)
    {
        var production = await _context.PaintProductions.FindAsync(id);
        if (production == null)
        {
            return NotFound();
        }

        if (production.Status == "On Hold")
        {
            this.AddWarningMessage("Production is already on hold.");
            return RedirectToAction("Index");
        }

        if (production.Status == "Completed" || production.Status == "Cancelled")
        {
            this.AddErrorMessage("Cannot put a completed or cancelled production on hold.");
            return RedirectToAction("Index");
        }

        production.Status = "On Hold";
        production.UpdatedAtUtc = DateTime.UtcNow;
        production.UpdatedBy = User.Identity?.Name ?? "System";

        await _context.SaveChangesAsync();

        this.AddSuccessMessage($"Production {production.ProductionNumber} has been put on hold.");
        return RedirectToAction("Index");
    }

    // POST: PaintProduction/Cancel/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var production = await _context.PaintProductions.FindAsync(id);
        if (production == null)
        {
            return NotFound();
        }

        if (production.Status == "Cancelled")
        {
            this.AddWarningMessage("Production is already cancelled.");
            return RedirectToAction("Index");
        }

        if (production.Status == "Completed")
        {
            this.AddErrorMessage("Cannot cancel a completed production.");
            return RedirectToAction("Index");
        }

        production.Status = "Cancelled";
        production.UpdatedAtUtc = DateTime.UtcNow;
        production.UpdatedBy = User.Identity?.Name ?? "System";

        await _context.SaveChangesAsync();

        this.AddSuccessMessage($"Production {production.ProductionNumber} has been cancelled.");
        return RedirectToAction("Index");
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

        model.PaintItems = await GetPaintItemListAsync();

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

    private async Task<List<PaintProductionPaintItemListItem>> GetPaintItemListAsync()
    {
        return await _context.PaintItems
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new PaintProductionPaintItemListItem
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU ?? "",
                UnitCost = p.UnitCost,
                WarehouseId = p.WarehouseId,
                CurrentStock = p.CurrentStock,
                UnitOfMeasure = p.UnitOfMeasure ?? ""
            })
            .ToListAsync();
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

    private string GenerateSKU()
    {
        var prefix = "SKU";
        var year = DateTime.UtcNow.Year.ToString().Substring(2);
        var count = _context.PaintItems.Count() + 1;
        return $"{prefix}{year}-{count:D4}";
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
