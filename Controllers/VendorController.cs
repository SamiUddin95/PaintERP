using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;

namespace PaintERP.Controllers;

public class VendorController(PaintErpDbContext context) : Controller
{
    private const string AuthCookie = "PaintErpAuth";
    private const string DemoEmail = "ops@usapainterp.com";

    private bool IsAuthorized() => Request.Cookies.TryGetValue(AuthCookie, out var token) && token == DemoEmail;

    // GET: Vendor
    public async Task<IActionResult> Index(string searchTerm = "", string filterStatus = "All", string filterType = "All", int? page = 1, int pageSize = 10)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var pageNumber = page ?? 1;

        var query = context.Vendors.AsQueryable();

        // Apply search
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(v =>
                v.BusinessName.Contains(searchTerm) ||
                v.VendorId.Contains(searchTerm) ||
                v.ContactEmail.Contains(searchTerm) ||
                v.VendorCategory.Contains(searchTerm));
        }

        // Apply status filter
        if (filterStatus == "Active")
        {
            query = query.Where(v => v.IsActive);
        }
        else if (filterStatus == "Inactive")
        {
            query = query.Where(v => !v.IsActive);
        }

        // Apply type filter
        if (filterType != "All")
        {
            query = query.Where(v => v.VendorType == filterType);
        }

        var vendorQuery = query
            .OrderBy(v => v.BusinessName)
            .Select(v => new VendorListItem
            {
                Id = v.Id,
                VendorId = v.VendorId,
                BusinessName = v.BusinessName,
                VendorType = v.VendorType,
                VendorCategory = v.VendorCategory,
                ContactEmail = v.ContactEmail,
                ContactPhone = v.ContactOfficePhone,
                BusinessCity = v.BusinessCity,
                BusinessState = v.BusinessState,
                IsActive = v.IsActive,
                OutstandingAmount = v.OutstandingAmount,
                VendorSince = v.VendorSince,
                LastPurchaseDate = v.LastPurchaseDate,
                VendorRating = v.VendorRating,
                Rating = v.VendorRating,
                IsPreferredVendor = v.IsPreferredVendor
            });

        var vendors = await PaginatedList<VendorListItem>.CreateAsync(vendorQuery, pageNumber, pageSize);

        var viewModel = new VendorListViewModel
        {
            Vendors = vendors,
            SearchTerm = searchTerm,
            FilterStatus = filterStatus,
            FilterType = filterType,
            TotalCount = context.Vendors.Count(),
            ActiveCount = context.Vendors.Count(v => v.IsActive),
            InactiveCount = context.Vendors.Count(v => !v.IsActive),
            TotalOutstanding = context.Vendors.Sum(v => v.OutstandingAmount)
        };

        return View(viewModel);
    }

    // GET: Vendor/Create
    public IActionResult Create()
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var model = new VendorFormViewModel
        {
            CompanyId = 1,
            VendorId = GenerateVendorId(),
            IsActive = true,
            VendorType = "Supplier",
            PaymentTerms = "Net 30",
            PreferredPaymentMethod = "Check",
            Currency = "USD",
            VendorSince = DateTime.UtcNow,
            VendorRating = 5
        };

        return View(model);
    }

    // POST: Vendor/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(VendorFormViewModel model)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Ensure CompanyId is valid
        if (model.CompanyId <= 0)
        {
            var firstCompany = context.Companies.FirstOrDefault();
            if (firstCompany != null)
            {
                model.CompanyId = firstCompany.Id;
            }
            else
            {
                ModelState.AddModelError("", "No company found. Please create a company first.");
                return View(model);
            }
        }

        var vendor = new Vendor
        {
            CompanyId = model.CompanyId,
            VendorId = model.VendorId,
            IsActive = model.IsActive,
            VendorType = model.VendorType,
            BusinessName = model.BusinessName,
            LegalBusinessName = model.LegalBusinessName,
            DBA = model.DBA,
            VendorCategory = model.VendorCategory,
            FederalTaxId = model.FederalTaxId,
            StateTaxId = model.StateTaxId,
            SalesTaxExemptionNumber = model.SalesTaxExemptionNumber,
            BusinessLicenseNumber = model.BusinessLicenseNumber,
            Industry = model.Industry,
            ContactFirstName = model.ContactFirstName,
            ContactLastName = model.ContactLastName,
            ContactJobTitle = model.ContactJobTitle,
            ContactEmail = model.ContactEmail,
            ContactMobilePhone = model.ContactMobilePhone,
            ContactOfficePhone = model.ContactOfficePhone,
            ContactExtension = model.ContactExtension,
            BusinessCountry = model.BusinessCountry,
            BusinessStreetAddress = model.BusinessStreetAddress,
            BusinessSuiteApt = model.BusinessSuiteApt,
            BusinessCity = model.BusinessCity,
            BusinessState = model.BusinessState,
            BusinessZipCode = model.BusinessZipCode,
            BusinessCounty = model.BusinessCounty,
            SameAsBusinessAddress = model.SameAsBusinessAddress,
            ShippingStreetAddress = model.ShippingStreetAddress,
            ShippingCity = model.ShippingCity,
            ShippingState = model.ShippingState,
            ShippingZipCode = model.ShippingZipCode,
            OpeningBalance = model.OpeningBalance,
            CreditLimit = model.CreditLimit,
            PaymentTerms = model.PaymentTerms,
            PreferredPaymentMethod = model.PreferredPaymentMethod,
            DefaultExpenseAccount = model.DefaultExpenseAccount,
            SalesTaxCode = model.SalesTaxCode,
            Is1099Vendor = model.Is1099Vendor,
            Currency = model.Currency,
            BankName = model.BankName,
            RoutingNumber = model.RoutingNumber,
            AccountNumber = model.AccountNumber,
            AccountType = model.AccountType,
            SwiftCode = model.SwiftCode,
            DefaultWarehouse = model.DefaultWarehouse,
            PreferredShippingMethod = model.PreferredShippingMethod,
            LeadTimeDays = model.LeadTimeDays,
            MinimumOrderQuantity = model.MinimumOrderQuantity,
            DefaultDiscountPercent = model.DefaultDiscountPercent,
            DefaultTaxRate = model.DefaultTaxRate,
            W9FormPath = model.W9FormPath,
            VendorContractPath = model.VendorContractPath,
            InsuranceCertificatePath = model.InsuranceCertificatePath,
            AdditionalDocumentsPath = model.AdditionalDocumentsPath,
            InternalNotes = model.InternalNotes,
            VendorSince = model.VendorSince,
            TotalPurchaseOrders = model.TotalPurchaseOrders,
            TotalBills = model.TotalBills,
            OutstandingAmount = model.OpeningBalance,
            LastPurchaseDate = model.LastPurchaseDate,
            AveragePaymentDays = model.AveragePaymentDays,
            VendorRating = model.VendorRating,
            IsPreferredVendor = model.IsPreferredVendor,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = DemoEmail,
            UpdatedBy = DemoEmail
        };

        context.Vendors.Add(vendor);
        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // GET: Vendor/Edit/5
    public IActionResult Edit(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var vendor = context.Vendors.Find(id);
        if (vendor == null)
        {
            return NotFound();
        }

        var model = new VendorFormViewModel
        {
            Id = vendor.Id,
            CompanyId = vendor.CompanyId,
            VendorId = vendor.VendorId,
            IsActive = vendor.IsActive,
            VendorType = vendor.VendorType,
            BusinessName = vendor.BusinessName,
            LegalBusinessName = vendor.LegalBusinessName,
            DBA = vendor.DBA,
            VendorCategory = vendor.VendorCategory,
            FederalTaxId = vendor.FederalTaxId,
            StateTaxId = vendor.StateTaxId,
            SalesTaxExemptionNumber = vendor.SalesTaxExemptionNumber,
            BusinessLicenseNumber = vendor.BusinessLicenseNumber,
            Industry = vendor.Industry,
            ContactFirstName = vendor.ContactFirstName,
            ContactLastName = vendor.ContactLastName,
            ContactJobTitle = vendor.ContactJobTitle,
            ContactEmail = vendor.ContactEmail,
            ContactMobilePhone = vendor.ContactMobilePhone,
            ContactOfficePhone = vendor.ContactOfficePhone,
            ContactExtension = vendor.ContactExtension,
            BusinessCountry = vendor.BusinessCountry,
            BusinessStreetAddress = vendor.BusinessStreetAddress,
            BusinessSuiteApt = vendor.BusinessSuiteApt,
            BusinessCity = vendor.BusinessCity,
            BusinessState = vendor.BusinessState,
            BusinessZipCode = vendor.BusinessZipCode,
            BusinessCounty = vendor.BusinessCounty,
            SameAsBusinessAddress = vendor.SameAsBusinessAddress,
            ShippingStreetAddress = vendor.ShippingStreetAddress,
            ShippingCity = vendor.ShippingCity,
            ShippingState = vendor.ShippingState,
            ShippingZipCode = vendor.ShippingZipCode,
            OpeningBalance = vendor.OpeningBalance,
            CreditLimit = vendor.CreditLimit,
            PaymentTerms = vendor.PaymentTerms,
            PreferredPaymentMethod = vendor.PreferredPaymentMethod,
            DefaultExpenseAccount = vendor.DefaultExpenseAccount,
            SalesTaxCode = vendor.SalesTaxCode,
            Is1099Vendor = vendor.Is1099Vendor,
            Currency = vendor.Currency,
            BankName = vendor.BankName,
            RoutingNumber = vendor.RoutingNumber,
            AccountNumber = vendor.AccountNumber,
            AccountType = vendor.AccountType,
            SwiftCode = vendor.SwiftCode,
            DefaultWarehouse = vendor.DefaultWarehouse,
            PreferredShippingMethod = vendor.PreferredShippingMethod,
            LeadTimeDays = vendor.LeadTimeDays,
            MinimumOrderQuantity = vendor.MinimumOrderQuantity,
            DefaultDiscountPercent = vendor.DefaultDiscountPercent,
            DefaultTaxRate = vendor.DefaultTaxRate,
            W9FormPath = vendor.W9FormPath,
            VendorContractPath = vendor.VendorContractPath,
            InsuranceCertificatePath = vendor.InsuranceCertificatePath,
            AdditionalDocumentsPath = vendor.AdditionalDocumentsPath,
            InternalNotes = vendor.InternalNotes,
            VendorSince = vendor.VendorSince,
            TotalPurchaseOrders = vendor.TotalPurchaseOrders,
            TotalBills = vendor.TotalBills,
            OutstandingAmount = vendor.OutstandingAmount,
            LastPurchaseDate = vendor.LastPurchaseDate,
            AveragePaymentDays = vendor.AveragePaymentDays,
            VendorRating = vendor.VendorRating,
            IsPreferredVendor = vendor.IsPreferredVendor
        };

        return View(model);
    }

    // POST: Vendor/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(VendorFormViewModel model)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var vendor = context.Vendors.Find(model.Id);
        if (vendor == null)
        {
            return NotFound();
        }

        vendor.VendorId = model.VendorId;
        vendor.IsActive = model.IsActive;
        vendor.VendorType = model.VendorType;
        vendor.BusinessName = model.BusinessName;
        vendor.LegalBusinessName = model.LegalBusinessName;
        vendor.DBA = model.DBA;
        vendor.VendorCategory = model.VendorCategory;
        vendor.FederalTaxId = model.FederalTaxId;
        vendor.StateTaxId = model.StateTaxId;
        vendor.SalesTaxExemptionNumber = model.SalesTaxExemptionNumber;
        vendor.BusinessLicenseNumber = model.BusinessLicenseNumber;
        vendor.Industry = model.Industry;
        vendor.ContactFirstName = model.ContactFirstName;
        vendor.ContactLastName = model.ContactLastName;
        vendor.ContactJobTitle = model.ContactJobTitle;
        vendor.ContactEmail = model.ContactEmail;
        vendor.ContactMobilePhone = model.ContactMobilePhone;
        vendor.ContactOfficePhone = model.ContactOfficePhone;
        vendor.ContactExtension = model.ContactExtension;
        vendor.BusinessCountry = model.BusinessCountry;
        vendor.BusinessStreetAddress = model.BusinessStreetAddress;
        vendor.BusinessSuiteApt = model.BusinessSuiteApt;
        vendor.BusinessCity = model.BusinessCity;
        vendor.BusinessState = model.BusinessState;
        vendor.BusinessZipCode = model.BusinessZipCode;
        vendor.BusinessCounty = vendor.BusinessCounty;
        vendor.SameAsBusinessAddress = model.SameAsBusinessAddress;
        vendor.ShippingStreetAddress = model.ShippingStreetAddress;
        vendor.ShippingCity = model.ShippingCity;
        vendor.ShippingState = model.ShippingState;
        vendor.ShippingZipCode = model.ShippingZipCode;
        vendor.OpeningBalance = model.OpeningBalance;
        vendor.OutstandingAmount = model.OutstandingAmount;
        vendor.CreditLimit = model.CreditLimit;
        vendor.PaymentTerms = model.PaymentTerms;
        vendor.PreferredPaymentMethod = model.PreferredPaymentMethod;
        vendor.DefaultExpenseAccount = model.DefaultExpenseAccount;
        vendor.SalesTaxCode = model.SalesTaxCode;
        vendor.Is1099Vendor = model.Is1099Vendor;
        vendor.Currency = model.Currency;
        vendor.BankName = model.BankName;
        vendor.RoutingNumber = model.RoutingNumber;
        vendor.AccountNumber = model.AccountNumber;
        vendor.AccountType = model.AccountType;
        vendor.SwiftCode = model.SwiftCode;
        vendor.DefaultWarehouse = model.DefaultWarehouse;
        vendor.PreferredShippingMethod = model.PreferredShippingMethod;
        vendor.LeadTimeDays = model.LeadTimeDays;
        vendor.MinimumOrderQuantity = model.MinimumOrderQuantity;
        vendor.DefaultDiscountPercent = model.DefaultDiscountPercent;
        vendor.DefaultTaxRate = model.DefaultTaxRate;
        vendor.W9FormPath = model.W9FormPath;
        vendor.VendorContractPath = model.VendorContractPath;
        vendor.InsuranceCertificatePath = model.InsuranceCertificatePath;
        vendor.AdditionalDocumentsPath = model.AdditionalDocumentsPath;
        vendor.InternalNotes = model.InternalNotes;
        vendor.VendorRating = model.VendorRating;
        vendor.IsPreferredVendor = model.IsPreferredVendor;
        vendor.UpdatedAtUtc = DateTime.UtcNow;
        vendor.UpdatedBy = DemoEmail;

        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // GET: Vendor/Delete/5
    public IActionResult Delete(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var vendor = context.Vendors.Find(id);
        if (vendor == null)
        {
            return NotFound();
        }

        return View(vendor);
    }

    // POST: Vendor/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var vendor = context.Vendors.Find(id);
        if (vendor != null)
        {
            context.Vendors.Remove(vendor);
            context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

    private string GenerateVendorId()
    {
        var prefix = "VND";
        var year = DateTime.UtcNow.Year.ToString().Substring(2);
        var count = context.Vendors.Count() + 1;
        return $"{prefix}{year}-{count:D4}";
    }
}
