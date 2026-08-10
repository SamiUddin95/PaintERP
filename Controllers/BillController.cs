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
public class BillController : Controller
{
    private readonly PaintErpDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IInventoryService _inventoryService;
    private readonly IAccountingService _accountingService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<BillController> _logger;

    public BillController(
        PaintErpDbContext context,
        IValidationService validationService,
        IInventoryService inventoryService,
        IAccountingService accountingService,
        ITransactionService transactionService,
        INotificationService notificationService,
        ILogger<BillController> logger)
    {
        _context = context;
        _validationService = validationService;
        _inventoryService = inventoryService;
        _accountingService = accountingService;
        _transactionService = transactionService;
        _notificationService = notificationService;
        _logger = logger;
    }

    // GET: Bill
    public async Task<IActionResult> Index(int? page, int pageSize = 10)
    {
        var pageNumber = page ?? 1;

        var query = _context.Bills
            .Include(b => b.Vendor)
            .Include(b => b.Warehouse)
            .OrderByDescending(b => b.BillDate)
            .Select(b => new BillListItem
            {
                Id = b.Id,
                BillNumber = b.BillNumber,
                VendorName = b.Vendor.BusinessName,
                BillDate = b.BillDate,
                DueDate = b.DueDate,
                GrandTotal = b.GrandTotal,
                BalanceDue = b.BalanceDue,
                Status = b.Status,
                IsPaid = b.IsPaid
            });

        var bills = await PaginatedList<BillListItem>.CreateAsync(query, pageNumber, pageSize);

        return View(bills);
    }

