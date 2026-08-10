using PaintERP.Models.Entities;

namespace PaintERP.Services;

public interface IAccountingService
{
    Task<JournalEntry> CreateSalesInvoiceEntryAsync(SalesInvoice invoice, CancellationToken cancellationToken = default);
    Task<JournalEntry> CreatePurchaseOrderEntryAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);
    Task<JournalEntry> CreateBillEntryAsync(Bill bill, CancellationToken cancellationToken = default);
    Task<JournalEntry> CreateCustomerPaymentEntryAsync(CustomerPayment payment, CancellationToken cancellationToken = default);
    Task<JournalEntry> CreateVendorPaymentEntryAsync(VendorPayment payment, CancellationToken cancellationToken = default);
    Task<JournalEntry> CreateProductionEntryAsync(PaintProduction production, CancellationToken cancellationToken = default);
    Task<bool> ReverseJournalEntryAsync(int journalEntryId, string reason, CancellationToken cancellationToken = default);
    Task<string> GenerateEntryNumberAsync(CancellationToken cancellationToken = default);
}
