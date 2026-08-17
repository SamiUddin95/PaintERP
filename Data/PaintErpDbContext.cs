using Microsoft.EntityFrameworkCore;
using PaintERP.Models.Entities;

namespace PaintERP.Data;

public class PaintErpDbContext(DbContextOptions<PaintErpDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<PaintItem> PaintItems => Set<PaintItem>();
    public DbSet<PaintFormula> PaintFormulas => Set<PaintFormula>();
    public DbSet<PaintFormulaItem> PaintFormulaItems => Set<PaintFormulaItem>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
    public DbSet<GoodsReceivedNote> GoodsReceivedNotes => Set<GoodsReceivedNote>();
    public DbSet<GoodsReceivedNoteItem> GoodsReceivedNoteItems => Set<GoodsReceivedNoteItem>();
    public DbSet<ProductionOrder> ProductionOrders => Set<ProductionOrder>();
    public DbSet<PaintProduction> PaintProductions => Set<PaintProduction>();
    public DbSet<PaintProductionMaterial> PaintProductionMaterials => Set<PaintProductionMaterial>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BillItem> BillItems => Set<BillItem>();
    public DbSet<BillPayment> BillPayments => Set<BillPayment>();
    public DbSet<InventoryTransfer> InventoryTransfers => Set<InventoryTransfer>();
    public DbSet<InventoryTransferItem> InventoryTransferItems => Set<InventoryTransferItem>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceItem> SalesInvoiceItems => Set<SalesInvoiceItem>();
    public DbSet<VendorPayment> VendorPayments => Set<VendorPayment>();
    public DbSet<VendorPaymentBill> VendorPaymentBills => Set<VendorPaymentBill>();
    public DbSet<CustomerPayment> CustomerPayments => Set<CustomerPayment>();
    public DbSet<CustomerPaymentInvoice> CustomerPaymentInvoices => Set<CustomerPaymentInvoice>();
    public DbSet<StockLedger> StockLedgers => Set<StockLedger>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AppSettings> AppSettings => Set<AppSettings>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();
    public DbSet<UnitConversion> UnitConversions => Set<UnitConversion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UnitConversion>()
            .Property(uc => uc.ConversionFactor)
            .HasPrecision(18, 6);

        modelBuilder.Entity<Customer>()
            .Property(c => c.LifetimeRevenue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Customer>()
            .Property(c => c.OpeningBalance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Customer>()
            .Property(c => c.CreditLimit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Customer>()
            .Property(c => c.CustomerDiscountPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Customer>()
            .Property(c => c.TotalSales)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Customer>()
            .Property(c => c.OutstandingBalance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Customer>()
            .Property(c => c.LifetimeRevenue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Customer>()
            .Property(c => c.AverageOrderValue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Vendor>()
            .Property(v => v.OutstandingAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Vendor>()
            .Property(v => v.OpeningBalance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Vendor>()
            .Property(v => v.CreditLimit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Vendor>()
            .Property(v => v.MinimumOrderQuantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Vendor>()
            .Property(v => v.DefaultDiscountPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Vendor>()
            .Property(v => v.DefaultTaxRate)
            .HasPrecision(5, 2);

        modelBuilder.Entity<SalesOrder>()
            .Property(s => s.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrder>()
            .Property(p => p.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrder>()
            .Property(p => p.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrder>()
            .Property(p => p.DiscountAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrder>()
            .Property(p => p.ShippingCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrder>()
            .Property(p => p.TaxAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrder>()
            .Property(p => p.AmountReceived)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrderItem>()
            .Property(poi => poi.QuantityOrdered)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrderItem>()
            .Property(poi => poi.QuantityReceived)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrderItem>()
            .Property(poi => poi.QuantityPending)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrderItem>()
            .Property(poi => poi.UnitCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrderItem>()
            .Property(poi => poi.DiscountPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PurchaseOrderItem>()
            .Property(poi => poi.DiscountAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrderItem>()
            .Property(poi => poi.TaxPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PurchaseOrderItem>()
            .Property(poi => poi.TaxAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrderItem>()
            .Property(poi => poi.LineTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PurchaseOrder>()
            .HasOne(p => p.Company)
            .WithMany()
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseOrder>()
            .HasOne(p => p.Warehouse)
            .WithMany()
            .HasForeignKey(p => p.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseOrderItem>()
            .HasOne(poi => poi.PurchaseOrder)
            .WithMany(po => po.PurchaseOrderItems)
            .HasForeignKey(poi => poi.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PurchaseOrderItem>()
            .HasOne(poi => poi.PaintItem)
            .WithMany()
            .HasForeignKey(poi => poi.PaintItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaintProduction>()
            .Property(p => p.OutputQuantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProduction>()
            .Property(p => p.MaterialCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProduction>()
            .Property(p => p.LaborCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProduction>()
            .Property(p => p.OverheadCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProduction>()
            .Property(p => p.ProductionCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProduction>()
            .Property(p => p.FinishedProductCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProduction>()
            .Property(p => p.CostPerUnit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProductionMaterial>()
            .Property(p => p.RequiredQuantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProductionMaterial>()
            .Property(p => p.ConsumedQuantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProductionMaterial>()
            .Property(p => p.UnitCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProductionMaterial>()
            .Property(p => p.TotalCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProductionMaterial>()
            .Property(p => p.PercentageInMix)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PaintProductionMaterial>()
            .Property(p => p.StockBefore)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProductionMaterial>()
            .Property(p => p.StockAfter)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintProduction>()
            .HasOne(p => p.Company)
            .WithMany()
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaintProduction>()
            .HasOne(p => p.Warehouse)
            .WithMany()
            .HasForeignKey(p => p.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaintProduction>()
            .HasOne(p => p.FinishedProduct)
            .WithMany()
            .HasForeignKey(p => p.FinishedProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaintProduction>()
            .HasOne(p => p.Formula)
            .WithMany()
            .HasForeignKey(p => p.FormulaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaintProductionMaterial>()
            .HasOne(pm => pm.PaintProduction)
            .WithMany(p => p.Materials)
            .HasForeignKey(pm => pm.PaintProductionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PaintProductionMaterial>()
            .HasOne(pm => pm.PaintItem)
            .WithMany()
            .HasForeignKey(pm => pm.PaintItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaintFormula>()
            .Property(f => f.TotalFormulaCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintFormula>()
            .Property(f => f.ExpectedYield)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintFormula>()
            .Property(f => f.WastePercentage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PaintFormula>()
            .Property(f => f.SellingPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintFormula>()
            .Property(f => f.GrossMargin)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PaintFormulaItem>()
            .Property(fi => fi.Percentage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PaintFormulaItem>()
            .Property(fi => fi.RequiredQuantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintFormulaItem>()
            .Property(fi => fi.UnitCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintFormulaItem>()
            .Property(fi => fi.InventoryAvailable)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintFormula>()
            .HasOne(f => f.Company)
            .WithMany()
            .HasForeignKey(f => f.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaintFormula>()
            .HasOne(f => f.ParentFormula)
            .WithMany()
            .HasForeignKey(f => f.ParentFormulaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaintFormulaItem>()
            .HasOne(fi => fi.PaintFormula)
            .WithMany(f => f.Items)
            .HasForeignKey(fi => fi.PaintFormulaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PaintFormulaItem>()
            .HasOne(fi => fi.PaintItem)
            .WithMany()
            .HasForeignKey(fi => fi.PaintItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Invoice>()
            .Property(i => i.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Bill>()
            .Property(b => b.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Bill>()
            .Property(b => b.DiscountAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Bill>()
            .Property(b => b.ShippingCharges)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Bill>()
            .Property(b => b.TaxAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Bill>()
            .Property(b => b.OtherCharges)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Bill>()
            .Property(b => b.GrandTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Bill>()
            .Property(b => b.BalanceDue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BillItem>()
            .Property(bi => bi.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BillItem>()
            .Property(bi => bi.UnitCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BillItem>()
            .Property(bi => bi.DiscountPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<BillItem>()
            .Property(bi => bi.DiscountAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BillItem>()
            .Property(bi => bi.TaxPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<BillItem>()
            .Property(bi => bi.TaxAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BillItem>()
            .Property(bi => bi.LineTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BillPayment>()
            .Property(bp => bp.PaymentAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BillPayment>()
            .Property(bp => bp.DiscountTaken)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BillPayment>()
            .Property(bp => bp.WriteOffAmount)
            .HasPrecision(18, 2);

        // InventoryTransfer configuration
        modelBuilder.Entity<InventoryTransfer>()
            .HasOne(it => it.SourceWarehouse)
            .WithMany()
            .HasForeignKey(it => it.SourceWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryTransfer>()
            .HasOne(it => it.DestinationWarehouse)
            .WithMany()
            .HasForeignKey(it => it.DestinationWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryTransfer>()
            .HasOne(it => it.Company)
            .WithMany()
            .HasForeignKey(it => it.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryTransferItem>()
            .HasOne(iti => iti.InventoryTransfer)
            .WithMany(it => it.InventoryTransferItems)
            .HasForeignKey(iti => iti.InventoryTransferId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InventoryTransferItem>()
            .HasOne(iti => iti.PaintItem)
            .WithMany()
            .HasForeignKey(iti => iti.PaintItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InventoryTransferItem>()
            .Property(iti => iti.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InventoryTransferItem>()
            .Property(iti => iti.SourceStockBefore)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InventoryTransferItem>()
            .Property(iti => iti.SourceStockAfter)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InventoryTransferItem>()
            .Property(iti => iti.DestinationStockBefore)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InventoryTransferItem>()
            .Property(iti => iti.DestinationStockAfter)
            .HasPrecision(18, 2);

        // SalesInvoice configuration
        modelBuilder.Entity<SalesInvoice>()
            .HasOne(si => si.Company)
            .WithMany()
            .HasForeignKey(si => si.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SalesInvoice>()
            .HasOne(si => si.Customer)
            .WithMany()
            .HasForeignKey(si => si.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesInvoice>()
            .HasOne(si => si.Warehouse)
            .WithMany()
            .HasForeignKey(si => si.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesInvoiceItem>()
            .HasOne(sii => sii.SalesInvoice)
            .WithMany(si => si.SalesInvoiceItems)
            .HasForeignKey(sii => sii.SalesInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SalesInvoiceItem>()
            .HasOne(sii => sii.PaintItem)
            .WithMany()
            .HasForeignKey(sii => sii.PaintItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesInvoice>()
            .Property(si => si.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoice>()
            .Property(si => si.DiscountAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoice>()
            .Property(si => si.ShippingCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoice>()
            .Property(si => si.SalesTaxAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoice>()
            .Property(si => si.GrandTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoice>()
            .Property(si => si.AmountPaid)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoice>()
            .Property(si => si.BalanceDue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(sii => sii.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(sii => sii.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(sii => sii.DiscountPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(sii => sii.DiscountAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(sii => sii.TaxPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(sii => sii.TaxAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(sii => sii.LineTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(sii => sii.StockBefore)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SalesInvoiceItem>()
            .Property(sii => sii.StockAfter)
            .HasPrecision(18, 2);

        // VendorPayment configuration
        modelBuilder.Entity<VendorPayment>()
            .HasOne(vp => vp.Company)
            .WithMany()
            .HasForeignKey(vp => vp.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VendorPayment>()
            .HasOne(vp => vp.Vendor)
            .WithMany()
            .HasForeignKey(vp => vp.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VendorPaymentBill>()
            .HasOne(vpb => vpb.VendorPayment)
            .WithMany(vp => vp.VendorPaymentBills)
            .HasForeignKey(vpb => vpb.VendorPaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VendorPaymentBill>()
            .HasOne(vpb => vpb.Bill)
            .WithMany()
            .HasForeignKey(vpb => vpb.BillId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VendorPayment>()
            .Property(vp => vp.TotalPaymentAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<VendorPayment>()
            .Property(vp => vp.TotalApplied)
            .HasPrecision(18, 2);

        modelBuilder.Entity<VendorPayment>()
            .Property(vp => vp.UnappliedAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<VendorPaymentBill>()
            .Property(vpb => vpb.BillAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<VendorPaymentBill>()
            .Property(vpb => vpb.AmountDue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<VendorPaymentBill>()
            .Property(vpb => vpb.PaymentAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<VendorPaymentBill>()
            .Property(vpb => vpb.RemainingBalance)
            .HasPrecision(18, 2);

        // CustomerPayment configuration
        modelBuilder.Entity<CustomerPayment>()
            .HasOne(cp => cp.Company)
            .WithMany()
            .HasForeignKey(cp => cp.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomerPayment>()
            .HasOne(cp => cp.Customer)
            .WithMany()
            .HasForeignKey(cp => cp.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomerPaymentInvoice>()
            .HasOne(cpi => cpi.CustomerPayment)
            .WithMany(cp => cp.CustomerPaymentInvoices)
            .HasForeignKey(cpi => cpi.CustomerPaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CustomerPaymentInvoice>()
            .HasOne(cpi => cpi.SalesInvoice)
            .WithMany()
            .HasForeignKey(cpi => cpi.SalesInvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomerPayment>()
            .Property(cp => cp.AmountReceived)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CustomerPayment>()
            .Property(cp => cp.TotalApplied)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CustomerPayment>()
            .Property(cp => cp.UnappliedAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CustomerPaymentInvoice>()
            .Property(cpi => cpi.InvoiceAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CustomerPaymentInvoice>()
            .Property(cpi => cpi.AmountDue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CustomerPaymentInvoice>()
            .Property(cpi => cpi.PaymentApplied)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CustomerPaymentInvoice>()
            .Property(cpi => cpi.RemainingBalance)
            .HasPrecision(18, 2);

        // StockLedger configuration
        modelBuilder.Entity<StockLedger>()
            .HasOne(sl => sl.Company)
            .WithMany()
            .HasForeignKey(sl => sl.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockLedger>()
            .HasOne(sl => sl.Warehouse)
            .WithMany()
            .HasForeignKey(sl => sl.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockLedger>()
            .HasOne(sl => sl.PaintItem)
            .WithMany()
            .HasForeignKey(sl => sl.PaintItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockLedger>()
            .Property(sl => sl.InQty)
            .HasPrecision(18, 2);

        modelBuilder.Entity<StockLedger>()
            .Property(sl => sl.OutQty)
            .HasPrecision(18, 2);

        modelBuilder.Entity<StockLedger>()
            .Property(sl => sl.RunningBalance)
            .HasPrecision(18, 2);

        // AppUser configuration
        modelBuilder.Entity<AppUser>()
            .HasOne(au => au.Company)
            .WithMany()
            .HasForeignKey(au => au.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // AppSettings configuration
        modelBuilder.Entity<AppSettings>(entity =>
        {
            entity.Property(s => s.DefaultTaxPercent).HasPrecision(5, 2);
            entity.Property(s => s.CompanyAddress).IsRequired(false);
            entity.Property(s => s.CompanyPhone).IsRequired(false);
            entity.Property(s => s.CompanyEmail).IsRequired(false);
            entity.Property(s => s.CompanyTaxId).IsRequired(false);
            entity.Property(s => s.CompanyLogoUrl).IsRequired(false);
            entity.Property(s => s.DefaultTaxCode).IsRequired(false);
            entity.Property(s => s.ApprovalWorkflow).IsRequired(false);
            entity.Property(s => s.InvoiceEmailTemplate).IsRequired(false);
            entity.Property(s => s.PaymentReceiptTemplate).IsRequired(false);
            entity.Property(s => s.PurchaseOrderEmailTemplate).IsRequired(false);
            entity.Property(s => s.QuickBooksApiKey).IsRequired(false);
            entity.Property(s => s.ShopifyApiKey).IsRequired(false);
            entity.Property(s => s.AmazonApiKey).IsRequired(false);
            entity.Property(s => s.UpsApiKey).IsRequired(false);
            entity.Property(s => s.FedExApiKey).IsRequired(false);
            entity.Property(s => s.UspsApiKey).IsRequired(false);
        });

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.UnitCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.PurchasePrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.SellingPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.MSRP)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.MinimumStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.MaximumStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.ReorderPoint)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.Weight)
            .HasPrecision(18, 3);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.CurrentStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.AvailableStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.ReservedStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .Property(p => p.InventoryValue)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PaintItem>()
            .HasOne(p => p.PreferredVendor)
            .WithMany()
            .HasForeignKey(p => p.PreferredVendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesOrder>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.SalesOrders)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Bill>()
            .HasOne(b => b.Vendor)
            .WithMany(v => v.Bills)
            .HasForeignKey(b => b.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Bill>()
            .HasOne(b => b.PurchaseOrder)
            .WithMany()
            .HasForeignKey(b => b.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Bill>()
            .HasOne(b => b.GoodsReceivedNote)
            .WithMany()
            .HasForeignKey(b => b.GRNId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Bill>()
            .HasOne(b => b.Warehouse)
            .WithMany()
            .HasForeignKey(b => b.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BillItem>()
            .HasOne(bi => bi.Bill)
            .WithMany(b => b.BillItems)
            .HasForeignKey(bi => bi.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BillItem>()
            .HasOne(bi => bi.PaintItem)
            .WithMany()
            .HasForeignKey(bi => bi.PaintItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BillItem>()
            .HasOne(bi => bi.Warehouse)
            .WithMany()
            .HasForeignKey(bi => bi.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BillPayment>()
            .HasOne(bp => bp.Bill)
            .WithMany(b => b.BillPayments)
            .HasForeignKey(bp => bp.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PurchaseOrder>()
            .HasOne(p => p.Vendor)
            .WithMany(v => v.PurchaseOrders)
            .HasForeignKey(p => p.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GoodsReceivedNote>()
            .HasOne(grn => grn.PurchaseOrder)
            .WithMany(po => po.GoodsReceivedNotes)
            .HasForeignKey(grn => grn.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // GoodsReceivedNote configuration
        modelBuilder.Entity<GoodsReceivedNote>()
            .Property(grn => grn.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNote>()
            .Property(grn => grn.DiscountAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNote>()
            .Property(grn => grn.TaxAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNote>()
            .Property(grn => grn.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNote>()
            .HasOne(grn => grn.Company)
            .WithMany()
            .HasForeignKey(grn => grn.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GoodsReceivedNote>()
            .HasOne(grn => grn.PurchaseOrder)
            .WithMany()
            .HasForeignKey(grn => grn.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GoodsReceivedNote>()
            .HasOne(grn => grn.Vendor)
            .WithMany()
            .HasForeignKey(grn => grn.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GoodsReceivedNote>()
            .HasOne(grn => grn.Warehouse)
            .WithMany()
            .HasForeignKey(grn => grn.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .HasOne(grni => grni.GoodsReceivedNote)
            .WithMany(grn => grn.GoodsReceivedNoteItems)
            .HasForeignKey(grni => grni.GoodsReceivedNoteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .HasOne(grni => grni.PurchaseOrderItem)
            .WithMany()
            .HasForeignKey(grni => grni.PurchaseOrderItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .HasOne(grni => grni.PaintItem)
            .WithMany()
            .HasForeignKey(grni => grni.PaintItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.QuantityOrdered)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.QuantityReceived)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.QuantityPreviouslyReceived)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.QuantityRemaining)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.UnitCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.DiscountPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.DiscountAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.TaxPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.TaxAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<GoodsReceivedNoteItem>()
            .Property(grni => grni.LineTotal)
            .HasPrecision(18, 2);

        var referenceDate = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Company>().HasData(new Company
        {
            Id = 1,
            Name = "USA Paint ERP",
            Industry = "Industrial Coatings",
            Country = "USA",
            PrimaryColor = "#0A5C9E",
            LogoUrl = "/images/logos/painterp-logo.svg"
        });

        modelBuilder.Entity<Warehouse>().HasData(
            new Warehouse { Id = 1, CompanyId = 1, Name = "Dallas Main", Location = "Dallas, TX" },
            new Warehouse { Id = 2, CompanyId = 1, Name = "Atlanta Distribution", Location = "Atlanta, GA" }
        );

        modelBuilder.Entity<PaintItem>().HasData(
            new PaintItem { Id = 1, WarehouseId = 1, SKU = "SKU-PB-001", Name = "Patriot Blue High Gloss", ColorFamily = "Blue", ColorHex = "#0D47A1", UnitCost = 45m, StockQuantity = 420, ReorderLevel = 150, ItemType = "Finished Product", PurchasePrice = 35m, SellingPrice = 55m, MSRP = 65m, PreferredVendorId = null, CreatedAtUtc = new DateTime(2026, 1, 5), UpdatedAtUtc = new DateTime(2026, 1, 5) },
            new PaintItem { Id = 2, WarehouseId = 2, SKU = "SKU-LR-002", Name = "Liberty Red Latex", ColorFamily = "Red", ColorHex = "#C62828", UnitCost = 38m, StockQuantity = 280, ReorderLevel = 120, ItemType = "Finished Product", PurchasePrice = 30m, SellingPrice = 48m, MSRP = 58m, PreferredVendorId = null, CreatedAtUtc = new DateTime(2026, 1, 10), UpdatedAtUtc = new DateTime(2026, 1, 10) }
        );

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, CompanyId = 1, CustomerId = "CST26-0001", BusinessName = "Coastal Builders", ContactEmail = "finance@coastalbuilders.us", Industry = "Construction", LifetimeRevenue = 560000m, CustomerSince = new DateTime(2026, 1, 10), CreatedAtUtc = new DateTime(2026, 1, 10), UpdatedAtUtc = new DateTime(2026, 1, 10) },
            new Customer { Id = 2, CompanyId = 1, CustomerId = "CST26-0002", BusinessName = "Evergreen Retail", ContactEmail = "ap@evergreenretail.com", Industry = "Retail", LifetimeRevenue = 420000m, CustomerSince = new DateTime(2026, 2, 15), CreatedAtUtc = new DateTime(2026, 2, 15), UpdatedAtUtc = new DateTime(2026, 2, 15) }
        );

        modelBuilder.Entity<Vendor>().HasData(
            new Vendor { Id = 1, CompanyId = 1, VendorId = "VND26-0001", BusinessName = "Titan Pigments", ContactEmail = "orders@titanpigments.com", VendorCategory = "Raw Materials", OutstandingAmount = 82000m, VendorSince = new DateTime(2026, 1, 15), CreatedAtUtc = new DateTime(2026, 1, 15), UpdatedAtUtc = new DateTime(2026, 1, 15) },
            new Vendor { Id = 2, CompanyId = 1, VendorId = "VND26-0002", BusinessName = "Northshore Packaging", ContactEmail = "billing@northshorepack.com", VendorCategory = "Packaging", OutstandingAmount = 31000m, VendorSince = new DateTime(2026, 2, 20), CreatedAtUtc = new DateTime(2026, 2, 20), UpdatedAtUtc = new DateTime(2026, 2, 20) }
        );

        modelBuilder.Entity<SalesOrder>().HasData(
            new SalesOrder { Id = 1, CompanyId = 1, CustomerId = 1, OrderDate = referenceDate.AddDays(-1), TotalAmount = 85000m, Status = "Open" },
            new SalesOrder { Id = 2, CompanyId = 1, CustomerId = 2, OrderDate = referenceDate.AddDays(-2), TotalAmount = 42000m, Status = "Fulfilled" }
        );

        modelBuilder.Entity<PurchaseOrder>().HasData(
            new PurchaseOrder { Id = 1, CompanyId = 1, VendorId = 1, OrderDate = referenceDate.AddDays(-3), TotalAmount = 29000m, Status = "Awaiting Approval" },
            new PurchaseOrder { Id = 2, CompanyId = 1, VendorId = 2, OrderDate = referenceDate.AddDays(-5), TotalAmount = 18000m, Status = "Approved" }
        );

        modelBuilder.Entity<ProductionOrder>().HasData(
            new ProductionOrder { Id = 1, CompanyId = 1, ProductionDate = referenceDate, BatchNumber = "PB-2407", ProductName = "Patriot Blue High Gloss", Quantity = 1200, Status = "Mixing" },
            new ProductionOrder { Id = 2, CompanyId = 1, ProductionDate = referenceDate.AddDays(-1), BatchNumber = "LR-2406", ProductName = "Liberty Red Latex", Quantity = 900, Status = "Quality Check" }
        );

        modelBuilder.Entity<Invoice>().HasData(
            new Invoice { Id = 1, CompanyId = 1, CustomerId = 1, InvoiceDate = referenceDate.AddDays(-6), Amount = 62000m, IsPaid = false },
            new Invoice { Id = 2, CompanyId = 1, CustomerId = 2, InvoiceDate = referenceDate.AddDays(-10), Amount = 48000m, IsPaid = true }
        );

        // ===== DATABASE INDEXES FOR PERFORMANCE =====
        
        // Customer indexes
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.CustomerId)
            .IsUnique();
        
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.ContactEmail);
        
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.IsActive);

        // Vendor indexes
        modelBuilder.Entity<Vendor>()
            .HasIndex(v => v.VendorId)
            .IsUnique();
        
        modelBuilder.Entity<Vendor>()
            .HasIndex(v => v.ContactEmail);
        
        modelBuilder.Entity<Vendor>()
            .HasIndex(v => v.IsActive);

        // SalesInvoice indexes
        modelBuilder.Entity<SalesInvoice>()
            .HasIndex(si => si.InvoiceNumber)
            .IsUnique();
        
        modelBuilder.Entity<SalesInvoice>()
            .HasIndex(si => si.InvoiceDate);
        
        modelBuilder.Entity<SalesInvoice>()
            .HasIndex(si => si.Status);
        
        modelBuilder.Entity<SalesInvoice>()
            .HasIndex(si => new { si.CustomerId, si.InvoiceDate });

        // PurchaseOrder indexes
        modelBuilder.Entity<PurchaseOrder>()
            .HasIndex(po => po.PONumber)
            .IsUnique();
        
        modelBuilder.Entity<PurchaseOrder>()
            .HasIndex(po => po.OrderDate);
        
        modelBuilder.Entity<PurchaseOrder>()
            .HasIndex(po => po.Status);
        
        modelBuilder.Entity<PurchaseOrder>()
            .HasIndex(po => new { po.VendorId, po.OrderDate });

        // Bill indexes
        modelBuilder.Entity<Bill>()
            .HasIndex(b => b.BillNumber)
            .IsUnique();
        
        modelBuilder.Entity<Bill>()
            .HasIndex(b => b.BillDate);
        
        modelBuilder.Entity<Bill>()
            .HasIndex(b => b.Status);
        
        modelBuilder.Entity<Bill>()
            .HasIndex(b => new { b.VendorId, b.BillDate });

        // PaintItem indexes
        modelBuilder.Entity<PaintItem>()
            .Property(p => p.SKU)
            .IsRequired(false);

        modelBuilder.Entity<PaintItem>()
            .HasIndex(p => p.SKU);
        
        modelBuilder.Entity<PaintItem>()
            .HasIndex(p => p.Name);
        
        modelBuilder.Entity<PaintItem>()
            .HasIndex(p => new { p.WarehouseId, p.StockQuantity });

        // PaintProduction indexes
        modelBuilder.Entity<PaintProduction>()
            .HasIndex(pp => pp.ProductionNumber)
            .IsUnique();
        
        modelBuilder.Entity<PaintProduction>()
            .HasIndex(pp => pp.ProductionDate);
        
        modelBuilder.Entity<PaintProduction>()
            .HasIndex(pp => pp.Status);

        // InventoryTransfer indexes
        modelBuilder.Entity<InventoryTransfer>()
            .HasIndex(it => it.TransferNumber)
            .IsUnique();
        
        modelBuilder.Entity<InventoryTransfer>()
            .HasIndex(it => it.TransferDate);
        
        modelBuilder.Entity<InventoryTransfer>()
            .HasIndex(it => it.Status);

        // CustomerPayment indexes
        modelBuilder.Entity<CustomerPayment>()
            .HasIndex(cp => cp.PaymentNumber)
            .IsUnique();
        
        modelBuilder.Entity<CustomerPayment>()
            .HasIndex(cp => cp.PaymentDate);
        
        modelBuilder.Entity<CustomerPayment>()
            .HasIndex(cp => new { cp.CustomerId, cp.PaymentDate });

        // VendorPayment indexes
        modelBuilder.Entity<VendorPayment>()
            .HasIndex(vp => vp.PaymentNumber)
            .IsUnique();
        
        modelBuilder.Entity<VendorPayment>()
            .HasIndex(vp => vp.PaymentDate);
        
        modelBuilder.Entity<VendorPayment>()
            .HasIndex(vp => new { vp.VendorId, vp.PaymentDate });

        // StockLedger indexes
        modelBuilder.Entity<StockLedger>()
            .HasIndex(sl => sl.TransactionDate);
        
        modelBuilder.Entity<StockLedger>()
            .HasIndex(sl => new { sl.PaintItemId, sl.WarehouseId, sl.TransactionDate });
        
        modelBuilder.Entity<StockLedger>()
            .HasIndex(sl => sl.TransactionType);

        // JournalEntry indexes
        modelBuilder.Entity<JournalEntry>()
            .HasIndex(je => je.EntryNumber)
            .IsUnique();
        
        modelBuilder.Entity<JournalEntry>()
            .HasIndex(je => je.EntryDate);
        
        modelBuilder.Entity<JournalEntry>()
            .HasIndex(je => je.TransactionType);
        
        modelBuilder.Entity<JournalEntry>()
            .HasIndex(je => new { je.TransactionType, je.ReferenceId });

        // JournalEntry configuration
        modelBuilder.Entity<JournalEntry>()
            .HasOne(je => je.Company)
            .WithMany()
            .HasForeignKey(je => je.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntryLine>()
            .HasOne(jel => jel.JournalEntry)
            .WithMany(je => je.JournalEntryLines)
            .HasForeignKey(jel => jel.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JournalEntry>()
            .Property(je => je.TotalDebit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<JournalEntry>()
            .Property(je => je.TotalCredit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<JournalEntryLine>()
            .Property(jel => jel.DebitAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<JournalEntryLine>()
            .Property(jel => jel.CreditAmount)
            .HasPrecision(18, 2);
    }
}
