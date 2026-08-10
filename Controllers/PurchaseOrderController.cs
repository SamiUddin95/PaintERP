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
public class PurchaseOrderController : Controller
{
    private readonly PaintErpDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IInventoryService _inventoryService;
    private readonly IAccountingService _accountingService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<PurchaseOrderController> _logger;

    public PurchaseOrderController(
        PaintErpDbContext context,
        IValidationService validationService,
        IInventoryService inventoryService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService,
        ILogger<PurchaseOrderController> logger)
    {
        _context = context;
        _validationService = validationService;
        _inventoryService = inventoryService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _logger = logger;
    }

    // GET: PurchaseOrder
    public async Task<IActionResult> Index(int? page, int pageSize = 10)
    {
        var pageNumber = page ?? 1;

        var query = _context.PurchaseOrders
            .Include(po => po.Vendor)
            .Include(po => po.Warehouse)
            .OrderByDescending(po => po.OrderDate)
            .Select(po => new PurchaseOrderListItem
            {
                Id = po.Id,
                PONumber = po.PONumber,
                VendorName = po.Vendor.BusinessName,
                OrderDate = po.OrderDate,
                ExpectedDeliveryDate = po.ExpectedDeliveryDate,
                TotalAmount = po.TotalAmount,
                Status = po.Status,
                IsApproved = po.Status == "Approved" || po.Status == "Received" || po.Status == "Partially Received" || po.Status == "Closed",
                IsReceived = po.Status == "Received" || po.Status == "Partially Received" || po.Status == "Closed",
                IsFullyReceived = po.IsFullyReceived
            });

        var purchaseOrders = await PaginatedList<PurchaseOrderListItem>.CreateAsync(query, pageNumber, pageSize);

        return View(purchaseOrders);
    }

    // GET: PurchaseOrder/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.Vendor)
            .Include(po => po.Warehouse)
            .Include(po => po.PurchaseOrderItems)
                .ThenInclude(poi => poi.PaintItem)
            .FirstOrDefaultAsync(po => po.Id == id);

        if (purchaseOrder == null)
        {
            return NotFound();
        }

