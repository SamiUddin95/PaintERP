using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Helpers;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;
using PaintERP.Services;

namespace PaintERP.Controllers;

// [Authorize] - TODO: Enable when authentication is implemented
public class GoodsReceivedNoteController : Controller
{
    private readonly PaintErpDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IInventoryService _inventoryService;
    private readonly IAccountingService _accountingService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<GoodsReceivedNoteController> _logger;

    public GoodsReceivedNoteController(
        PaintErpDbContext context,
        IValidationService validationService,
        IInventoryService inventoryService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService,
        ILogger<GoodsReceivedNoteController> logger)
    {
        _context = context;
        _validationService = validationService;
        _inventoryService = inventoryService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _logger = logger;
    }

    // GET: GoodsReceivedNote
    public async Task<IActionResult> Index(int? page, int pageSize = 10)
    {
        var pageNumber = page ?? 1;

        var query = _context.GoodsReceivedNotes
            .Include(grn => grn.PurchaseOrder)
            .Include(grn => grn.Vendor)
            .Include(grn => grn.GoodsReceivedNoteItems)
            .OrderByDescending(grn => grn.GRNDate)
            .Select(grn => new GRNListItem
            {
                Id = grn.Id,
                GRNNumber = grn.GRNNumber,
                PONumber = grn.PurchaseOrder.PONumber,
                VendorName = grn.Vendor.BusinessName,
                GRNDate = grn.GRNDate,
                TotalAmount = grn.TotalAmount,
                Status = grn.Status,
                ItemsCount = grn.GoodsReceivedNoteItems.Count,
                IsPosted = grn.IsPosted
            });

        var grns = await PaginatedList<GRNListItem>.CreateAsync(query, pageNumber, pageSize);

        return View(grns);
    }

    // GET: GoodsReceivedNote/Create
    public async Task<IActionResult> Create(int? purchaseOrderId, CancellationToken cancellationToken)
    {
        var model = new GoodsReceivedNoteFormViewModel
        {
            GRNDate = DateTime.Today,
            GRNNumber = await GenerateGRNNumberAsync(cancellationToken)
        };

        await PopulateDropdowns(model, purchaseOrderId, cancellationToken);

        if (purchaseOrderId.HasValue)
        {
            await LoadPurchaseOrderItemsAsync(model, purchaseOrderId.Value, cancellationToken);
        }

        return View(model);
    }

