using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;

namespace PaintERP.Services;

public class AccountingService : IAccountingService
{
    private readonly PaintErpDbContext _context;

    public AccountingService(PaintErpDbContext context)
    {
        _context = context;
    }

    public async Task<JournalEntry> CreateSalesInvoiceEntryAsync(SalesInvoice invoice, CancellationToken cancellationToken = default)
    {
        // Sales Invoice Entry:
        // DR: Accounts Receivable (Customer)
        // CR: Sales Revenue
        // DR: Cost of Goods Sold
        // CR: Inventory

        var entry = new JournalEntry
        {
            CompanyId = invoice.CompanyId,
            EntryNumber = await GenerateEntryNumberAsync(cancellationToken),
            EntryDate = invoice.InvoiceDate,
            TransactionType = "Sales Invoice",
            ReferenceId = invoice.Id,
            ReferenceNumber = invoice.InvoiceNumber,
            Description = $"Sales Invoice: {invoice.InvoiceNumber}",
            Status = "Posted",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = invoice.CreatedBy,
            UpdatedBy = invoice.UpdatedBy
        };

        var lines = new List<JournalEntryLine>();
        int lineNumber = 1;

        // DR: Accounts Receivable
        lines.Add(new JournalEntryLine
        {
            AccountCode = "1200",
            AccountName = "Accounts Receivable",
            Description = $"Customer: {invoice.Customer?.BusinessName ?? "Unknown"}",
            DebitAmount = invoice.GrandTotal,
            CreditAmount = 0,
            LineNumber = lineNumber++
        });

        // CR: Sales Revenue (Subtotal - before tax)
        lines.Add(new JournalEntryLine
        {
            AccountCode = "4000",
            AccountName = "Sales Revenue",
            Description = "Sales Revenue",
            DebitAmount = 0,
            CreditAmount = invoice.Subtotal,
            LineNumber = lineNumber++
        });

        // CR: Sales Tax Payable
        if (invoice.SalesTaxAmount > 0)
        {
            lines.Add(new JournalEntryLine
            {
                AccountCode = "2300",
                AccountName = "Sales Tax Payable",
                Description = "Sales Tax",
                DebitAmount = 0,
                CreditAmount = invoice.SalesTaxAmount,
                LineNumber = lineNumber++
            });
        }

        // Calculate COGS and reduce inventory
        if (invoice.SalesInvoiceItems != null && invoice.SalesInvoiceItems.Any())
        {
            decimal totalCOGS = 0;
            foreach (var item in invoice.SalesInvoiceItems)
            {
                var paintItem = await _context.PaintItems
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == (item.PaintItemId ?? 0), cancellationToken);
                
                if (paintItem != null)
                {
                    totalCOGS += item.Quantity * paintItem.UnitCost;
                }
            }

            if (totalCOGS > 0)
            {
                // DR: Cost of Goods Sold
                lines.Add(new JournalEntryLine
                {
                    AccountCode = "5000",
                    AccountName = "Cost of Goods Sold",
                    Description = "COGS for sold items",
                    DebitAmount = totalCOGS,
                    CreditAmount = 0,
                    LineNumber = lineNumber++
                });

                // CR: Inventory
                lines.Add(new JournalEntryLine
                {
                    AccountCode = "1300",
                    AccountName = "Inventory",
                    Description = "Inventory reduction",
                    DebitAmount = 0,
                    CreditAmount = totalCOGS,
                    LineNumber = lineNumber++
                });
            }
        }

        entry.TotalDebit = lines.Sum(l => l.DebitAmount);
        entry.TotalCredit = lines.Sum(l => l.CreditAmount);
        entry.JournalEntryLines = lines;

        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync(cancellationToken);

