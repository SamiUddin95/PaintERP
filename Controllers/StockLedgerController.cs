using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;

namespace PaintERP.Controllers;

public class StockLedgerController : Controller
{
    private readonly PaintErpDbContext _context;

    public StockLedgerController(PaintErpDbContext context)
    {
        _context = context;
    }

    // GET: StockLedger
    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, int? warehouseId, int? paintItemId, string? transactionType)
    {
        var query = _context.StockLedgers
            .Include(sl => sl.Warehouse)
            .Include(sl => sl.PaintItem)
            .AsQueryable();

        // Apply filters for the main query
        if (startDate.HasValue)
        {
            query = query.Where(sl => sl.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(sl => sl.TransactionDate <= endDate.Value);
        }

        if (warehouseId.HasValue)
        {
            query = query.Where(sl => sl.WarehouseId == warehouseId.Value);
        }

        if (paintItemId.HasValue)
        {
            query = query.Where(sl => sl.PaintItemId == paintItemId.Value);
        }

        if (!string.IsNullOrEmpty(transactionType))
        {
            query = query.Where(sl => sl.TransactionType == transactionType);
        }

        var entries = await query
            .OrderBy(sl => sl.TransactionDate)
            .ThenBy(sl => sl.Id)
            .ToListAsync();

        // Calculate opening balance (transactions before start date)
        decimal openingBalance = 0;
        if (startDate.HasValue)
        {
            var openingQuery = _context.StockLedgers.AsQueryable();
            
            if (warehouseId.HasValue)
            {
                openingQuery = openingQuery.Where(sl => sl.WarehouseId == warehouseId.Value);
            }
            
            if (paintItemId.HasValue)
            {
                openingQuery = openingQuery.Where(sl => sl.PaintItemId == paintItemId.Value);
            }
            
            openingBalance = await openingQuery
                .Where(sl => sl.TransactionDate < startDate.Value)
                .SumAsync(sl => sl.InQty - sl.OutQty);
        }

        // Recalculate running balance starting from opening balance
        decimal runningBalance = openingBalance;
        foreach (var entry in entries)
        {
            runningBalance += entry.InQty - entry.OutQty;
            entry.RunningBalance = runningBalance;
        }

        // Create opening balance entry
        var openingEntry = new StockLedger
        {
            Id = 0,
            TransactionDate = startDate ?? DateTime.MinValue,
            TransactionType = "Opening Balance",
            ReferenceNumber = "-",
            Description = "Opening Balance",
            InQty = 0,
            OutQty = 0,
            RunningBalance = openingBalance,
            UserName = "-",
            Remarks = "Balance carried forward"
        };

        // Add opening entry at the beginning
        var allEntries = new List<StockLedger> { openingEntry };
        allEntries.AddRange(entries);

        // Sort by date ascending for running balance calculation
        var sortedEntries = allEntries
            .OrderBy(sl => sl.TransactionDate == DateTime.MinValue ? DateTime.MinValue : sl.TransactionDate)
            .ThenBy(sl => sl.Id)
            .ToList();

        // Reverse to show descending (newest first) while keeping opening balance at top
        var displayEntries = new List<StockLedger> { openingEntry };
        displayEntries.AddRange(sortedEntries.Where(sl => sl.TransactionType != "Opening Balance").Reverse());

        // Populate dropdown options
        ViewBag.Warehouses = await _context.Warehouses
            .OrderBy(w => w.Name)
            .Select(w => new { w.Id, w.Name })
            .ToListAsync();

        ViewBag.PaintItems = await _context.PaintItems
            .OrderBy(p => p.Name)
            .Select(p => new { p.Id, p.Name })
            .ToListAsync();

        ViewBag.TransactionTypes = new List<string>
        {
            "Purchase",
            "Sale",
            "Adjustment",
            "Transfer In",
            "Transfer Out",
            "Production",
            "Purchase Reversal"
        };

        // Set filter values for view
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        ViewBag.WarehouseId = warehouseId;
        ViewBag.PaintItemId = paintItemId;
        ViewBag.TransactionType = transactionType;

        return View(sortedEntries);
    }

    // GET: StockLedger/ExportCSV
    public async Task<IActionResult> ExportCSV(DateTime? startDate, DateTime? endDate, int? warehouseId, int? paintItemId, string? transactionType)
    {
        var query = _context.StockLedgers
            .Include(sl => sl.Warehouse)
            .Include(sl => sl.PaintItem)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(sl => sl.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(sl => sl.TransactionDate <= endDate.Value);
        }

        if (warehouseId.HasValue)
        {
            query = query.Where(sl => sl.WarehouseId == warehouseId.Value);
        }

        if (paintItemId.HasValue)
        {
            query = query.Where(sl => sl.PaintItemId == paintItemId.Value);
        }

        if (!string.IsNullOrEmpty(transactionType))
        {
            query = query.Where(sl => sl.TransactionType == transactionType);
        }

        var entries = await query
            .OrderByDescending(sl => sl.TransactionDate)
            .ThenBy(sl => sl.Id)
            .ToListAsync();

        var csv = "Date,Transaction,Reference,Item,SKU,IN Qty,OUT Qty,Balance,User,Remarks\n";

        foreach (var entry in entries)
        {
            var itemSku = entry.PaintItem?.SKU ?? "";
            var itemName = entry.PaintItem?.Name ?? "";
            csv += $"{entry.TransactionDate:MM/dd/yyyy HH:mm},{entry.TransactionType},{entry.ReferenceNumber},{itemName},{itemSku},{entry.InQty},{entry.OutQty},{entry.RunningBalance},{entry.UserName},{entry.Remarks ?? ""}\n";
        }

        var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
        return File(bytes, "text/csv", "stock_ledger.csv");
    }
}
