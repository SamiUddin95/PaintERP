namespace PaintERP.Models.ViewModels;

public class DashboardViewModel
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanySubtitle { get; set; } = string.Empty;
    public List<KpiCard> Kpis { get; set; } = new();
    public List<ChartCard> Charts { get; set; } = new();
    public List<ListWidget> Widgets { get; set; } = new();
}

public class KpiCard
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string TrendLabel { get; set; } = string.Empty;
    public string TrendValue { get; set; } = string.Empty;
    public string AccentColor { get; set; } = "#0A5C9E";
}

public class ChartCard
{
    public string Title { get; set; } = string.Empty;
    public string ChartType { get; set; } = "line";
    public string AccentColor { get; set; } = "#0A5C9E";
    public IEnumerable<string> Labels { get; set; } = Array.Empty<string>();
    public IEnumerable<decimal> Values { get; set; } = Array.Empty<decimal>();
    public string Subtitle { get; set; } = string.Empty;
}

public class ListWidget
{
    public string Title { get; set; } = string.Empty;
    public IEnumerable<ListWidgetRow> Rows { get; set; } = Array.Empty<ListWidgetRow>();
}

public class ListWidgetRow
{
    public string Primary { get; set; } = string.Empty;
    public string Secondary { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusClass { get; set; } = "status-info";
}