    // POST: GoodsReceivedNote/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GoodsReceivedNoteFormViewModel model, CancellationToken cancellationToken)
    {
        try
        {
            // Initialize collections if null
            model.Warehouses ??= new List<GRNWarehouseListItem>();
            model.PurchaseOrders ??= new List<GRNPOListItem>();
            model.GoodsReceivedNoteItems ??= new List<GoodsReceivedNoteItemViewModel>();

            _logger.LogInformation("GRN Create POST - Subtotal={Subtotal}, Discount={Discount}, Tax={Tax}, Total={Total}",
                model.Subtotal, model.DiscountAmount, model.TaxAmount, model.TotalAmount);

            // Recalculate totals from items if they are 0 (fallback)
            if (model.TotalAmount == 0 && model.GoodsReceivedNoteItems.Any())
            {
                decimal subtotal = 0;
                decimal discountAmount = 0;
                decimal taxAmount = 0;

                foreach (var item in model.GoodsReceivedNoteItems)
                {
                    var lineSubtotal = item.QuantityReceived * item.UnitCost;
                    var lineDiscount = lineSubtotal * (item.DiscountPercent / 100);
                    var afterDiscount = lineSubtotal - lineDiscount;
                    var lineTax = afterDiscount * (item.TaxPercent / 100);
                    var lineTotal = afterDiscount + lineTax;

                    subtotal += lineSubtotal;
                    discountAmount += lineDiscount;
                    taxAmount += lineTax;

                    item.LineTotal = lineTotal;
                    item.DiscountAmount = lineDiscount;
                    item.TaxAmount = lineTax;
                }

                model.Subtotal = subtotal;
                model.DiscountAmount = discountAmount;
                model.TaxAmount = taxAmount;
                model.TotalAmount = subtotal - discountAmount + taxAmount;

                _logger.LogInformation("Recalculated totals - Subtotal={Subtotal}, Discount={Discount}, Tax={Tax}, Total={Total}",
                    model.Subtotal, model.DiscountAmount, model.TaxAmount, model.TotalAmount);
            }

            // Step 1: Validate ModelState
            if (!ModelState.IsValid)
            {
                this.AddErrorMessage("Please correct the validation errors");
                await PopulateDropdowns(model, model.PurchaseOrderId, cancellationToken);
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

            var goodsReceivedNote = new GoodsReceivedNote
            {
                CompanyId = defaultCompanyId,
                PurchaseOrderId = model.PurchaseOrderId,
                GRNNumber = model.GRNNumber,
                GRNDate = model.GRNDate,
                WarehouseId = model.WarehouseId,
                VendorId = model.VendorId,
                ReferenceNumber = model.ReferenceNumber,
                Status = model.Status,
                Subtotal = model.Subtotal,
                DiscountAmount = model.DiscountAmount,
                TaxAmount = model.TaxAmount,
                TotalAmount = model.TotalAmount,
                InternalNotes = model.InternalNotes,
                VendorNotes = model.VendorNotes,
                IsPosted = false
            };

            // Add GRN items
            foreach (var item in model.GoodsReceivedNoteItems.Where(gi => gi.QuantityReceived > 0))
            {
                goodsReceivedNote.GoodsReceivedNoteItems.Add(new GoodsReceivedNoteItem
                {
                    PurchaseOrderItemId = item.PurchaseOrderItemId,
                    SKU = item.SKU,
                    Description = item.Description,
                    PaintItemId = item.PaintItemId,
                    QuantityOrdered = item.QuantityOrdered,
                    QuantityPreviouslyReceived = item.QuantityPreviouslyReceived,
                    QuantityReceived = item.QuantityReceived,
                    QuantityRemaining = item.QuantityRemaining,
                    Unit = item.Unit,
                    UnitCost = item.UnitCost,
                    DiscountPercent = item.DiscountPercent,
                    DiscountAmount = item.DiscountAmount,
                    TaxPercent = item.TaxPercent,
                    TaxAmount = item.TaxAmount,
                    LineTotal = item.LineTotal,
                    BatchNumber = item.BatchNumber,
                    ExpiryDate = item.ExpiryDate,
                    Location = item.Location,
                    Remarks = item.Remarks
                });
            }

            // Step 2: Business Validation
            var validationResult = await _validationService.ValidateGoodsReceivedNoteAsync(goodsReceivedNote, cancellationToken);

            if (!validationResult.IsValid)
            {
                _notificationService.AddValidationErrors(validationResult);
                await PopulateDropdowns(model, model.PurchaseOrderId, cancellationToken);
                return this.WithValidationErrors(validationResult, model);
            }

            // Display warnings if any
            foreach (var warning in validationResult.Warnings)
            {
                this.AddWarningMessage(warning);
            }

            // Step 3: Execute in Transaction
            _logger.LogInformation("Starting transaction for GRN creation: GRNNumber={GRNNumber}, PurchaseOrderId={PurchaseOrderId}, TotalAmount={TotalAmount}",
                goodsReceivedNote.GRNNumber, goodsReceivedNote.PurchaseOrderId, goodsReceivedNote.TotalAmount);

            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                try
                {
                    // Set audit fields inside transaction
                    goodsReceivedNote.CreatedAtUtc = DateTime.UtcNow;
                    goodsReceivedNote.UpdatedAtUtc = DateTime.UtcNow;
                    goodsReceivedNote.CreatedBy = User.Identity?.Name ?? "System";
                    goodsReceivedNote.UpdatedBy = User.Identity?.Name ?? "System";

                    // Save GRN
                    _context.GoodsReceivedNotes.Add(goodsReceivedNote);
                    var saveResult = await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "GRN {GRNNumber} saved to database with Id={GRNId}, SaveChangesAsync returned {SaveResult}",
                        goodsReceivedNote.GRNNumber, goodsReceivedNote.Id, saveResult
                    );

                    // Update PurchaseOrderItem quantities
                    foreach (var grnItem in goodsReceivedNote.GoodsReceivedNoteItems)
                    {
                        var poItem = await _context.PurchaseOrderItems
                            .FirstOrDefaultAsync(poi => poi.Id == grnItem.PurchaseOrderItemId, cancellationToken);

                        if (poItem != null)
                        {
                            poItem.QuantityReceived += grnItem.QuantityReceived;
                            poItem.QuantityPending = poItem.QuantityOrdered - poItem.QuantityReceived;
                            poItem.IsFullyReceived = poItem.QuantityReceived >= poItem.QuantityOrdered;
                        }
                    }

                    // Update PurchaseOrder status
                    var purchaseOrder = await _context.PurchaseOrders
                        .Include(po => po.PurchaseOrderItems)
                        .FirstOrDefaultAsync(po => po.Id == goodsReceivedNote.PurchaseOrderId, cancellationToken);

                    if (purchaseOrder != null)
                    {
                        purchaseOrder.IsFullyReceived = purchaseOrder.PurchaseOrderItems.All(poi => poi.IsFullyReceived);
                        purchaseOrder.AmountReceived += goodsReceivedNote.TotalAmount;

                        // Update PO status based on receipt status
                        if (purchaseOrder.IsFullyReceived)
                        {
                            purchaseOrder.Status = "Received";
                        }
                        else if (purchaseOrder.PurchaseOrderItems.Any(poi => poi.QuantityReceived > 0))
                        {
                            // Some items received but not all
                            purchaseOrder.Status = "Partially Received";
                        }

                        purchaseOrder.UpdatedAtUtc = DateTime.UtcNow;
                        purchaseOrder.UpdatedBy = User.Identity?.Name ?? "System";
                    }

                    await _context.SaveChangesAsync(cancellationToken);

                    return TransactionResult.SuccessResult(
                        $"GRN {goodsReceivedNote.GRNNumber} created successfully",
                        new { GRNId = goodsReceivedNote.Id }
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating GRN: {Message}, StackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                    return TransactionResult.FailureResult(
                        "An error occurred while creating the GRN",
                        ex.Message
                    );
                }
            }, cancellationToken);

            _logger.LogInformation("Transaction completed: Success={Success}, Message={Message}",
                transactionResult.Success, transactionResult.Message);

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
                await PopulateDropdowns(model, model.PurchaseOrderId, cancellationToken);
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in Create action: {Message}", ex.Message);
            model.Warehouses ??= new List<GRNWarehouseListItem>();
            model.PurchaseOrders ??= new List<GRNPOListItem>();
            model.GoodsReceivedNoteItems ??= new List<GoodsReceivedNoteItemViewModel>();
            await PopulateDropdowns(model, model.PurchaseOrderId, cancellationToken);
            return this.WithError("An unexpected error occurred. Please try again.", "Index");
        }
    }

    // GET: GoodsReceivedNote/Edit/5
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var grn = await _context.GoodsReceivedNotes
            .Include(g => g.PurchaseOrder)
            .Include(g => g.Vendor)
            .Include(g => g.Warehouse)
            .Include(g => g.GoodsReceivedNoteItems)
                .ThenInclude(gi => gi.PaintItem)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        if (grn == null)
        {
            return NotFound();
        }

        if (grn.IsPosted)
        {
            this.AddErrorMessage("Cannot edit a posted GRN");
            return RedirectToAction(nameof(Details), new { id });
        }

        var model = new GoodsReceivedNoteFormViewModel
        {
            Id = grn.Id,
            GRNNumber = grn.GRNNumber,
            GRNDate = grn.GRNDate,
            PurchaseOrderId = grn.PurchaseOrderId,
            PONumber = grn.PurchaseOrder?.PONumber,
            VendorId = grn.VendorId,
            VendorName = grn.Vendor?.BusinessName,
            WarehouseId = grn.WarehouseId,
            ExpectedDeliveryDate = grn.PurchaseOrder?.ExpectedDeliveryDate,
            ReferenceNumber = grn.ReferenceNumber,
            InternalNotes = grn.InternalNotes,
            VendorNotes = grn.VendorNotes,
            Status = grn.Status,
            Subtotal = grn.Subtotal,
            DiscountAmount = grn.DiscountAmount,
            TaxAmount = grn.TaxAmount,
            TotalAmount = grn.TotalAmount
        };

        await PopulateDropdowns(model, grn.PurchaseOrderId, cancellationToken);

        // Load existing GRN items
        foreach (var grnItem in grn.GoodsReceivedNoteItems)
        {
            model.GoodsReceivedNoteItems.Add(new GoodsReceivedNoteItemViewModel
            {
                PurchaseOrderItemId = grnItem.PurchaseOrderItemId,
                SKU = grnItem.SKU,
                Description = grnItem.Description,
                PaintItemId = grnItem.PaintItemId,
                QuantityOrdered = grnItem.QuantityOrdered,
                QuantityPreviouslyReceived = grnItem.QuantityPreviouslyReceived,
                QuantityRemaining = grnItem.QuantityRemaining,
                QuantityReceived = grnItem.QuantityReceived,
                Unit = grnItem.Unit,
                UnitCost = grnItem.UnitCost,
                DiscountPercent = grnItem.DiscountPercent,
                DiscountAmount = grnItem.DiscountAmount,
                TaxPercent = grnItem.TaxPercent,
                TaxAmount = grnItem.TaxAmount,
                LineTotal = grnItem.LineTotal,
                BatchNumber = grnItem.BatchNumber,
                ExpiryDate = grnItem.ExpiryDate,
                Location = grnItem.Location,
                Remarks = grnItem.Remarks
            });
        }

        return View("Create", model);
    }

    // POST: GoodsReceivedNote/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GoodsReceivedNoteFormViewModel model, CancellationToken cancellationToken)
    {
        var grn = await _context.GoodsReceivedNotes
            .Include(g => g.GoodsReceivedNoteItems)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        if (grn == null)
        {
            return NotFound();
        }

        if (grn.IsPosted)
        {
            this.AddErrorMessage("Cannot edit a posted GRN");
            return RedirectToAction(nameof(Details), new { id });
        }

        try
        {
            // Initialize collections if null
            model.Warehouses ??= new List<GRNWarehouseListItem>();
            model.PurchaseOrders ??= new List<GRNPOListItem>();
            model.GoodsReceivedNoteItems ??= new List<GoodsReceivedNoteItemViewModel>();

            // Step 1: Validate ModelState
            if (!ModelState.IsValid)
            {
                this.AddErrorMessage("Please correct the validation errors");
                await PopulateDropdowns(model, model.PurchaseOrderId, cancellationToken);
                return View("Create", model);
            }

            // Update GRN properties
            grn.GRNDate = model.GRNDate;
            grn.WarehouseId = model.WarehouseId;
            grn.ReferenceNumber = model.ReferenceNumber;
            grn.Status = model.Status;
            grn.Subtotal = model.Subtotal;
            grn.DiscountAmount = model.DiscountAmount;
            grn.TaxAmount = model.TaxAmount;
            grn.TotalAmount = model.TotalAmount;
            grn.InternalNotes = model.InternalNotes;
            grn.VendorNotes = model.VendorNotes;
            grn.UpdatedAtUtc = DateTime.UtcNow;
            grn.UpdatedBy = User.Identity?.Name ?? "System";

            // Remove existing items
            var existingItems = grn.GoodsReceivedNoteItems.ToList();
            _context.GoodsReceivedNoteItems.RemoveRange(existingItems);

            // Add updated items
            foreach (var item in model.GoodsReceivedNoteItems.Where(gi => gi.QuantityReceived > 0))
            {
                grn.GoodsReceivedNoteItems.Add(new GoodsReceivedNoteItem
                {
                    PurchaseOrderItemId = item.PurchaseOrderItemId,
                    SKU = item.SKU,
                    Description = item.Description,
                    PaintItemId = item.PaintItemId,
                    QuantityOrdered = item.QuantityOrdered,
                    QuantityPreviouslyReceived = item.QuantityPreviouslyReceived,
                    QuantityReceived = item.QuantityReceived,
                    QuantityRemaining = item.QuantityRemaining,
                    Unit = item.Unit,
                    UnitCost = item.UnitCost,
                    DiscountPercent = item.DiscountPercent,
                    DiscountAmount = item.DiscountAmount,
                    TaxPercent = item.TaxPercent,
                    TaxAmount = item.TaxAmount,
                    LineTotal = item.LineTotal,
                    BatchNumber = item.BatchNumber,
                    ExpiryDate = item.ExpiryDate,
                    Location = item.Location,
                    Remarks = item.Remarks
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            this.AddSuccessMessage($"GRN {grn.GRNNumber} updated successfully");
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in Edit action: {Message}", ex.Message);
            model.Warehouses ??= new List<GRNWarehouseListItem>();
            model.PurchaseOrders ??= new List<GRNPOListItem>();
            model.GoodsReceivedNoteItems ??= new List<GoodsReceivedNoteItemViewModel>();
            await PopulateDropdowns(model, model.PurchaseOrderId, cancellationToken);
            this.AddErrorMessage("An unexpected error occurred. Please try again.");
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    // GET: GoodsReceivedNote/Details/5
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var grn = await _context.GoodsReceivedNotes
            .Include(grn => grn.PurchaseOrder)
            .Include(grn => grn.Vendor)
            .Include(grn => grn.Warehouse)
            .Include(grn => grn.GoodsReceivedNoteItems)
                .ThenInclude(grni => grni.PaintItem)
            .Include(grn => grn.GoodsReceivedNoteItems)
                .ThenInclude(grni => grni.PurchaseOrderItem)
            .FirstOrDefaultAsync(grn => grn.Id == id, cancellationToken);

        if (grn == null)
        {
            return NotFound();
        }

        return View(grn);
    }

    // POST: GoodsReceivedNote/Post/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Post(int id, CancellationToken cancellationToken)
    {
        var grn = await _context.GoodsReceivedNotes
            .Include(grn => grn.GoodsReceivedNoteItems)
            .Include(grn => grn.Warehouse)
            .FirstOrDefaultAsync(grn => grn.Id == id, cancellationToken);

        if (grn == null)
        {
            return NotFound();
        }

        if (grn.IsPosted)
        {
            this.AddErrorMessage("GRN is already posted");
            return RedirectToAction(nameof(Details), new { id });
        }

        var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
        {
            try
            {
                // Update inventory for each item
                foreach (var grnItem in grn.GoodsReceivedNoteItems)
                {
                    if (grnItem.PaintItemId.HasValue && grn.WarehouseId.HasValue)
                    {
                        await _inventoryService.IncreaseStockAsync(
                            grnItem.PaintItemId.Value,
                            grn.WarehouseId.Value,
                            grnItem.QuantityReceived,
                            "GRN Receipt",
                            grn.Id,
                            $"GRN: {grn.GRNNumber}",
                            cancellationToken
                        );
                    }
                }

                // Mark GRN as posted
                grn.IsPosted = true;
                grn.PostedDate = DateTime.UtcNow;
                grn.PostedBy = User.Identity?.Name ?? "System";
                grn.Status = "Posted";
                grn.UpdatedAtUtc = DateTime.UtcNow;
                grn.UpdatedBy = User.Identity?.Name ?? "System";

                await _context.SaveChangesAsync(cancellationToken);

                return TransactionResult.SuccessResult($"GRN {grn.GRNNumber} posted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting GRN: {Message}", ex.Message);
                return TransactionResult.FailureResult("An error occurred while posting the GRN", ex.Message);
            }
        }, cancellationToken);

        if (transactionResult.Success)
        {
            return this.WithSuccess(transactionResult.Message, nameof(Details), new { id });
        }
        else
        {
            foreach (var error in transactionResult.Errors)
            {
                this.AddErrorMessage(error);
            }
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    // GET: GoodsReceivedNote/ConvertToBill/5
    public async Task<IActionResult> ConvertToBill(int id, CancellationToken cancellationToken)
    {
        var grn = await _context.GoodsReceivedNotes
            .Include(grn => grn.PurchaseOrder)
            .Include(grn => grn.Vendor)
            .Include(grn => grn.GoodsReceivedNoteItems)
            .FirstOrDefaultAsync(grn => grn.Id == id, cancellationToken);

        if (grn == null)
        {
            return NotFound();
        }

        if (!grn.IsPosted)
        {
            this.AddErrorMessage("GRN must be posted before converting to bill");
            return RedirectToAction(nameof(Details), new { id });
        }

        if (grn.ConvertedBillId.HasValue)
        {
            this.AddErrorMessage("GRN has already been converted to a bill");
            return RedirectToAction(nameof(Details), new { id });
        }

        // Redirect to Bill Create with GRN data
        // This would be implemented when Bill Create supports GRN pre-population
        this.AddInfoMessage($"GRN {grn.GRNNumber} data loaded. Complete the bill creation.");
        return RedirectToAction("Create", "Bill", new { grnId = id });
    }

    private async Task PopulateDropdowns(GoodsReceivedNoteFormViewModel model, int? purchaseOrderId, CancellationToken cancellationToken)
    {
        model.Warehouses = await _context.Warehouses
            .OrderBy(w => w.Name)
            .Select(w => new GRNWarehouseListItem { Id = w.Id, Name = w.Name })
            .ToListAsync(cancellationToken);

        // Filter POs that are approved and not fully received
        var poQuery = _context.PurchaseOrders
            .Include(po => po.Vendor)
            .OrderByDescending(po => po.OrderDate);

        // If a specific PO is provided, include it even if it doesn't meet normal criteria
        if (purchaseOrderId.HasValue)
        {
            model.PurchaseOrders = await poQuery
                .Where(po => (po.IsApproved && !po.IsCancelled && !po.IsFullyReceived) || po.Id == purchaseOrderId.Value)
                .Select(po => new GRNPOListItem
                {
                    Id = po.Id,
                    PONumber = po.PONumber,
                    VendorName = po.Vendor.BusinessName,
                    OrderDate = po.OrderDate,
                    TotalAmount = po.TotalAmount,
                    Status = po.Status,
                    IsFullyReceived = po.IsFullyReceived,
                    VendorId = po.VendorId,
                    WarehouseId = po.WarehouseId
                })
                .ToListAsync(cancellationToken);
        }
        else
        {
            model.PurchaseOrders = await poQuery
                .Where(po => po.IsApproved && !po.IsCancelled && !po.IsFullyReceived)
                .Select(po => new GRNPOListItem
                {
                    Id = po.Id,
                    PONumber = po.PONumber,
                    VendorName = po.Vendor.BusinessName,
                    OrderDate = po.OrderDate,
                    TotalAmount = po.TotalAmount,
                    Status = po.Status,
                    IsFullyReceived = po.IsFullyReceived,
                    VendorId = po.VendorId,
                    WarehouseId = po.WarehouseId
                })
                .ToListAsync(cancellationToken);
        }
    }

    private async Task LoadPurchaseOrderItemsAsync(GoodsReceivedNoteFormViewModel model, int purchaseOrderId, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.Vendor)
            .Include(po => po.PurchaseOrderItems)
            .FirstOrDefaultAsync(po => po.Id == purchaseOrderId, cancellationToken);

        if (purchaseOrder != null)
        {
            model.PurchaseOrderId = purchaseOrder.Id;
            model.PONumber = purchaseOrder.PONumber;
            model.VendorId = purchaseOrder.VendorId;
            model.VendorName = purchaseOrder.Vendor.BusinessName;
            model.WarehouseId = purchaseOrder.WarehouseId;
            model.ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate;
            model.ReferenceNumber = purchaseOrder.ReferenceNumber;
            model.InternalNotes = purchaseOrder.InternalNotes;
            model.VendorNotes = purchaseOrder.VendorNotes;

            _logger.LogInformation("Loading PO items for PO {POId}, found {ItemCount} items", purchaseOrderId, purchaseOrder.PurchaseOrderItems?.Count ?? 0);

            decimal subtotal = 0;
            decimal discountAmount = 0;
            decimal taxAmount = 0;

            // Load ALL PO items to allow additional receipts
            foreach (var poItem in purchaseOrder.PurchaseOrderItems)
            {
                _logger.LogInformation("Adding PO item {ItemId}: SKU={SKU}, Description={Description}, QuantityOrdered={QuantityOrdered}, QuantityReceived={QuantityReceived}, QuantityPending={QuantityPending}, UnitCost={UnitCost}",
                    poItem.Id, poItem.SKU, poItem.Description, poItem.QuantityOrdered, poItem.QuantityReceived, poItem.QuantityPending, poItem.UnitCost);

                // Calculate line total based on ordered quantity (for GRN, we show what was ordered)
                var lineSubtotal = poItem.QuantityOrdered * poItem.UnitCost;
                var lineDiscount = lineSubtotal * (poItem.DiscountPercent / 100);
                var afterDiscount = lineSubtotal - lineDiscount;
                var lineTax = afterDiscount * (poItem.TaxPercent / 100);
                var lineTotal = afterDiscount + lineTax;

                _logger.LogInformation("Line calculation: QtyOrdered={QtyOrdered}, UnitCost={UnitCost}, LineSubtotal={LineSubtotal}, LineTotal={LineTotal}",
                    poItem.QuantityOrdered, poItem.UnitCost, lineSubtotal, lineTotal);

                subtotal += lineSubtotal;
                discountAmount += lineDiscount;
                taxAmount += lineTax;

                model.GoodsReceivedNoteItems.Add(new GoodsReceivedNoteItemViewModel
                {
                    PurchaseOrderItemId = poItem.Id,
                    SKU = poItem.SKU,
                    Description = poItem.Description,
                    PaintItemId = poItem.PaintItemId,
                    QuantityOrdered = poItem.QuantityOrdered,
                    QuantityPreviouslyReceived = poItem.QuantityReceived,
                    QuantityRemaining = poItem.QuantityPending,
                    QuantityReceived = poItem.QuantityPending > 0 ? poItem.QuantityPending : poItem.QuantityOrdered, // Default to pending or ordered
                    Unit = string.IsNullOrWhiteSpace(poItem.Unit) ? "Each" : poItem.Unit,
                    UnitCost = poItem.UnitCost,
                    DiscountPercent = poItem.DiscountPercent,
                    DiscountAmount = lineDiscount,
                    TaxPercent = poItem.TaxPercent,
                    TaxAmount = lineTax,
                    LineTotal = lineTotal
                });
            }

            // Set summary totals
            model.Subtotal = subtotal;
            model.DiscountAmount = discountAmount;
            model.TaxAmount = taxAmount;
            model.TotalAmount = subtotal - discountAmount + taxAmount;

            _logger.LogInformation("Loaded {ItemCount} items into GRN model with TotalAmount={TotalAmount}", model.GoodsReceivedNoteItems.Count, model.TotalAmount);
        }
        else
        {
            _logger.LogWarning("Purchase Order {POId} not found", purchaseOrderId);
        }
    }

    private async Task<string> GenerateGRNNumberAsync(CancellationToken cancellationToken)
    {
        var lastGRN = await _context.GoodsReceivedNotes
            .Where(grn => grn.GRNNumber.StartsWith("GRN-"))
            .OrderByDescending(grn => grn.GRNNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastGRN == null)
        {
            return "GRN-0001";
        }

        var lastNumber = lastGRN.GRNNumber.Replace("GRN-", "");
        if (int.TryParse(lastNumber, out int number))
        {
            return $"GRN-{(number + 1):D4}";
        }

        return "GRN-0001";
    }
}
