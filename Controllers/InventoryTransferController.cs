using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Helpers;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;
using PaintERP.Services;

namespace PaintERP.Controllers;

public class InventoryTransferController : Controller
{
    private readonly PaintErpDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IInventoryService _inventoryService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<InventoryTransferController> _logger;

    public InventoryTransferController(
        PaintErpDbContext context,
        IValidationService validationService,
        IInventoryService inventoryService,
        ITransactionService transactionService,
        INotificationService notificationService,
        ILogger<InventoryTransferController> logger)
    {
        _context = context;
        _validationService = validationService;
        _inventoryService = inventoryService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _logger = logger;
    }

    // GET: InventoryTransfer
    public async Task<IActionResult> Index()
    {
        var transfers = await _context.InventoryTransfers
            .Include(it => it.SourceWarehouse)
            .Include(it => it.DestinationWarehouse)
            .OrderByDescending(it => it.TransferDate)
            .ToListAsync();

        return View(transfers);
    }

    // GET: InventoryTransfer/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var transfer = await _context.InventoryTransfers
            .Include(it => it.SourceWarehouse)
            .Include(it => it.DestinationWarehouse)
            .Include(it => it.InventoryTransferItems)
                .ThenInclude(iti => iti.PaintItem)
            .FirstOrDefaultAsync(it => it.Id == id);

        if (transfer == null)
        {
            return NotFound();
        }