    // GET: Bill/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var bill = await _context.Bills
            .Include(b => b.Vendor)
            .Include(b => b.Warehouse)
            .Include(b => b.PurchaseOrder)
            .Include(b => b.BillItems)
                .ThenInclude(bi => bi.PaintItem)
            .Include(b => b.BillItems)
                .ThenInclude(bi => bi.Warehouse)
            .Include(b => b.BillPayments)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bill == null)
        {
            return NotFound();
        }

        return View(bill);
    }

    // GET: Bill/Create
    public async Task<IActionResult> Create(int? grnId)
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

        var model = new BillFormViewModel
        {
            CompanyId = defaultCompanyId,
            BillDate = DateTime.UtcNow,
            BillNumber = await GenerateBillNumber(),
            BillItems = new List<BillItemViewModel> { new BillItemViewModel() },
            Vendors = await _context.Vendors
                .Where(v => v.IsActive)
                .OrderBy(v => v.BusinessName)
                .Select(v => new BillVendorListItem { Id = v.Id, BusinessName = v.BusinessName, VendorId = v.VendorId })
                .ToListAsync(),
            Warehouses = await _context.Warehouses
                .OrderBy(w => w.Name)
                .Select(w => new BillWarehouseListItem { Id = w.Id, Name = w.Name })
                .ToListAsync(),
            PurchaseOrders = await _context.PurchaseOrders
                .Where(po => po.Status == "Open")
                .OrderByDescending(po => po.OrderDate)
                .Select(po => new BillPurchaseOrderListItem { Id = po.Id, PONumber = $"PO-{po.Id}", OrderDate = po.OrderDate, TotalAmount = po.TotalAmount })
                .ToListAsync(),
            PaintItems = await _context.PaintItems
                .OrderBy(p => p.Name)
                .Select(p => new BillPaintItemListItem { Id = p.Id, Name = p.Name, SKU = p.SKU ?? "", UnitCost = p.UnitCost, Unit = p.UnitOfMeasure ?? "" })
                .ToListAsync()
        };

        // If creating from GRN, load GRN data
        if (grnId.HasValue)
        {
            var grn = await _context.GoodsReceivedNotes
                .Include(g => g.PurchaseOrder)
                .Include(g => g.GoodsReceivedNoteItems)
                .FirstOrDefaultAsync(g => g.Id == grnId.Value);

            if (grn != null)
            {
                model.VendorId = grn.VendorId;
                model.WarehouseId = grn.WarehouseId;
                model.PurchaseOrderId = grn.PurchaseOrderId;
                model.ReferenceNumber = grn.ReferenceNumber;
                model.BillDate = grn.GRNDate;
                model.Subtotal = grn.Subtotal;
                model.DiscountAmount = grn.DiscountAmount;
                model.TaxAmount = grn.TaxAmount;
                model.GrandTotal = grn.TotalAmount;
                model.InternalNotes = grn.InternalNotes;
                model.VendorNotes = grn.VendorNotes;

                // Load GRN items as bill items
                model.BillItems = grn.GoodsReceivedNoteItems.Select(gi => new BillItemViewModel
                {
                    SKU = gi.SKU,
                    Description = gi.Description,
                    PaintItemId = gi.PaintItemId,
                    Quantity = gi.QuantityReceived,
                    Unit = gi.Unit,
                    UnitCost = gi.UnitCost,
                    DiscountPercent = gi.DiscountPercent,
                    DiscountAmount = gi.DiscountAmount,
                    TaxPercent = gi.TaxPercent,
                    TaxAmount = gi.TaxAmount,
                    LineTotal = gi.LineTotal,
                    WarehouseId = grn.WarehouseId
                }).ToList();

                // Store GRN ID for linking
                ViewData["GRNId"] = grnId.Value;
                ViewData["GRNNumber"] = grn.GRNNumber;

                // Reload POs to include the selected PO even if not open
                if (model.PurchaseOrderId.HasValue)
                {
                    model.PurchaseOrders = await _context.PurchaseOrders
                        .Where(po => po.Status == "Open" || po.Id == model.PurchaseOrderId.Value)
                        .OrderByDescending(po => po.OrderDate)
                        .Select(po => new BillPurchaseOrderListItem { Id = po.Id, PONumber = $"PO-{po.Id}", OrderDate = po.OrderDate, TotalAmount = po.TotalAmount })
                        .ToListAsync();
                }
            }
        }

        return View(model);
    }

    // GET: Bill/CreateFromPO/5
    public async Task<IActionResult> CreateFromPO(int purchaseOrderId)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.Vendor)
            .Include(po => po.Warehouse)
            .Include(po => po.PurchaseOrderItems)
                .ThenInclude(poi => poi.PaintItem)
            .FirstOrDefaultAsync(po => po.Id == purchaseOrderId);

        if (purchaseOrder == null)
        {
            return NotFound();
        }

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

        var model = new BillFormViewModel
        {
            CompanyId = defaultCompanyId, // Use default company automatically - no user selection needed
            PurchaseOrderId = purchaseOrder.Id,
            VendorId = purchaseOrder.VendorId,
            WarehouseId = purchaseOrder.WarehouseId,
            BillDate = DateTime.UtcNow,
            DueDate = purchaseOrder.ExpectedDeliveryDate,
            BillNumber = await GenerateBillNumber(),
            VendorInvoiceNumber = purchaseOrder.PONumber,
            PaymentTerms = purchaseOrder.PaymentTerms,
            ReferenceNumber = purchaseOrder.ReferenceNumber,
            ShippingMethod = "Ground",
            Subtotal = purchaseOrder.Subtotal,
            DiscountAmount = purchaseOrder.DiscountAmount,
            TaxAmount = purchaseOrder.TaxAmount,
            ShippingCharges = purchaseOrder.ShippingCost,
            GrandTotal = purchaseOrder.TotalAmount,
            BillItems = purchaseOrder.PurchaseOrderItems.Select(poi => new BillItemViewModel
            {
                SKU = poi.SKU,
                Description = poi.Description,
                PaintItemId = poi.PaintItemId,
                Quantity = poi.QuantityOrdered,
                Unit = poi.Unit,
                UnitCost = poi.UnitCost,
                DiscountPercent = poi.DiscountPercent,
                DiscountAmount = poi.DiscountAmount,
                TaxPercent = poi.TaxPercent,
                TaxAmount = poi.TaxAmount,
                LineTotal = poi.LineTotal,
                WarehouseId = purchaseOrder.WarehouseId
            }).ToList(),
            Vendors = await _context.Vendors
                .Where(v => v.IsActive)
                .OrderBy(v => v.BusinessName)
                .Select(v => new BillVendorListItem { Id = v.Id, BusinessName = v.BusinessName, VendorId = v.VendorId })
                .ToListAsync(),
            Warehouses = await _context.Warehouses
                .OrderBy(w => w.Name)
                .Select(w => new BillWarehouseListItem { Id = w.Id, Name = w.Name })
                .ToListAsync(),
            PurchaseOrders = await _context.PurchaseOrders
                .Where(po => po.Status == "Open" || po.Id == purchaseOrderId)
                .OrderByDescending(po => po.OrderDate)
                .Select(po => new BillPurchaseOrderListItem { Id = po.Id, PONumber = $"PO-{po.Id}", OrderDate = po.OrderDate, TotalAmount = po.TotalAmount })
                .ToListAsync(),
            PaintItems = await _context.PaintItems
                .OrderBy(p => p.Name)
                .Select(p => new BillPaintItemListItem { Id = p.Id, Name = p.Name, SKU = p.SKU ?? "", UnitCost = p.UnitCost, Unit = p.UnitOfMeasure ?? "" })
                .ToListAsync()
        };

        return View("Create", model);
    }

    // POST: Bill/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BillFormViewModel model, CancellationToken cancellationToken)
    {
        try
        {
            // Initialize collections if null (form post doesn't include these)
            model.Vendors ??= new List<BillVendorListItem>();
            model.Warehouses ??= new List<BillWarehouseListItem>();
            model.PurchaseOrders ??= new List<BillPurchaseOrderListItem>();
            model.PaintItems ??= new List<BillPaintItemListItem>();
            model.BillItems ??= new List<BillItemViewModel> { new BillItemViewModel() };

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

            var bill = new Bill
            {
                CompanyId = defaultCompanyId, // Use default company automatically - no user selection needed
                VendorId = model.VendorId,
                BillNumber = model.BillNumber,
                VendorInvoiceNumber = model.VendorInvoiceNumber,
                PurchaseOrderId = model.PurchaseOrderId,
                BillDate = model.BillDate,
                DueDate = model.DueDate,
                WarehouseId = model.WarehouseId,
                PaymentTerms = model.PaymentTerms,
                ShippingMethod = model.ShippingMethod,
                ReferenceNumber = model.ReferenceNumber,
                Currency = model.Currency,
                TaxCode = model.TaxCode,
                Status = model.Status,
                IsApproved = model.IsApproved,
                IsVoid = model.IsVoid,
                IsPaid = model.IsPaid,
                Subtotal = model.Subtotal,
                DiscountAmount = model.DiscountAmount,
                ShippingCharges = model.ShippingCharges,
                TaxAmount = model.TaxAmount,
                OtherCharges = model.OtherCharges,
                GrandTotal = model.GrandTotal,
                BalanceDue = model.BalanceDue,
                AttachmentPath = model.AttachmentPath,
                InternalNotes = model.InternalNotes,
                VendorNotes = model.VendorNotes,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name ?? "System",
                UpdatedBy = User.Identity?.Name ?? "System"
            };

            // Link to GRN if creating from GRN
            int? grnId = ViewData["GRNId"] as int?;
            if (grnId.HasValue)
            {
                bill.GRNId = grnId.Value;
            }

            // Add bill items
            foreach (var item in model.BillItems.Where(bi => bi.Quantity > 0))
            {
                bill.BillItems.Add(new BillItem
                {
                    SKU = item.SKU,
                    Description = item.Description,
                    PaintItemId = item.PaintItemId,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    UnitCost = item.UnitCost,
                    DiscountPercent = item.DiscountPercent,
                    DiscountAmount = item.DiscountAmount,
                    TaxPercent = item.TaxPercent,
                    TaxAmount = item.TaxAmount,
                    LineTotal = item.LineTotal
                });
            }

            _logger.LogInformation("Bill before validation: VendorId={VendorId}, BillNumber={BillNumber}, BillDate={BillDate}, GrandTotal={GrandTotal}, BillItemsCount={BillItemsCount}",
                bill.VendorId, bill.BillNumber, bill.BillDate, bill.GrandTotal, bill.BillItems.Count);

            // Step 2: Business Validation
            var validationResult = await _validationService.ValidateBillAsync(bill, cancellationToken);

            _logger.LogInformation("Bill validation result: IsValid={IsValid}, ErrorCount={ErrorCount}, Errors={Errors}",
                validationResult.IsValid, validationResult.Errors.Count, string.Join("; ", validationResult.Errors));

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Bill validation failed. Errors: {Errors}", string.Join("; ", validationResult.Errors));
                _notificationService.AddValidationErrors(validationResult);
                await PopulateDropdowns(model);

                // Add errors to ModelState explicitly
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return this.WithValidationErrors(validationResult, model);
            }

            // Display warnings if any
            foreach (var warning in validationResult.Warnings)
            {
                this.AddWarningMessage(warning);
            }

            // Step 3: Execute in Transaction (ensures atomicity)
            _logger.LogInformation("Starting transaction for bill creation: BillNumber={BillNumber}, VendorId={VendorId}, GrandTotal={GrandTotal}",
                bill.BillNumber, bill.VendorId, bill.GrandTotal);

            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                try
                {
                    // Set audit fields inside transaction
                    bill.CreatedAtUtc = DateTime.UtcNow;
                    bill.UpdatedAtUtc = DateTime.UtcNow;
                    bill.CreatedBy = User.Identity?.Name ?? "System";
                    bill.UpdatedBy = User.Identity?.Name ?? "System";

                    // Save bill
                    _context.Bills.Add(bill);
                    var saveResult = await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Bill {BillNumber} saved to database with Id={BillId}, SaveChangesAsync returned {SaveResult}",
                        bill.BillNumber, bill.Id, saveResult
                    );

                    // If created from GRN, update GRN to mark as converted
                    if (bill.GRNId.HasValue)
                    {
                        var grn = await _context.GoodsReceivedNotes
                            .FirstOrDefaultAsync(g => g.Id == bill.GRNId.Value, cancellationToken);

                        if (grn != null)
                        {
                            grn.ConvertedBillId = bill.Id;
                            grn.UpdatedAtUtc = DateTime.UtcNow;
                            grn.UpdatedBy = User.Identity?.Name ?? "System";
                            await _context.SaveChangesAsync(cancellationToken);

                            _logger.LogInformation(
                                "GRN {GRNId} marked as converted to Bill {BillId}",
                                grn.Id, bill.Id
                            );
                        }
                    }

                    // Bills don't immediately affect inventory or accounting
                    // Those happen when bill is paid or when inventory is received from PO
                    // So we just save the bill for now

                    return TransactionResult.SuccessResult(
                        $"Bill {bill.BillNumber} created successfully",
                        new { BillId = bill.Id }
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating bill: {Message}, StackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                    return TransactionResult.FailureResult(
                        "An error occurred while creating the bill",
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
            await PopulateDropdowns(model);
            return this.WithError("An unexpected error occurred. Please try again.", "Index");
        }
    }

    // GET: Bill/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var bill = await _context.Bills
            .Include(b => b.BillItems)
            .Include(b => b.PurchaseOrder)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bill == null)
        {
            return NotFound();
        }

        var model = new BillFormViewModel
        {
            Id = bill.Id,
            CompanyId = bill.CompanyId,
            VendorId = bill.VendorId,
            BillNumber = bill.BillNumber,
            VendorInvoiceNumber = bill.VendorInvoiceNumber,
            PurchaseOrderId = bill.PurchaseOrderId,
            BillDate = bill.BillDate,
            DueDate = bill.DueDate,
            WarehouseId = bill.WarehouseId,
            PaymentTerms = bill.PaymentTerms,
            ShippingMethod = bill.ShippingMethod,
            ReferenceNumber = bill.ReferenceNumber,
            Currency = bill.Currency,
            TaxCode = bill.TaxCode,
            Status = bill.Status,
            IsApproved = bill.IsApproved,
            IsVoid = bill.IsVoid,
            IsPaid = bill.IsPaid,
            Subtotal = bill.Subtotal,
            DiscountAmount = bill.DiscountAmount,
            ShippingCharges = bill.ShippingCharges,
            TaxAmount = bill.TaxAmount,
            OtherCharges = bill.OtherCharges,
            GrandTotal = bill.GrandTotal,
            BalanceDue = bill.BalanceDue,
            AttachmentPath = bill.AttachmentPath,
            InternalNotes = bill.InternalNotes,
            VendorNotes = bill.VendorNotes,
            BillItems = bill.BillItems.Select(bi => new BillItemViewModel
            {
                Id = bi.Id,
                BillId = bi.BillId,
                SKU = bi.SKU,
                Description = bi.Description,
                WarehouseId = bi.WarehouseId,
                PaintItemId = bi.PaintItemId,
                Quantity = bi.Quantity,
                Unit = bi.Unit,
                UnitCost = bi.UnitCost,
                DiscountPercent = bi.DiscountPercent,
                DiscountAmount = bi.DiscountAmount,
                TaxPercent = bi.TaxPercent,
                TaxAmount = bi.TaxAmount,
                LineTotal = bi.LineTotal
            }).ToList()
        };

        await PopulateDropdowns(model, bill.PurchaseOrderId);
        await LoadVendorData(model);

        return View(model);
    }

    // POST: Bill/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BillFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model, model.PurchaseOrderId);
            await LoadVendorData(model);
            return View(model);
        }

        var bill = await _context.Bills
            .Include(b => b.BillItems)
            .FirstOrDefaultAsync(b => b.Id == model.Id);

        if (bill == null)
        {
            return NotFound();
        }

        bill.VendorId = model.VendorId;
        bill.BillNumber = model.BillNumber;
        bill.VendorInvoiceNumber = model.VendorInvoiceNumber;
        bill.PurchaseOrderId = model.PurchaseOrderId;
        bill.BillDate = model.BillDate;
        bill.DueDate = model.DueDate;
        bill.WarehouseId = model.WarehouseId;
        bill.PaymentTerms = model.PaymentTerms;
        bill.ShippingMethod = model.ShippingMethod;
        bill.ReferenceNumber = model.ReferenceNumber;
        bill.Currency = model.Currency;
        bill.TaxCode = model.TaxCode;
        bill.Status = "Draft"; // Save button sets status to Draft
        bill.IsApproved = false;
        bill.IsVoid = false;
        bill.IsPaid = model.IsPaid;
        bill.Subtotal = model.Subtotal;
        bill.DiscountAmount = model.DiscountAmount;
        bill.ShippingCharges = model.ShippingCharges;
        bill.TaxAmount = model.TaxAmount;
        bill.OtherCharges = model.OtherCharges;
        bill.GrandTotal = model.GrandTotal;
        bill.BalanceDue = model.BalanceDue;
        bill.AttachmentPath = model.AttachmentPath;
        bill.InternalNotes = model.InternalNotes;
        bill.VendorNotes = model.VendorNotes;
        bill.UpdatedAtUtc = DateTime.UtcNow;
        bill.UpdatedBy = User.Identity?.Name ?? "System";

        // Update bill items
        var existingItems = bill.BillItems.ToList();
        _context.BillItems.RemoveRange(existingItems);

        foreach (var item in model.BillItems.Where(bi => bi.Quantity > 0))
        {
            var billItem = new BillItem
            {
                BillId = bill.Id,
                SKU = item.SKU,
                Description = item.Description,
                WarehouseId = item.WarehouseId,
                PaintItemId = item.PaintItemId,
                Quantity = item.Quantity,
                Unit = item.Unit,
                UnitCost = item.UnitCost,
                DiscountPercent = item.DiscountPercent,
                DiscountAmount = item.DiscountAmount,
                TaxPercent = item.TaxPercent,
                TaxAmount = item.TaxAmount,
                LineTotal = item.LineTotal
            };
            _context.BillItems.Add(billItem);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Bill/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var bill = await _context.Bills
            .Include(b => b.Vendor)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bill == null)
        {
            return NotFound();
        }

        return View(bill);
    }

    // POST: Bill/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        try
        {
            var transactionResult = await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                var bill = await _context.Bills
                    .Include(b => b.BillItems)
                    .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

                if (bill == null)
                {
                    return TransactionResult.FailureResult("Bill not found");
                }

                // Find associated journal entry
                var journalEntry = await _context.JournalEntries
                    .FirstOrDefaultAsync(je => je.TransactionType == "Bill" && je.ReferenceId == id, cancellationToken);

                // Reverse inventory movements if bill affected inventory
                if (bill.BillItems != null && bill.BillItems.Any())
                {
                    foreach (var item in bill.BillItems)
                    {
                        if (item.PaintItemId.HasValue && item.Quantity > 0)
                        {
                            await _inventoryService.ReduceStockAsync(
                                Convert.ToInt32(item.PaintItemId),
                                bill.WarehouseId ?? 0,
                                item.Quantity,
                                "Bill Reversal",
                                bill.Id,
                                $"Reversal of Bill: {bill.BillNumber}",
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
                        $"Bill {bill.BillNumber} deleted",
                        cancellationToken
                    );
                }

                // Delete bill
                _context.Bills.Remove(bill);
                await _context.SaveChangesAsync(cancellationToken);

                return TransactionResult.SuccessResult($"Bill {bill.BillNumber} deleted successfully");
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
            _logger.LogError(ex, "Error deleting bill: {Message}", ex.Message);
            return this.WithError("An error occurred while deleting the bill", "Index");
        }
    }

    // POST: Bill/Approve/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill != null)
        {
            bill.IsApproved = true;
            bill.Status = "Approved";
            bill.UpdatedAtUtc = DateTime.UtcNow;
            bill.UpdatedBy = User.Identity?.Name ?? "System";
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    // POST: Bill/Void/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Void(int id)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill != null)
        {
            bill.IsVoid = true;
            bill.Status = "Void";
            bill.UpdatedAtUtc = DateTime.UtcNow;
            bill.UpdatedBy = User.Identity?.Name ?? "System";
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    private async Task<string> GenerateBillNumber()
    {
        var lastBill = await _context.Bills
            .OrderByDescending(b => b.Id)
            .FirstOrDefaultAsync();

        if (lastBill == null)
        {
            return "BILL-0001";
        }

        var lastNumber = lastBill.BillNumber.Replace("BILL-", "");
        if (int.TryParse(lastNumber, out int number))
        {
            return $"BILL-{(number + 1):D4}";
        }

        return "BILL-0001";
    }

    private async Task PopulateDropdowns(BillFormViewModel model, int? currentPurchaseOrderId = null)
    {
        model.Vendors = (await _context.Vendors
            .Where(v => v.IsActive)
            .OrderBy(v => v.BusinessName)
            .Select(v => new BillVendorListItem { Id = v.Id, BusinessName = v.BusinessName, VendorId = v.VendorId })
            .ToListAsync()) ?? new List<BillVendorListItem>();

        model.Warehouses = (await _context.Warehouses
            .OrderBy(w => w.Name)
            .Select(w => new BillWarehouseListItem { Id = w.Id, Name = w.Name })
            .ToListAsync()) ?? new List<BillWarehouseListItem>();

        // Get all POs for edit - show all so user can see the current PO even if not open
        var purchaseOrders = await _context.PurchaseOrders
            .OrderByDescending(po => po.OrderDate)
            .Select(po => new BillPurchaseOrderListItem { Id = po.Id, PONumber = $"PO-{po.Id}", OrderDate = po.OrderDate, TotalAmount = po.TotalAmount })
            .ToListAsync();

        model.PurchaseOrders = purchaseOrders ?? new List<BillPurchaseOrderListItem>();

        model.PaintItems = (await _context.PaintItems
            .OrderBy(p => p.Name)
            .Select(p => new BillPaintItemListItem { Id = p.Id, Name = p.Name, SKU = p.SKU ?? "", UnitCost = p.UnitCost, Unit = p.UnitOfMeasure ?? "" })
            .ToListAsync()) ?? new List<BillPaintItemListItem>();
    }

    private async Task LoadVendorData(BillFormViewModel model)
    {
        if (model.VendorId > 0)
        {
            var vendor = await _context.Vendors.FindAsync(model.VendorId);
            if (vendor != null)
            {
                model.VendorBalance = vendor.OutstandingAmount;
                model.LastPurchaseDate = vendor.LastPurchaseDate;
                model.VendorCredit = vendor.CreditLimit;

                // Load payment history
                model.PaymentHistory = await _context.BillPayments
                    .Where(bp => bp.Bill.VendorId == model.VendorId)
                    .OrderByDescending(bp => bp.PaymentDate)
                    .Take(10)
                    .Select(bp => new PaymentHistoryViewModel
                    {
                        Id = bp.Id,
                        PaymentDate = bp.PaymentDate,
                        PaymentNumber = bp.PaymentNumber,
                        PaymentMethod = bp.PaymentMethod,
                        PaymentAmount = bp.PaymentAmount,
                        BalanceAfter = bp.Bill.BalanceDue
                    })
                    .ToListAsync();
            }
        }
    }
}

public class BillListItem
{
    public int Id { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public DateTime BillDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal BalanceDue { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
}
