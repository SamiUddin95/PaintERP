using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;

namespace PaintERP.Controllers;

public class ItemController(PaintErpDbContext context) : Controller
{
    private const string AuthCookie = "PaintErpAuth";
    private const string DemoEmail = "ops@usapainterp.com";

    private bool IsAuthorized() => Request.Cookies.TryGetValue(AuthCookie, out var token) && token == DemoEmail;

    // GET: Item
    public async Task<IActionResult> Index(string searchTerm = "", string filterType = "All", string filterCategory = "All", string filterSource = "All", int? page = 1, int pageSize = 10)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var pageNumber = page ?? 1;

        var query = context.PaintItems.AsQueryable();

        // Apply search
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(i =>
                i.Name.Contains(searchTerm) ||
                i.SKU.Contains(searchTerm) ||
                i.UPCBarcode.Contains(searchTerm) ||
                i.Category.Contains(searchTerm));
        }

        // Apply type filter
        if (filterType != "All")
        {
            query = query.Where(i => i.ItemType == filterType);
        }

        // Apply category filter
        if (filterCategory != "All")
        {
            query = query.Where(i => i.Category == filterCategory);
        }

        // Apply source filter (from Paint Production)
        if (filterSource == "Production")
        {
            query = query.Where(i => i.SourceProductionId.HasValue);
        }
        else if (filterSource == "NonProduction")
        {
            query = query.Where(i => !i.SourceProductionId.HasValue);
        }

        var itemQuery = query
            .OrderBy(i => i.Name)
            .Select(i => new ItemListItem
            {
                Id = i.Id,
                SKU = i.SKU,
                UPCBarcode = i.UPCBarcode,
                Name = i.Name,
                ItemType = i.ItemType,
                Category = i.Category,
                Brand = i.Brand,
                WarehouseName = i.WarehouseName,
                CurrentStock = i.CurrentStock,
                AvailableStock = i.AvailableStock,
                InventoryValue = i.InventoryValue,
                SellingPrice = i.SellingPrice ?? 0,
                ColorHex = i.ColorHex,
                IsHazardousMaterial = i.IsHazardousMaterial,
                UnitOfMeasure = i.UnitOfMeasure
            });

        var items = await PaginatedList<ItemListItem>.CreateAsync(itemQuery, pageNumber, pageSize);

        var viewModel = new ItemListViewModel
        {
            Items = items,
            SearchTerm = searchTerm,
            FilterType = filterType,
            FilterCategory = filterCategory,
            FilterSource = filterSource,
            TotalCount = context.PaintItems.Count(),
            TotalInventoryValue = context.PaintItems.Sum(i => i.InventoryValue),
            LowStockCount = context.PaintItems.Count(i => i.CurrentStock <= i.ReorderPoint)
        };

        return View(viewModel);
    }

    // GET: Item/Create
    public IActionResult Create()
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var model = new ItemFormViewModel
        {
            WarehouseId = 1,
            SKU = GenerateSKU(),
            ItemType = "Inventory",
            UnitOfMeasure = "GAL",
            PurchaseUnit = "GAL",
            SalesUnit = "GAL",
            CostMethod = "Average Cost",
            ColorHex = "#2266CC",
            Vendors = context.Vendors.Select(v => v.BusinessName).ToList(),
            Warehouses = context.Warehouses.ToList()
        };

        return View(model);
    }

    // POST: Item/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ItemFormViewModel model)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        if (!ModelState.IsValid)
        {
            model.Vendors = context.Vendors.Select(v => v.BusinessName).ToList();
            model.Warehouses = context.Warehouses.ToList();
            return View(model);
        }

        try
        {
            var warehouse = context.Warehouses.Find(model.WarehouseId);
            if (warehouse == null)
            {
                ModelState.AddModelError("WarehouseId", "Selected warehouse not found. Please create a warehouse first.");
                model.Vendors = context.Vendors.Select(v => v.BusinessName).ToList();
                model.Warehouses = context.Warehouses.ToList();
                return View(model);
            }

            var item = new PaintItem
            {
                WarehouseId = model.WarehouseId,
                SKU = model.SKU,
                UPCBarcode = model.UPCBarcode,
                Name = model.Name,
                ItemType = model.ItemType,
                Category = model.Category,
                Brand = model.Brand,
                Manufacturer = model.Manufacturer,
                WarehouseName = warehouse.Name,
                DefaultBin = model.DefaultBin,
                UnitOfMeasure = model.UnitOfMeasure,
                PurchaseUnit = model.PurchaseUnit,
                SalesUnit = model.SalesUnit,
                CostMethod = model.CostMethod,
                PurchasePrice = model.PurchasePrice,
                SellingPrice = model.SellingPrice,
                MSRP = model.MSRP,
                MinimumStock = model.MinimumStock,
                MaximumStock = model.MaximumStock,
                ReorderPoint = model.ReorderPoint,
                PreferredVendorId = model.PreferredVendorId,
                SalesTaxCategory = model.SalesTaxCategory,
                InventoryAssetAccount = model.InventoryAssetAccount,
                COGSAccount = model.COGSAccount,
                IncomeAccount = model.IncomeAccount,
                Weight = model.Weight,
                Dimensions = model.Dimensions,
                IsHazardousMaterial = model.IsHazardousMaterial,
                LotTracking = model.LotTracking,
                BatchTracking = model.BatchTracking,
                ExpirationDate = model.ExpirationDate,
                ColorFamily = model.ColorFamily,
                ColorHex = model.ColorHex,
                UnitCost = model.UnitCost,
                StockQuantity = model.StockQuantity,
                ReorderLevel = model.ReorderLevel,
                ImagePath = model.ImagePath,
                AttachmentsPath = model.AttachmentsPath,
                Notes = model.Notes,
                CurrentStock = model.StockQuantity,
                AvailableStock = model.StockQuantity,
                ReservedStock = 0,
                InventoryValue = model.StockQuantity * model.UnitCost,
                LastPurchaseDate = DateTime.UtcNow,
                LastSaleDate = null,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                CreatedBy = DemoEmail,
                UpdatedBy = DemoEmail
            };

            context.PaintItems.Add(item);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"An error occurred while saving the item: {ex.Message}");
            model.Vendors = context.Vendors.Select(v => v.BusinessName).ToList();
            model.Warehouses = context.Warehouses.ToList();
            return View(model);
        }
    }

    // GET: Item/Edit/5
    public IActionResult Edit(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var item = context.PaintItems.Find(id);
        if (item == null)
        {
            return NotFound();
        }

        var model = new ItemFormViewModel
        {
            Id = item.Id,
            WarehouseId = item.WarehouseId,
            SKU = item.SKU,
            UPCBarcode = item.UPCBarcode,
            Name = item.Name,
            ItemType = item.ItemType,
            Category = item.Category,
            Brand = item.Brand,
            Manufacturer = item.Manufacturer,
            WarehouseName = item.WarehouseName,
            DefaultBin = item.DefaultBin,
            UnitOfMeasure = item.UnitOfMeasure,
            PurchaseUnit = item.PurchaseUnit,
            SalesUnit = item.SalesUnit,
            CostMethod = item.CostMethod,
            PurchasePrice = item.PurchasePrice,
            SellingPrice = item.SellingPrice ?? 0,
            MSRP = item.MSRP,
            MinimumStock = item.MinimumStock,
            MaximumStock = item.MaximumStock,
            ReorderPoint = item.ReorderPoint,
            PreferredVendorId = item.PreferredVendorId,
            SalesTaxCategory = item.SalesTaxCategory,
            InventoryAssetAccount = item.InventoryAssetAccount,
            COGSAccount = item.COGSAccount,
            IncomeAccount = item.IncomeAccount,
            Weight = item.Weight,
            Dimensions = item.Dimensions,
            IsHazardousMaterial = item.IsHazardousMaterial,
            LotTracking = item.LotTracking,
            BatchTracking = item.BatchTracking,
            ExpirationDate = item.ExpirationDate,
            ColorFamily = item.ColorFamily,
            ColorHex = item.ColorHex,
            UnitCost = item.UnitCost,
            StockQuantity = item.StockQuantity,
            ReorderLevel = item.ReorderLevel,
            ImagePath = item.ImagePath,
            AttachmentsPath = item.AttachmentsPath,
            Notes = item.Notes,
            CurrentStock = item.CurrentStock,
            AvailableStock = item.AvailableStock,
            ReservedStock = item.ReservedStock,
            InventoryValue = item.InventoryValue,
            LastPurchaseDate = item.LastPurchaseDate,
            LastSaleDate = item.LastSaleDate,
            Vendors = context.Vendors.Select(v => v.BusinessName).ToList(),
            Warehouses = context.Warehouses.ToList()
        };

        return View(model);
    }

    // POST: Item/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ItemFormViewModel model)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        if (!ModelState.IsValid)
        {
            model.Vendors = context.Vendors.Select(v => v.BusinessName).ToList();
            model.Warehouses = context.Warehouses.ToList();
            return View(model);
        }

        try
        {
            var item = context.PaintItems.Find(model.Id);
            if (item == null)
            {
                return NotFound();
            }

            var warehouse = context.Warehouses.Find(model.WarehouseId);
            if (warehouse == null)
            {
                ModelState.AddModelError("WarehouseId", "Selected warehouse not found. Please select a valid warehouse.");
                model.Vendors = context.Vendors.Select(v => v.BusinessName).ToList();
                model.Warehouses = context.Warehouses.ToList();
                return View(model);
            }

            item.WarehouseId = model.WarehouseId;
            item.SKU = model.SKU;
            item.UPCBarcode = model.UPCBarcode;
            item.Name = model.Name;
            item.ItemType = model.ItemType;
            item.Category = model.Category;
            item.Brand = model.Brand;
            item.Manufacturer = model.Manufacturer;
            item.WarehouseName = warehouse.Name;
            item.DefaultBin = model.DefaultBin;
            item.UnitOfMeasure = model.UnitOfMeasure;
            item.PurchaseUnit = model.PurchaseUnit;
            item.SalesUnit = model.SalesUnit;
            item.CostMethod = model.CostMethod;
            item.PurchasePrice = model.PurchasePrice;
            item.SellingPrice = model.SellingPrice;
            item.MSRP = model.MSRP;
            item.MinimumStock = model.MinimumStock;
            item.MaximumStock = model.MaximumStock;
            item.ReorderPoint = model.ReorderPoint;
            item.PreferredVendorId = model.PreferredVendorId;
            item.SalesTaxCategory = model.SalesTaxCategory;
            item.InventoryAssetAccount = model.InventoryAssetAccount;
            item.COGSAccount = model.COGSAccount;
            item.IncomeAccount = model.IncomeAccount;
            item.Weight = model.Weight;
            item.Dimensions = model.Dimensions;
            item.IsHazardousMaterial = model.IsHazardousMaterial;
            item.LotTracking = model.LotTracking;
            item.BatchTracking = model.BatchTracking;
            item.ExpirationDate = model.ExpirationDate;
            item.ColorFamily = model.ColorFamily;
            item.ColorHex = model.ColorHex;
            item.UnitCost = model.UnitCost;
            item.StockQuantity = model.StockQuantity;
            item.ReorderLevel = model.ReorderLevel;
            item.ImagePath = model.ImagePath;
            item.AttachmentsPath = model.AttachmentsPath;
            item.Notes = model.Notes;
            item.CurrentStock = model.StockQuantity;
            item.AvailableStock = model.StockQuantity;
            item.ReservedStock = 0;
            item.InventoryValue = model.StockQuantity * model.UnitCost;
            item.UpdatedAtUtc = DateTime.UtcNow;
            item.UpdatedBy = DemoEmail;

            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"An error occurred while updating the item: {ex.Message}");
            model.Vendors = context.Vendors.Select(v => v.BusinessName).ToList();
            model.Warehouses = context.Warehouses.ToList();
            return View(model);
        }
    }

    // GET: Item/Delete/5
    public IActionResult Delete(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var item = context.PaintItems.Find(id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // POST: Item/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var item = context.PaintItems.Find(id);
        if (item != null)
        {
            context.PaintItems.Remove(item);
            context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

    private string GenerateSKU()
    {
        var prefix = "SKU";
        var year = DateTime.UtcNow.Year.ToString().Substring(2);
        var count = context.PaintItems.Count() + 1;
        return $"{prefix}{year}-{count:D4}";
    }
}