        return View(transfer);
    }

    // GET: InventoryTransfer/Create
    public async Task<IActionResult> Create()
    {
        var model = new InventoryTransferFormViewModel
        {
            TransferNumber = await GenerateTransferNumber(),
            TransferDate = DateTime.UtcNow,
            Status = "Draft",
            InventoryTransferItems = new List<InventoryTransferItemViewModel> { new InventoryTransferItemViewModel() }
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    // POST: InventoryTransfer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InventoryTransferFormViewModel model, CancellationToken cancellationToken)
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

            var transfer = new InventoryTransfer
            {
                CompanyId = model.CompanyId,
                TransferNumber = model.TransferNumber,
                SourceWarehouseId = model.SourceWarehouseId,
                DestinationWarehouseId = model.DestinationWarehouseId,
                TransferDate = model.TransferDate,
                Status = model.Status,
                TrackingNumber = model.TrackingNumber,
                ShippedBy = model.ShippedBy,
                ReceivedBy = model.ReceivedBy,
                ApprovedAtUtc = model.ApprovedAtUtc,
                ShippedAtUtc = model.ShippedAtUtc,
                ReceivedAtUtc = model.ReceivedAtUtc,
                Notes = model.Notes,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name ?? "System",
                UpdatedBy = User.Identity?.Name ?? "System"
            };

            foreach (var item in model.InventoryTransferItems.Where(iti => iti.Quantity > 0))
            {
                transfer.InventoryTransferItems.Add(new InventoryTransferItem
                {
                    PaintItemId = item.PaintItemId,
                    SKU = item.SKU,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    BatchNumber = item.BatchNumber,
                    SourceStockBefore = item.SourceStockBefore,
                    SourceStockAfter = item.SourceStockAfter,
                    DestinationStockBefore = item.DestinationStockBefore,
                    DestinationStockAfter = item.DestinationStockAfter
                });
            }

            // Step 2: Business Validation
            var validationResult = await _validationService.ValidateInventoryTransferAsync(transfer, cancellationToken);
            
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
                    // Save transfer
                    _context.InventoryTransfers.Add(transfer);
                    await _context.SaveChangesAsync(cancellationToken);

                    // Process inventory movements
                    if (transfer.InventoryTransferItems != null && transfer.InventoryTransferItems.Any())
                    {
                        foreach (var item in transfer.InventoryTransferItems)
                        {
                            // Decrease stock from source warehouse
                            await _inventoryService.ReduceStockAsync(
                                item.PaintItemId ?? 0,
                                transfer.SourceWarehouseId,
                                item.Quantity,
                                "Inventory Transfer Out",
                                transfer.Id,
                                $"Transfer: {transfer.TransferNumber}",
                                cancellationToken
                            );

                            // Increase stock in destination warehouse
                            await _inventoryService.IncreaseStockAsync(
                                item.PaintItemId ?? 0,
                                transfer.DestinationWarehouseId,
                                item.Quantity,
                                "Inventory Transfer In",
                                transfer.Id,
                                $"Transfer: {transfer.TransferNumber}",
                                cancellationToken
                            );
                        }
                    }

                    _logger.LogInformation(
                        "Inventory Transfer {TransferNumber} created successfully",
                        transfer.TransferNumber
                    );

                    return TransactionResult.SuccessResult(
                        $"Inventory Transfer {transfer.TransferNumber} created successfully",
                        new { TransferId = transfer.Id }
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating inventory transfer: {Message}", ex.Message);
                    return TransactionResult.FailureResult(
                        "An error occurred while creating the inventory transfer",
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

    // GET: InventoryTransfer/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var transfer = await _context.InventoryTransfers
            .Include(it => it.InventoryTransferItems)
            .FirstOrDefaultAsync(it => it.Id == id);

        if (transfer == null)
        {
            return NotFound();
        }

        var model = new InventoryTransferFormViewModel
        {
            Id = transfer.Id,
            CompanyId = transfer.CompanyId,
            TransferNumber = transfer.TransferNumber,
            SourceWarehouseId = transfer.SourceWarehouseId,
            DestinationWarehouseId = transfer.DestinationWarehouseId,
            TransferDate = transfer.TransferDate,
            Status = transfer.Status,
            TrackingNumber = transfer.TrackingNumber,
            ShippedBy = transfer.ShippedBy,
            ReceivedBy = transfer.ReceivedBy,
            ApprovedAtUtc = transfer.ApprovedAtUtc,
            ShippedAtUtc = transfer.ShippedAtUtc,
            ReceivedAtUtc = transfer.ReceivedAtUtc,
            Notes = transfer.Notes,
            InventoryTransferItems = transfer.InventoryTransferItems.Select(iti => new InventoryTransferItemViewModel
            {
                Id = iti.Id,
                InventoryTransferId = iti.InventoryTransferId,
                SKU = iti.SKU,
                Description = iti.Description,
                PaintItemId = iti.PaintItemId,
                Quantity = iti.Quantity,
                Unit = iti.Unit,
                BatchNumber = iti.BatchNumber,
                SourceStockBefore = iti.SourceStockBefore,
                SourceStockAfter = iti.SourceStockAfter,
                DestinationStockBefore = iti.DestinationStockBefore,
                DestinationStockAfter = iti.DestinationStockAfter
            }).ToList()
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    // POST: InventoryTransfer/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, InventoryTransferFormViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model);
            return View(model);
        }

        var transfer = await _context.InventoryTransfers
            .Include(it => it.InventoryTransferItems)
            .FirstOrDefaultAsync(it => it.Id == id);

        if (transfer == null)
        {
            return NotFound();
        }

        transfer.SourceWarehouseId = model.SourceWarehouseId;
        transfer.DestinationWarehouseId = model.DestinationWarehouseId;
        transfer.TransferDate = model.TransferDate;
        transfer.Status = model.Status;
        transfer.TrackingNumber = model.TrackingNumber;
        transfer.ShippedBy = model.ShippedBy;
        transfer.ReceivedBy = model.ReceivedBy;
        transfer.ApprovedAtUtc = model.ApprovedAtUtc;
        transfer.ShippedAtUtc = model.ShippedAtUtc;
        transfer.ReceivedAtUtc = model.ReceivedAtUtc;
        transfer.Notes = model.Notes;
        transfer.UpdatedAtUtc = DateTime.UtcNow;
        transfer.UpdatedBy = User.Identity?.Name ?? "System";

        var existingItems = transfer.InventoryTransferItems.ToList();
        _context.InventoryTransferItems.RemoveRange(existingItems);

        foreach (var item in model.InventoryTransferItems.Where(iti => iti.Quantity > 0))
        {
            transfer.InventoryTransferItems.Add(new InventoryTransferItem
            {
                PaintItemId = item.PaintItemId,
                SKU = item.SKU,
                Description = item.Description,
                Quantity = item.Quantity,
                Unit = item.Unit,
                BatchNumber = item.BatchNumber,
                SourceStockBefore = item.SourceStockBefore,
                SourceStockAfter = item.SourceStockAfter,
                DestinationStockBefore = item.DestinationStockBefore,
                DestinationStockAfter = item.DestinationStockAfter
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: InventoryTransfer/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var transfer = await _context.InventoryTransfers
            .Include(it => it.SourceWarehouse)
            .Include(it => it.DestinationWarehouse)
            .FirstOrDefaultAsync(it => it.Id == id);

        if (transfer == null)
        {
            return NotFound();
        }

        return View(transfer);
    }

    // POST: InventoryTransfer/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        try
        {
            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                var transfer = await _context.InventoryTransfers
                    .Include(it => it.InventoryTransferItems)
                    .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

                if (transfer == null)
                {
                    return TransactionResult.FailureResult("Inventory transfer not found");
                }

                // Reverse inventory movements
                if (transfer.InventoryTransferItems != null && transfer.InventoryTransferItems.Any())
                {
                    foreach (var item in transfer.InventoryTransferItems)
                    {
                        // Restore stock to source warehouse
                        await _inventoryService.IncreaseStockAsync(
                            item.PaintItemId ?? 0,
                            transfer.SourceWarehouseId,
                            item.Quantity,
                            "Inventory Transfer Reversal",
                            transfer.Id,
                            $"Reversal of Transfer: {transfer.TransferNumber}",
                            cancellationToken
                        );

                        // Remove stock from destination warehouse
                        await _inventoryService.ReduceStockAsync(
                            item.PaintItemId ?? 0,
                            transfer.DestinationWarehouseId,
                            item.Quantity,
                            "Inventory Transfer Reversal",
                            transfer.Id,
                            $"Reversal of Transfer: {transfer.TransferNumber}",
                            cancellationToken
                        );
                    }
                }

                // Delete transfer
                _context.InventoryTransfers.Remove(transfer);
                await _context.SaveChangesAsync(cancellationToken);

                return TransactionResult.SuccessResult($"Inventory Transfer {transfer.TransferNumber} deleted successfully");
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
            _logger.LogError(ex, "Error deleting inventory transfer: {Message}", ex.Message);
            return this.WithError("An error occurred while deleting the inventory transfer", "Index");
        }
    }

    private async Task<string> GenerateTransferNumber()
    {
        var lastTransfer = await _context.InventoryTransfers
            .OrderByDescending(it => it.Id)
            .FirstOrDefaultAsync();

        if (lastTransfer == null)
        {
            return "IT-0001";
        }

        var lastNumber = lastTransfer.TransferNumber.Replace("IT-", "");
        if (int.TryParse(lastNumber, out int number))
        {
            return $"IT-{(number + 1):D4}";
        }

        return "IT-0001";
    }

    private async Task PopulateDropdowns(InventoryTransferFormViewModel model)
    {
        model.Warehouses = await _context.Warehouses
            .OrderBy(w => w.Name)
            .Select(w => new InventoryTransferWarehouseListItem { Id = w.Id, Name = w.Name })
            .ToListAsync();

        model.PaintItems = await _context.PaintItems
            .OrderBy(p => p.Name)
            .Select(p => new InventoryTransferPaintItemListItem { Id = p.Id, Name = p.Name, SKU = p.SKU ?? "", UnitOfMeasure = p.UnitOfMeasure ?? "" })
            .ToListAsync();
    }
}