        return entry;
    }

    public async Task<JournalEntry> CreatePurchaseOrderEntryAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        // Purchase Order Entry (when received):
        // DR: Inventory
        // CR: Accounts Payable

        var entry = new JournalEntry
        {
            CompanyId = purchaseOrder.CompanyId,
            EntryNumber = await GenerateEntryNumberAsync(cancellationToken),
            EntryDate = purchaseOrder.OrderDate,
            TransactionType = "Purchase Order",
            ReferenceId = purchaseOrder.Id,
            ReferenceNumber = purchaseOrder.PONumber,
            Description = $"Purchase Order: {purchaseOrder.PONumber}",
            Status = "Posted",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = purchaseOrder.CreatedBy,
            UpdatedBy = purchaseOrder.UpdatedBy
        };

        var lines = new List<JournalEntryLine>();

        // DR: Inventory
        lines.Add(new JournalEntryLine
        {
            AccountCode = "1300",
            AccountName = "Inventory",
            Description = "Inventory purchase",
            DebitAmount = purchaseOrder.Subtotal,
            CreditAmount = 0,
            LineNumber = 1
        });

        // CR: Accounts Payable
        lines.Add(new JournalEntryLine
        {
            AccountCode = "2000",
            AccountName = "Accounts Payable",
            Description = $"Vendor: {purchaseOrder.Vendor?.BusinessName ?? "Unknown"}",
            DebitAmount = 0,
            CreditAmount = purchaseOrder.TotalAmount,
            LineNumber = 2
        });

        // DR: Tax if applicable
        if (purchaseOrder.TaxAmount > 0)
        {
            lines.Add(new JournalEntryLine
            {
                AccountCode = "1400",
                AccountName = "Input Tax",
                Description = "Purchase Tax",
                DebitAmount = purchaseOrder.TaxAmount,
                CreditAmount = 0,
                LineNumber = 3
            });
        }

        entry.TotalDebit = lines.Sum(l => l.DebitAmount);
        entry.TotalCredit = lines.Sum(l => l.CreditAmount);
        entry.JournalEntryLines = lines;

        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync(cancellationToken);

        return entry;
    }

    public async Task<JournalEntry> CreateBillEntryAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        // Bill Entry:
        // DR: Expense/Inventory
        // CR: Accounts Payable

        var entry = new JournalEntry
        {
            CompanyId = bill.CompanyId,
            EntryNumber = await GenerateEntryNumberAsync(cancellationToken),
            EntryDate = bill.BillDate,
            TransactionType = "Bill",
            ReferenceId = bill.Id,
            ReferenceNumber = bill.BillNumber,
            Description = $"Bill: {bill.BillNumber}",
            Status = "Posted",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = bill.CreatedBy,
            UpdatedBy = bill.UpdatedBy
        };

        var lines = new List<JournalEntryLine>();

        // DR: Inventory or Expense
        lines.Add(new JournalEntryLine
        {
            AccountCode = "1300",
            AccountName = "Inventory",
            Description = "Inventory/Expense",
            DebitAmount = bill.Subtotal,
            CreditAmount = 0,
            LineNumber = 1
        });

        // CR: Accounts Payable
        lines.Add(new JournalEntryLine
        {
            AccountCode = "2000",
            AccountName = "Accounts Payable",
            Description = $"Vendor: {bill.Vendor?.BusinessName ?? "Unknown"}",
            DebitAmount = 0,
            CreditAmount = bill.GrandTotal,
            LineNumber = 2
        });

        // DR: Tax
        if (bill.TaxAmount > 0)
        {
            lines.Add(new JournalEntryLine
            {
                AccountCode = "1400",
                AccountName = "Input Tax",
                Description = "Bill Tax",
                DebitAmount = bill.TaxAmount,
                CreditAmount = 0,
                LineNumber = 3
            });
        }

        entry.TotalDebit = lines.Sum(l => l.DebitAmount);
        entry.TotalCredit = lines.Sum(l => l.CreditAmount);
        entry.JournalEntryLines = lines;

        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync(cancellationToken);

        return entry;
    }

    public async Task<JournalEntry> CreateCustomerPaymentEntryAsync(CustomerPayment payment, CancellationToken cancellationToken = default)
    {
        // Customer Payment Entry:
        // DR: Cash/Bank
        // CR: Accounts Receivable

        var entry = new JournalEntry
        {
            CompanyId = payment.CompanyId,
            EntryNumber = await GenerateEntryNumberAsync(cancellationToken),
            EntryDate = payment.PaymentDate,
            TransactionType = "Customer Payment",
            ReferenceId = payment.Id,
            ReferenceNumber = payment.PaymentNumber,
            Description = $"Customer Payment: {payment.PaymentNumber}",
            Status = "Posted",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = payment.CreatedBy,
            UpdatedBy = payment.UpdatedBy
        };

        var lines = new List<JournalEntryLine>
        {
            new JournalEntryLine
            {
                AccountCode = "1000",
                AccountName = "Cash/Bank",
                Description = $"Payment Method: {payment.PaymentMethod}",
                DebitAmount = payment.AmountReceived,
                CreditAmount = 0,
                LineNumber = 1
            },
            new JournalEntryLine
            {
                AccountCode = "1200",
                AccountName = "Accounts Receivable",
                Description = $"Customer: {payment.Customer?.BusinessName ?? "Unknown"}",
                DebitAmount = 0,
                CreditAmount = payment.AmountReceived,
                LineNumber = 2
            }
        };

        entry.TotalDebit = lines.Sum(l => l.DebitAmount);
        entry.TotalCredit = lines.Sum(l => l.CreditAmount);
        entry.JournalEntryLines = lines;

        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync(cancellationToken);

        return entry;
    }

    public async Task<JournalEntry> CreateVendorPaymentEntryAsync(VendorPayment payment, CancellationToken cancellationToken = default)
    {
        // Vendor Payment Entry:
        // DR: Accounts Payable
        // CR: Cash/Bank

        var entry = new JournalEntry
        {
            CompanyId = payment.CompanyId,
            EntryNumber = await GenerateEntryNumberAsync(cancellationToken),
            EntryDate = payment.PaymentDate,
            TransactionType = "Vendor Payment",
            ReferenceId = payment.Id,
            ReferenceNumber = payment.PaymentNumber,
            Description = $"Vendor Payment: {payment.PaymentNumber}",
            Status = "Posted",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = payment.CreatedBy,
            UpdatedBy = payment.UpdatedBy
        };

        var lines = new List<JournalEntryLine>
        {
            new JournalEntryLine
            {
                AccountCode = "2000",
                AccountName = "Accounts Payable",
                Description = $"Vendor: {payment.Vendor?.BusinessName ?? "Unknown"}",
                DebitAmount = payment.TotalPaymentAmount,
                CreditAmount = 0,
                LineNumber = 1
            },
            new JournalEntryLine
            {
                AccountCode = "1000",
                AccountName = "Cash/Bank",
                Description = $"Payment Method: {payment.PaymentMethod}",
                DebitAmount = 0,
                CreditAmount = payment.TotalPaymentAmount,
                LineNumber = 2
            }
        };

        entry.TotalDebit = lines.Sum(l => l.DebitAmount);
        entry.TotalCredit = lines.Sum(l => l.CreditAmount);
        entry.JournalEntryLines = lines;

        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync(cancellationToken);

        return entry;
    }

    public async Task<JournalEntry> CreateProductionEntryAsync(PaintProduction production, CancellationToken cancellationToken = default)
    {
        // Production Entry:
        // DR: Work in Progress / Finished Goods
        // CR: Raw Materials Inventory

        var entry = new JournalEntry
        {
            CompanyId = production.CompanyId,
            EntryNumber = await GenerateEntryNumberAsync(cancellationToken),
            EntryDate = production.ProductionDate,
            TransactionType = "Production",
            ReferenceId = production.Id,
            ReferenceNumber = production.ProductionNumber,
            Description = $"Production: {production.ProductionNumber}",
            Status = "Posted",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = production.CreatedBy,
            UpdatedBy = production.UpdatedBy
        };

        var lines = new List<JournalEntryLine>();

        // DR: Finished Goods Inventory
        lines.Add(new JournalEntryLine
        {
            AccountCode = "1310",
            AccountName = "Finished Goods Inventory",
            Description = "Finished goods from production",
            DebitAmount = production.ProductionCost,
            CreditAmount = 0,
            LineNumber = 1
        });

        // CR: Raw Materials Inventory
        lines.Add(new JournalEntryLine
        {
            AccountCode = "1300",
            AccountName = "Raw Materials Inventory",
            Description = "Materials consumed in production",
            DebitAmount = 0,
            CreditAmount = production.MaterialCost,
            LineNumber = 2
        });

        // CR: Labor Cost
        if (production.LaborCost > 0)
        {
            lines.Add(new JournalEntryLine
            {
                AccountCode = "5100",
                AccountName = "Direct Labor",
                Description = "Labor cost",
                DebitAmount = 0,
                CreditAmount = production.LaborCost,
                LineNumber = 3
            });
        }

        // CR: Manufacturing Overhead
        if (production.OverheadCost > 0)
        {
            lines.Add(new JournalEntryLine
            {
                AccountCode = "5200",
                AccountName = "Manufacturing Overhead",
                Description = "Overhead cost",
                DebitAmount = 0,
                CreditAmount = production.OverheadCost,
                LineNumber = 4
            });
        }

        entry.TotalDebit = lines.Sum(l => l.DebitAmount);
        entry.TotalCredit = lines.Sum(l => l.CreditAmount);
        entry.JournalEntryLines = lines;

        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync(cancellationToken);

        return entry;
    }

    public async Task<bool> ReverseJournalEntryAsync(int journalEntryId, string reason, CancellationToken cancellationToken = default)
    {
        var originalEntry = await _context.JournalEntries
            .Include(je => je.JournalEntryLines)
            .FirstOrDefaultAsync(je => je.Id == journalEntryId, cancellationToken);

        if (originalEntry == null || originalEntry.IsReversed)
            return false;

        // Create reversal entry
        var reversalEntry = new JournalEntry
        {
            CompanyId = originalEntry.CompanyId,
            EntryNumber = await GenerateEntryNumberAsync(cancellationToken),
            EntryDate = DateTime.UtcNow,
            TransactionType = $"{originalEntry.TransactionType} - Reversal",
            ReferenceId = originalEntry.ReferenceId,
            ReferenceNumber = originalEntry.ReferenceNumber,
            Description = $"Reversal of {originalEntry.EntryNumber}: {reason}",
            Status = "Posted",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        // Reverse all lines (swap debit and credit)
        var reversalLines = new List<JournalEntryLine>();
        foreach (var line in originalEntry.JournalEntryLines)
        {
            reversalLines.Add(new JournalEntryLine
            {
                AccountCode = line.AccountCode,
                AccountName = line.AccountName,
                Description = $"Reversal: {line.Description}",
                DebitAmount = line.CreditAmount,
                CreditAmount = line.DebitAmount,
                LineNumber = line.LineNumber
            });
        }

        reversalEntry.TotalDebit = reversalLines.Sum(l => l.DebitAmount);
        reversalEntry.TotalCredit = reversalLines.Sum(l => l.CreditAmount);
        reversalEntry.JournalEntryLines = reversalLines;

        // Mark original as reversed
        originalEntry.IsReversed = true;
        originalEntry.ReversedDate = DateTime.UtcNow;
        originalEntry.UpdatedAtUtc = DateTime.UtcNow;

        _context.JournalEntries.Add(reversalEntry);
        await _context.SaveChangesAsync(cancellationToken);

        originalEntry.ReversedByEntryId = reversalEntry.Id;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<string> GenerateEntryNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.ToString("yy");
        var month = DateTime.UtcNow.ToString("MM");
        
        var lastEntry = await _context.JournalEntries
            .Where(je => je.EntryNumber.StartsWith($"JE-{year}{month}"))
            .OrderByDescending(je => je.EntryNumber)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNumber = 1;
        if (lastEntry != null)
        {
            var lastNumberPart = lastEntry.EntryNumber.Split('-').LastOrDefault();
            if (int.TryParse(lastNumberPart, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"JE-{year}{month}-{nextNumber:D4}";
    }
}
