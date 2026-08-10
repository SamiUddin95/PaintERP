using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;

namespace PaintERP.Controllers;

public class SettingsController : Controller
{
    private readonly PaintErpDbContext _context;

    public SettingsController(PaintErpDbContext context)
    {
        _context = context;
    }

    // GET: Settings
    public async Task<IActionResult> Index()
    {
        AppSettings? settings = null;
        try
        {
            settings = await _context.AppSettings
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch
        {
            // If query fails, create new settings
        }
        
        if (settings == null)
        {
            // Create default settings if none exist
            var defaultCompany = await _context.Companies.FirstOrDefaultAsync();
            settings = new AppSettings
            {
                CompanyId = defaultCompany?.Id ?? 1,
                CompanyName = "Unicorp Bissonet",
                FiscalYearStart = "01-01",
                FiscalYearEnd = "12-31",
                DefaultCurrency = "USD",
                Currencies = "USD",
                AccountingMethod = "Accrual",
                DefaultPaymentTerms = "Net 30",
                DefaultTaxCode = "",
                DefaultTaxPercent = 0,
                ShippingCarriers = "UPS;FedEx;USPS;DHL",
                BarcodeType = "Code128",
                ApprovalWorkflow = "",
                InvoiceEmailTemplate = "",
                PaymentReceiptTemplate = "",
                PurchaseOrderEmailTemplate = "",
                InvoiceNumberPrefix = "INV-",
                PurchaseOrderNumberPrefix = "PO-",
                BillNumberPrefix = "BILL-",
                TransferNumberPrefix = "IT-"
            };
            _context.AppSettings.Add(settings);
            await _context.SaveChangesAsync();
        }

        return View(settings);
    }

    // POST: Settings
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(AppSettings settings)
    {
        if (!ModelState.IsValid)
        {
            return View(settings);
        }

        var existingSettings = await _context.AppSettings.FirstOrDefaultAsync();
        if (existingSettings == null)
        {
            _context.AppSettings.Add(settings);
        }
        else
        {
            existingSettings.CompanyId = settings.CompanyId;
            existingSettings.CompanyName = settings.CompanyName;
            existingSettings.CompanyPhone = settings.CompanyPhone;
            existingSettings.CompanyEmail = settings.CompanyEmail;
            existingSettings.CompanyTaxId = settings.CompanyTaxId;
            existingSettings.CompanyAddress = settings.CompanyAddress;
            existingSettings.CompanyLogoUrl = settings.CompanyLogoUrl;
            existingSettings.FiscalYearStart = settings.FiscalYearStart;
            existingSettings.FiscalYearEnd = settings.FiscalYearEnd;
            existingSettings.DefaultCurrency = settings.DefaultCurrency;
            existingSettings.Currencies = settings.Currencies;
            existingSettings.AccountingMethod = settings.AccountingMethod;
            existingSettings.DefaultPaymentTerms = settings.DefaultPaymentTerms;
            existingSettings.DefaultTaxCode = settings.DefaultTaxCode;
            existingSettings.DefaultTaxPercent = settings.DefaultTaxPercent;
            existingSettings.ShippingCarriers = settings.ShippingCarriers;
            existingSettings.BarcodeType = settings.BarcodeType;
            existingSettings.ApprovalWorkflow = settings.ApprovalWorkflow;
            existingSettings.InvoiceEmailTemplate = settings.InvoiceEmailTemplate;
            existingSettings.PaymentReceiptTemplate = settings.PaymentReceiptTemplate;
            existingSettings.PurchaseOrderEmailTemplate = settings.PurchaseOrderEmailTemplate;
            existingSettings.InvoiceNumberPrefix = settings.InvoiceNumberPrefix;
            existingSettings.PurchaseOrderNumberPrefix = settings.PurchaseOrderNumberPrefix;
            existingSettings.BillNumberPrefix = settings.BillNumberPrefix;
            existingSettings.TransferNumberPrefix = settings.TransferNumberPrefix;
            existingSettings.QuickBooksApiKey = settings.QuickBooksApiKey;
            existingSettings.ShopifyApiKey = settings.ShopifyApiKey;
            existingSettings.AmazonApiKey = settings.AmazonApiKey;
            existingSettings.UpsApiKey = settings.UpsApiKey;
            existingSettings.FedExApiKey = settings.FedExApiKey;
            existingSettings.UspsApiKey = settings.UspsApiKey;
        }

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Settings saved successfully.";
        return RedirectToAction(nameof(Index));
    }
}
