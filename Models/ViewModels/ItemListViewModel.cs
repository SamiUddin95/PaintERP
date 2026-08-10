namespace PaintERP.Models.ViewModels;

public class ItemListViewModel
{
    public PaginatedList<ItemListItem> Items { get; set; } = null!;
    public string SearchTerm { get; set; } = string.Empty;
    public string FilterType { get; set; } = "All"; // All, Inventory, Non Inventory, Service, Assembly, Raw Material, Finished Product
    public string FilterCategory { get; set; } = "All";
    public int TotalCount { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int LowStockCount { get; set; }
}

public class ItemListItem
{
    public int Id { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string UPCBarcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
    public decimal AvailableStock { get; set; }
    public decimal InventoryValue { get; set; }
    public decimal SellingPrice { get; set; }
    public string ColorHex { get; set; } = string.Empty;
    public bool IsHazardousMaterial { get; set; }
}
