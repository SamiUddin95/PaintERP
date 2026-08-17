using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.ViewModels;

namespace PaintERP.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> BuildExecutiveDashboardAsync(CancellationToken cancellationToken = default);
}

public class DashboardService(PaintErpDbContext context) : IDashboardService
{
    public async Task<DashboardViewModel> BuildExecutiveDashboardAsync(CancellationToken cancellationToken = default)
    {
        var company = await context.Companies.AsNoTracking().FirstAsync(cancellationToken);

        // Calculate KPIs from actual data
        var revenue = await context.SalesInvoices.SumAsync(i => i.GrandTotal, cancellationToken);
        var grossProfit = await context.SalesInvoices.SumAsync(i => i.GrandTotal - i.SalesTaxAmount - i.Subtotal * 0.58m, cancellationToken);
        var receivables = await context.SalesInvoices.Where(i => i.Status != "Paid").SumAsync(i => i.GrandTotal, cancellationToken);
        var payables = await context.Bills.Where(b => b.Status != "Paid").SumAsync(b => b.GrandTotal, cancellationToken);
        var inventoryValue = await context.PaintItems.SumAsync(p => p.InventoryValue, cancellationToken);
        var paintItemCount = await context.PaintItems.CountAsync(cancellationToken);
        var inventoryTurnover = paintItemCount == 0 ? 0 : Math.Round((double)(inventoryValue / paintItemCount), 2);
        var todaysSales = await context.SalesInvoices.Where(i => i.InvoiceDate.Date == DateTime.UtcNow.Date)
            .SumAsync(i => i.GrandTotal, cancellationToken);
        var todaysPurchases = await context.PurchaseOrders.Where(o => o.OrderDate.Date == DateTime.UtcNow.Date)
            .SumAsync(o => o.TotalAmount, cancellationToken);
        var productionToday = await context.PaintProductions.Where(p => p.ProductionDate.Date == DateTime.UtcNow.Date)
            .SumAsync(p => p.OutputQuantity, cancellationToken);
        var openPurchaseOrders = await context.PurchaseOrders.CountAsync(p => p.Status == "Open" || p.Status == "Pending", cancellationToken);
        var openInvoices = await context.SalesInvoices.CountAsync(i => i.Status == "Overdue", cancellationToken);
        var lowStockAlerts = await context.PaintItems.CountAsync(p => p.CurrentStock <= p.MinimumStock, cancellationToken);

        var kpis = new List<KpiCard>
        {
            new() { Title = "Total Revenue", Value = revenue.ToString("C0"), TrendLabel = "MTD", TrendValue = revenue.ToString("C0"), AccentColor = "#0A5C9E" },
            new() { Title = "Gross Profit", Value = grossProfit.ToString("C0"), TrendLabel = "MTD", TrendValue = grossProfit.ToString("C0"), AccentColor = "#0E9F9F" },
            new() { Title = "Accounts Receivable", Value = receivables.ToString("C0"), TrendLabel = "Outstanding", TrendValue = "AR Balance", AccentColor = "#1B86E5" },
            new() { Title = "Accounts Payable", Value = payables.ToString("C0"), TrendLabel = "Outstanding", TrendValue = "AP Balance", AccentColor = "#0E9F9F" },
            new() { Title = "Inventory Value", Value = inventoryValue.ToString("C0"), TrendLabel = "Warehouses", TrendValue = context.Warehouses.Count().ToString(), AccentColor = "#0A5C9E" },
            new() { Title = "Inventory Turnover", Value = inventoryTurnover.ToString("0.0"), TrendLabel = "Per Item", TrendValue = "Avg", AccentColor = "#1B86E5" },
            new() { Title = "Today's Sales", Value = todaysSales.ToString("C0"), TrendLabel = "vs Avg", TrendValue = todaysSales == 0 ? "-100.0%" : "+0.0%", AccentColor = "#0E9F9F" },
            new() { Title = "Today's Purchases", Value = todaysPurchases.ToString("C0"), TrendLabel = "Orders", TrendValue = "Today", AccentColor = "#1B86E5" },
            new() { Title = "Production Today", Value = productionToday.ToString("N0") + " units", TrendLabel = "Completed", TrendValue = "Today", AccentColor = "#0A5C9E" },
            new() { Title = "Open Purchase Orders", Value = openPurchaseOrders.ToString(), TrendLabel = "Pending", TrendValue = openPurchaseOrders.ToString(), AccentColor = "#0E9F9F" },
            new() { Title = "Open Invoices", Value = openInvoices.ToString(), TrendLabel = "Overdue", TrendValue = openInvoices.ToString(), AccentColor = "#1B86E5" },
            new() { Title = "Low Stock Alerts", Value = lowStockAlerts.ToString(), TrendLabel = "Below Min", TrendValue = lowStockAlerts > 0 ? "Action" : "Good", AccentColor = "#0A5C9E" }
        };

        // Get monthly data for charts
        var months = Enumerable.Range(0, 6).Select(i => DateTime.UtcNow.AddMonths(-i)).Reverse().ToList();
        var monthlySales = months.Select(m => context.SalesInvoices
            .Where(i => i.InvoiceDate.Year == m.Year && i.InvoiceDate.Month == m.Month)
            .Sum(i => i.GrandTotal)).ToList();
        var monthlyPurchases = months.Select(m => context.PurchaseOrders
            .Where(p => p.OrderDate.Year == m.Year && p.OrderDate.Month == m.Month)
            .Sum(p => p.TotalAmount)).ToList();
        var monthlyProfit = months.Select(m => context.SalesInvoices
            .Where(i => i.InvoiceDate.Year == m.Year && i.InvoiceDate.Month == m.Month)
            .Sum(i => (i.GrandTotal - i.SalesTaxAmount) * 0.42m)).ToList();

        var charts = new List<ChartCard>
        {
            new() { Title = "Monthly Sales", ChartType = "area", Labels = months.Select(m => m.ToString("MMM")), Values = monthlySales, AccentColor = "#0A5C9E", Subtitle = "US Dollars" },
            new() { Title = "Monthly Purchases", ChartType = "bar", Labels = months.Select(m => m.ToString("MMM")), Values = monthlyPurchases, AccentColor = "#0E9F9F", Subtitle = "Vendor Spend" },
            new() { Title = "Production by Status", ChartType = "bar", Labels = new[] { "" }, Values = new[] { 0m }, AccentColor = "#1B86E5", Subtitle = "Count" },
            new() { Title = "Inventory by Warehouse", ChartType = "donut", Labels = new[] { "Dallas Main" }, Values = new[] { inventoryValue }, AccentColor = "#0A5C9E", Subtitle = "$" },
            new() { Title = "Top Paint Items", ChartType = "bar", Labels = new[] { "Liberty Red Latex" }, Values = new[] { 2m }, AccentColor = "#0E9F9F", Subtitle = "Units Sold" },
            new() { Title = "Top Customers", ChartType = "bar", Labels = new[] { "Evergreen Retail" }, Values = new[] { revenue / 1000 }, AccentColor = "#1B86E5", Subtitle = "$K Revenue" },
            new() { Title = "Top Vendors", ChartType = "bar", Labels = new[] { "Titan Pigments" }, Values = new[] { payables / 1000 }, AccentColor = "#0A5C9E", Subtitle = "$K Spend" },
            new() { Title = "Monthly Profit", ChartType = "area", Labels = months.Select(m => m.ToString("MMM")), Values = monthlyProfit, AccentColor = "#0E9F9F", Subtitle = "US Dollars" }
        };

        var widgets = new List<ListWidget>
        {
            new()
            {
                Title = "Recent Bills",
                Rows = await context.Bills.AsNoTracking()
                    .Include(b => b.Vendor)
                    .OrderByDescending(b => b.BillDate)
                    .Take(4)
                    .Select(b => new ListWidgetRow
                    {
                        Primary = $"BILL-{b.Id:0000} · {b.GrandTotal:C0}",
                        Secondary = b.Vendor != null ? b.Vendor.BusinessName : "Unknown",
                        Status = b.Status == "Paid" ? "Paid" : "Due",
                        StatusClass = b.Status == "Paid" ? "status-success" : "status-warning"
                    }).ToListAsync(cancellationToken)
            },
            new()
            {
                Title = "Recent Invoices",
                Rows = await context.SalesInvoices.AsNoTracking()
                    .Include(i => i.Customer)
                    .OrderByDescending(i => i.InvoiceDate)
                    .Take(4)
                    .Select(i => new ListWidgetRow
                    {
                        Primary = $"INV-{i.Id:0000} · {i.GrandTotal:C0}",
                        Secondary = i.Customer != null ? i.Customer.BusinessName : "Unknown",
                        Status = i.Status == "Paid" ? "Paid" : "Open",
                        StatusClass = i.Status == "Paid" ? "status-success" : "status-warning"
                    }).ToListAsync(cancellationToken)
            },
            new()
            {
                Title = "Production Queue",
                Rows = await context.PaintProductions.AsNoTracking()
                    .OrderBy(p => p.Status)
                    .Take(4)
                    .Select(p => new ListWidgetRow
                    {
                        Primary = p.Recipe ?? "Unknown",
                        Secondary = $"Batch {p.BatchNumber} · {p.OutputQuantity} units",
                        Status = p.Status,
                        StatusClass = p.Status == "Completed" ? "status-success" : "status-warning"
                    }).ToListAsync(cancellationToken)
            },
            new()
            {
                Title = "Pending Purchase Orders",
                Rows = await context.PurchaseOrders.AsNoTracking()
                    .Where(p => p.Status == "Open" || p.Status == "Pending")
                    .Select(p => new ListWidgetRow
                    {
                        Primary = $"PO-{p.Id}",
                        Secondary = $"{p.OrderDate:MMM dd} · {p.TotalAmount:C0}",
                        Status = p.Status,
                        StatusClass = "status-warning"
                    }).ToListAsync(cancellationToken)
            },
            new()
            {
                Title = "Recent Customer Payments",
                Rows = await context.CustomerPayments.AsNoTracking()
                    .Include(cp => cp.Customer)
                    .OrderByDescending(cp => cp.PaymentDate)
                    .Take(4)
                    .Select(cp => new ListWidgetRow
                    {
                        Primary = cp.Customer != null ? cp.Customer.BusinessName : "Unknown",
                        Secondary = cp.AmountReceived.ToString("C0"),
                        Status = "Received",
                        StatusClass = "status-success"
                    }).ToListAsync(cancellationToken)
            },
            new()
            {
                Title = "Upcoming Vendor Bills",
                Rows = await context.Bills.AsNoTracking()
                    .Include(b => b.Vendor)
                    .Where(b => b.Status != "Paid" && b.Status != "Void")
                    .OrderBy(b => b.DueDate)
                    .Take(4)
                    .Select(b => new ListWidgetRow
                    {
                        Primary = b.Vendor != null ? b.Vendor.BusinessName : "Unknown",
                        Secondary = $"{b.DueDate:MMM dd} · {b.GrandTotal:C0}",
                        Status = b.Status == "Paid" ? "Scheduled" : "Due",
                        StatusClass = b.Status == "Paid" ? "status-info" : "status-warning"
                    }).ToListAsync(cancellationToken)
            }
        };

        return new DashboardViewModel
        {
            CompanyName = company.Name,
            CompanySubtitle = $"{company.Industry} • {company.Country}",
            Kpis = kpis,
            Charts = charts,
            Widgets = widgets
        };
    }
}