        return View(purchaseOrder);
    }

    // GET: PurchaseOrder/Create
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

        var model = new PurchaseOrderFormViewModel
        {
            CompanyId = defaultCompanyId, // TODO: Get from user session
            OrderDate = DateTime.UtcNow,
            PONumber = await GeneratePONumber(),
            PurchaseOrderItems = new List<PurchaseOrderItemViewModel> { new PurchaseOrderItemViewModel() },
            Vendors = await _context.Vendors
                .Where(v => v.IsActive)
                .OrderBy(v => v.BusinessName)
                .Select(v => new PurchaseOrderVendorListItem { Id = v.Id, BusinessName = v.BusinessName, VendorId = v.VendorId })
                .ToListAsync(),
            Warehouses = await _context.Warehouses
                .OrderBy(w => w.Name)
                .Select(w => new PurchaseOrderWarehouseListItem { Id = w.Id, Name = w.Name })
                .ToListAsync(),
            PaintItems = await _context.PaintItems
                .OrderBy(p => p.Name)
                .Select(p => new PurchaseOrderPaintItemListItem { Id = p.Id, Name = p.Name, SKU = p.SKU ?? "", UnitCost = p.UnitCost, Unit = p.UnitOfMeasure ?? "" })
                .ToListAsync()
        };

        return View(model);
    }

    // POST: PurchaseOrder/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PurchaseOrderFormViewModel model, CancellationToken cancellationToken)
    {
        try
        {
            // Initialize collections if null (form post doesn't include these)
            model.Vendors ??= new List<PurchaseOrderVendorListItem>();
            model.Warehouses ??= new List<PurchaseOrderWarehouseListItem>();
            model.PaintItems ??= new List<PurchaseOrderPaintItemListItem>();
            model.PurchaseOrderItems ??= new List<PurchaseOrderItemViewModel> { new PurchaseOrderItemViewModel() };

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

            var purchaseOrder = new PurchaseOrder
            {
                CompanyId = defaultCompanyId, // Use default company automatically - no user selection needed
                VendorId = model.VendorId,
                PONumber = model.PONumber,
                OrderDate = model.OrderDate,
                ExpectedDeliveryDate = model.ExpectedDeliveryDate,
                WarehouseId = model.WarehouseId,
                PaymentTerms = model.PaymentTerms,
                ReferenceNumber = model.ReferenceNumber,
                Status = model.Status,
                Subtotal = model.Subtotal,
                DiscountAmount = model.DiscountAmount,
                TaxAmount = model.TaxAmount,
                TotalAmount = model.TotalAmount,
                InternalNotes = model.InternalNotes,
                VendorNotes = model.VendorNotes,
                Buyer = model.Buyer,
                ShippingAddress = model.ShippingAddress,
                IsApproved = model.IsApproved,
                IsCancelled = model.IsCancelled,
                IsFullyReceived = model.IsFullyReceived,
                ShippingCost = model.ShippingCost,
                AmountReceived = model.AmountReceived,
                ApprovedDate = model.IsApproved ? DateTime.UtcNow : null,
                CancelledDate = model.IsCancelled ? DateTime.UtcNow : null
            };

            // Add purchase order items
            foreach (var item in model.PurchaseOrderItems.Where(poi => poi.QuantityOrdered > 0))
            {
                var purchaseOrderItem = new PurchaseOrderItem
                {
                    PurchaseOrderId = purchaseOrder.Id,
                    SKU = item.SKU,
                    Description = item.Description,
                    PaintItemId = item.PaintItemId,
                    QuantityOrdered = item.QuantityOrdered,
                    QuantityReceived = item.QuantityReceived,
                    QuantityPending = item.QuantityOrdered - item.QuantityReceived,
                    Unit = item.Unit,
                    UnitCost = item.UnitCost,
                    DiscountPercent = item.DiscountPercent,
                    DiscountAmount = item.DiscountAmount,
                    TaxPercent = item.TaxPercent,
                    TaxAmount = item.TaxAmount,
                    LineTotal = item.LineTotal,
                    IsFullyReceived = item.IsFullyReceived
                };
                purchaseOrder.PurchaseOrderItems.Add(purchaseOrderItem);
            }

            // Step 2: Business Validation
            var validationResult = await _validationService.ValidatePurchaseOrderAsync(purchaseOrder, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                _notificationService.AddValidationErrors(validationResult);
                await PopulateDropdowns(model);
                return this.WithValidationErrors(validationResult, model);
            }

            // Display warnings if any
            foreach (var warning in validationResult.Warnings)
            {
                this.AddWarningMessage(warning);
            }

            // Step 3: Execute in Transaction (ensures atomicity)
            _logger.LogInformation("Starting transaction for PO creation: PONumber={PONumber}, VendorId={VendorId}, TotalAmount={TotalAmount}",
                purchaseOrder.PONumber, purchaseOrder.VendorId, purchaseOrder.TotalAmount);

            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                try
                {
                    // Set audit fields inside transaction
                    purchaseOrder.CreatedAtUtc = DateTime.UtcNow;
                    purchaseOrder.UpdatedAtUtc = DateTime.UtcNow;
                    purchaseOrder.CreatedBy = User.Identity?.Name ?? "System";
                    purchaseOrder.UpdatedBy = User.Identity?.Name ?? "System";

                    // Save purchase order
                    _context.PurchaseOrders.Add(purchaseOrder);
                    var saveResult = await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Purchase Order {PONumber} saved to database with Id={POId}, SaveChangesAsync returned {SaveResult}",
                        purchaseOrder.PONumber, purchaseOrder.Id, saveResult
                    );

                    // Purchase orders don't immediately affect inventory or accounting
                    // Those happen when items are received or when PO is converted to bill
                    // So we just save the PO for now

                    return TransactionResult.SuccessResult(
                        $"Purchase Order {purchaseOrder.PONumber} created successfully",
                        new { PurchaseOrderId = purchaseOrder.Id }
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating purchase order: {Message}, StackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                    return TransactionResult.FailureResult(
                        "An error occurred while creating the purchase order",
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
                await PopulateDropdowns(model);
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in Create action: {Message}", ex.Message);
            // Initialize collections if null
            model.Vendors ??= new List<PurchaseOrderVendorListItem>();
            model.Warehouses ??= new List<PurchaseOrderWarehouseListItem>();
            model.PaintItems ??= new List<PurchaseOrderPaintItemListItem>();
            model.PurchaseOrderItems ??= new List<PurchaseOrderItemViewModel> { new PurchaseOrderItemViewModel() };
            await PopulateDropdowns(model);
            return this.WithError("An unexpected error occurred. Please try again.", "Index");
        }
    }

    // GET: PurchaseOrder/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.PurchaseOrderItems)
            .FirstOrDefaultAsync(po => po.Id == id);

        if (purchaseOrder == null)
        {
            return NotFound();
        }

        var model = new PurchaseOrderFormViewModel
        {
            Id = purchaseOrder.Id,
            CompanyId = purchaseOrder.CompanyId,
            VendorId = purchaseOrder.VendorId,
            PONumber = purchaseOrder.PONumber,
            OrderDate = purchaseOrder.OrderDate,
            ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate,
            WarehouseId = purchaseOrder.WarehouseId,
            PaymentTerms = purchaseOrder.PaymentTerms,
            ReferenceNumber = purchaseOrder.ReferenceNumber,
            Status = purchaseOrder.Status,
            Subtotal = purchaseOrder.Subtotal,
            DiscountAmount = purchaseOrder.DiscountAmount,
            TaxAmount = purchaseOrder.TaxAmount,
            ShippingCost = purchaseOrder.ShippingCost,
            AmountReceived = purchaseOrder.AmountReceived,
            TotalAmount = purchaseOrder.TotalAmount,
            InternalNotes = purchaseOrder.InternalNotes,
            VendorNotes = purchaseOrder.VendorNotes,
            Buyer = purchaseOrder.Buyer,
            ShippingAddress = purchaseOrder.ShippingAddress,
            IsApproved = purchaseOrder.IsApproved,
            IsCancelled = purchaseOrder.IsCancelled,
            IsFullyReceived = purchaseOrder.IsFullyReceived,
            ApprovedDate = purchaseOrder.ApprovedDate,
            CancelledDate = purchaseOrder.CancelledDate,
            ConvertedBillId = purchaseOrder.ConvertedBillId,
            PurchaseOrderItems = purchaseOrder.PurchaseOrderItems.Select(poi => new PurchaseOrderItemViewModel
            {
                Id = poi.Id,
                PurchaseOrderId = poi.PurchaseOrderId,
                SKU = poi.SKU,
                Description = poi.Description,
                PaintItemId = poi.PaintItemId,
                QuantityOrdered = poi.QuantityOrdered,
                QuantityReceived = poi.QuantityReceived,
                QuantityPending = poi.QuantityPending,
                Unit = poi.Unit,
                UnitCost = poi.UnitCost,
                DiscountPercent = poi.DiscountPercent,
                DiscountAmount = poi.DiscountAmount,
                TaxPercent = poi.TaxPercent,
                TaxAmount = poi.TaxAmount,
                LineTotal = poi.LineTotal,
                IsFullyReceived = poi.IsFullyReceived
            }).ToList()
        };

        await PopulateDropdowns(model);

        return View(model);
    }

    // POST: PurchaseOrder/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PurchaseOrderFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model);
            return View(model);
        }

        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.PurchaseOrderItems)
            .FirstOrDefaultAsync(po => po.Id == model.Id);

        if (purchaseOrder == null)
        {
            return NotFound();
        }

        purchaseOrder.VendorId = model.VendorId;
        purchaseOrder.PONumber = model.PONumber;
        purchaseOrder.OrderDate = model.OrderDate;
        purchaseOrder.ExpectedDeliveryDate = model.ExpectedDeliveryDate;
        purchaseOrder.WarehouseId = model.WarehouseId;
        purchaseOrder.PaymentTerms = model.PaymentTerms;
        purchaseOrder.ReferenceNumber = model.ReferenceNumber;
        purchaseOrder.Status = model.Status;
        purchaseOrder.Subtotal = model.Subtotal;
        purchaseOrder.DiscountAmount = model.DiscountAmount;
        purchaseOrder.TaxAmount = model.TaxAmount;
        purchaseOrder.ShippingCost = model.ShippingCost;
        purchaseOrder.AmountReceived = model.AmountReceived;
        purchaseOrder.TotalAmount = model.TotalAmount;
        purchaseOrder.InternalNotes = model.InternalNotes;
        purchaseOrder.VendorNotes = model.VendorNotes;
        purchaseOrder.Buyer = model.Buyer;
        purchaseOrder.ShippingAddress = model.ShippingAddress;
        purchaseOrder.IsApproved = model.IsApproved;
        purchaseOrder.IsCancelled = model.IsCancelled;
        purchaseOrder.IsFullyReceived = model.IsFullyReceived;
        purchaseOrder.ApprovedDate = model.IsApproved ? DateTime.UtcNow : purchaseOrder.ApprovedDate;
        purchaseOrder.CancelledDate = model.IsCancelled ? DateTime.UtcNow : purchaseOrder.CancelledDate;
        purchaseOrder.UpdatedAtUtc = DateTime.UtcNow;
        purchaseOrder.UpdatedBy = User.Identity?.Name ?? "System";

        // Update purchase order items
        var existingItems = purchaseOrder.PurchaseOrderItems.ToList();
        _context.PurchaseOrderItems.RemoveRange(existingItems);

        foreach (var item in model.PurchaseOrderItems.Where(poi => poi.QuantityOrdered > 0))
        {
            var purchaseOrderItem = new PurchaseOrderItem
            {
                PurchaseOrderId = purchaseOrder.Id,
                SKU = item.SKU,
                Description = item.Description,
                PaintItemId = item.PaintItemId,
                QuantityOrdered = item.QuantityOrdered,
                QuantityReceived = item.QuantityReceived,
                QuantityPending = item.QuantityPending,
                Unit = item.Unit,
                UnitCost = item.UnitCost,
                DiscountPercent = item.DiscountPercent,
                DiscountAmount = item.DiscountAmount,
                TaxPercent = item.TaxPercent,
                TaxAmount = item.TaxAmount,
                LineTotal = item.LineTotal,
                IsFullyReceived = item.IsFullyReceived
            };
            _context.PurchaseOrderItems.Add(purchaseOrderItem);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: PurchaseOrder/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.Vendor)
            .FirstOrDefaultAsync(po => po.Id == id);

        if (purchaseOrder == null)
        {
            return NotFound();
        }

        return View(purchaseOrder);
    }

    // POST: PurchaseOrder/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        try
        {
            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                var purchaseOrder = await _context.PurchaseOrders
                    .Include(po => po.PurchaseOrderItems)
                    .FirstOrDefaultAsync(po => po.Id == id, cancellationToken);

                if (purchaseOrder == null)
                {
                    return TransactionResult.FailureResult("Purchase order not found");
                }

                // Find associated journal entry
                var journalEntry = await _context.JournalEntries
                    .FirstOrDefaultAsync(je => je.TransactionType == "Purchase Order" && je.ReferenceId == id, cancellationToken);

                // Reverse inventory movements if items were received
                if (purchaseOrder.PurchaseOrderItems != null && purchaseOrder.PurchaseOrderItems.Any())
                {
                    foreach (var item in purchaseOrder.PurchaseOrderItems)
                    {
                        if (item.QuantityReceived > 0)
                        {
                            await _inventoryService.ReduceStockAsync(
                                Convert.ToInt32(item.PaintItemId),
                                purchaseOrder.WarehouseId ?? 0,
                                item.QuantityReceived,
                                "Purchase Order Reversal",
                                purchaseOrder.Id,
                                $"Reversal of PO: {purchaseOrder.PONumber}",
                                cancellationToken
                            );
                        }
                    }
                }

                // Reverse journal entry
                if (journalEntry != null)
                {
                    await _accountingService.ReverseJournalEntryAsync(
                        journalEntry.Id,
                        $"Purchase Order {purchaseOrder.PONumber} deleted",
                        cancellationToken
                    );
                }

                // Delete purchase order
                _context.PurchaseOrders.Remove(purchaseOrder);
                await _context.SaveChangesAsync(cancellationToken);

                return TransactionResult.SuccessResult($"Purchase Order {purchaseOrder.PONumber} deleted successfully");
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
            _logger.LogError(ex, "Error deleting purchase order: {Message}", ex.Message);
            return this.WithError("An error occurred while deleting the purchase order", "Index");
        }
    }

    // POST: PurchaseOrder/Approve/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
        if (purchaseOrder != null)
        {
            purchaseOrder.Status = "Approved";
            purchaseOrder.UpdatedAtUtc = DateTime.UtcNow;
            purchaseOrder.UpdatedBy = User.Identity?.Name ?? "System";
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    // POST: PurchaseOrder/Close/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(int id)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
        if (purchaseOrder != null)
        {
            purchaseOrder.Status = "Closed";
            purchaseOrder.UpdatedAtUtc = DateTime.UtcNow;
            purchaseOrder.UpdatedBy = User.Identity?.Name ?? "System";
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    // GET: PurchaseOrder/ReceiveItems/5
    public async Task<IActionResult> ReceiveItems(int id)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.Vendor)
            .Include(po => po.Warehouse)
            .Include(po => po.PurchaseOrderItems)
                .ThenInclude(poi => poi.PaintItem)
            .FirstOrDefaultAsync(po => po.Id == id);

        if (purchaseOrder == null)
        {
            return NotFound();
        }

        // Redirect to GRN Create with PO pre-selected
        return RedirectToAction("Create", "GoodsReceivedNote", new { purchaseOrderId = id });
    }

    private async Task<string> GeneratePONumber()
    {
        var lastPO = await _context.PurchaseOrders
            .OrderByDescending(po => po.Id)
            .FirstOrDefaultAsync();

        if (lastPO == null)
        {
            return "PO-0001";
        }

        var lastNumber = lastPO.PONumber.Replace("PO-", "");
        if (int.TryParse(lastNumber, out int number))
        {
            return $"PO-{(number + 1):D4}";
        }

        return "PO-0001";
    }

    private async Task PopulateDropdowns(PurchaseOrderFormViewModel model)
    {
        model.Vendors = (await _context.Vendors
            .Where(v => v.IsActive)
            .OrderBy(v => v.BusinessName)
            .Select(v => new PurchaseOrderVendorListItem { Id = v.Id, BusinessName = v.BusinessName, VendorId = v.VendorId })
            .ToListAsync()) ?? new List<PurchaseOrderVendorListItem>();

        model.Warehouses = (await _context.Warehouses
            .OrderBy(w => w.Name)
            .Select(w => new PurchaseOrderWarehouseListItem { Id = w.Id, Name = w.Name })
            .ToListAsync()) ?? new List<PurchaseOrderWarehouseListItem>();

        model.PaintItems = (await _context.PaintItems
            .OrderBy(p => p.Name)
            .Select(p => new PurchaseOrderPaintItemListItem { Id = p.Id, Name = p.Name, SKU = p.SKU ?? "", UnitCost = p.UnitCost, Unit = p.UnitOfMeasure ?? "" })
            .ToListAsync()) ?? new List<PurchaseOrderPaintItemListItem>();
    }
}

public class PurchaseOrderListItem
{
    public int Id { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public bool IsReceived { get; set; }
    public bool IsFullyReceived { get; set; }
}
