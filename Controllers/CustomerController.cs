using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;
using PaintERP.Models.ViewModels;

namespace PaintERP.Controllers;

public class CustomerController(PaintErpDbContext context) : Controller
{
    private const string AuthCookie = "PaintErpAuth";
    private const string DemoEmail = "ops@usapainterp.com";

    private bool IsAuthorized() => Request.Cookies.TryGetValue(AuthCookie, out var token) && token == DemoEmail;

    // GET: Customer
    public async Task<IActionResult> Index(string searchTerm = "", string filterStatus = "All", string filterType = "All", int? page = 1, int pageSize = 10)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var pageNumber = page ?? 1;

        var query = context.Customers.AsQueryable();

        // Apply search
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c =>
                c.BusinessName.Contains(searchTerm) ||
                c.CustomerId.Contains(searchTerm) ||
                c.ContactEmail.Contains(searchTerm) ||
                c.Industry.Contains(searchTerm));
        }

        // Apply status filter
        if (filterStatus == "Active")
        {
            query = query.Where(c => c.IsActive);
        }
        else if (filterStatus == "Inactive")
        {
            query = query.Where(c => !c.IsActive);
        }

        // Apply type filter
        if (filterType != "All")
        {
            query = query.Where(c => c.CustomerType == filterType);
        }

        var customerQuery = query
            .OrderBy(c => c.BusinessName)
            .Select(c => new CustomerListItem
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                BusinessName = c.BusinessName,
                CustomerType = c.CustomerType,
                Industry = c.Industry,
                ContactEmail = c.ContactEmail,
                ContactPhone = c.ContactOfficePhone,
                BillingCity = c.BillingCity,
                BillingState = c.BillingState,
                IsActive = c.IsActive,
                OutstandingBalance = c.OutstandingBalance,
                LifetimeRevenue = c.LifetimeRevenue,
                CustomerSince = c.CustomerSince,
                LastInvoiceDate = c.LastInvoiceDate,
                CustomerRating = c.CustomerRating,
                Rating = c.CustomerRating,
                IsVIP = c.IsVIP
            });

        var customers = await PaginatedList<CustomerListItem>.CreateAsync(customerQuery, pageNumber, pageSize);

        var viewModel = new CustomerListViewModel
        {
            Customers = customers,
            SearchTerm = searchTerm,
            FilterStatus = filterStatus,
            FilterType = filterType,
            TotalCount = context.Customers.Count(),
            ActiveCount = context.Customers.Count(c => c.IsActive),
            InactiveCount = context.Customers.Count(c => !c.IsActive),
            TotalOutstanding = context.Customers.Sum(c => c.OutstandingBalance),
            TotalLifetimeRevenue = context.Customers.Sum(c => c.LifetimeRevenue)
        };

        return View(viewModel);
    }

    // GET: Customer/Create
    public IActionResult Create()
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var model = new CustomerFormViewModel
        {
            CompanyId = 1,
            CustomerId = GenerateCustomerId(),
            IsActive = true,
            CustomerType = "Commercial",
            PaymentTerms = "Net 30",
            PreferredPaymentMethod = "Check",
            Currency = "USD",
            CustomerSince = DateTime.UtcNow,
            CustomerRating = 5
        };

        return View(model);
    }

    // POST: Customer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CustomerFormViewModel model)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var customer = new Customer
        {
            CompanyId = model.CompanyId,
            CustomerId = model.CustomerId,
            IsActive = model.IsActive,
            CustomerType = model.CustomerType,
            BusinessName = model.BusinessName,
            LegalCompanyName = model.LegalCompanyName,
            DBA = model.DBA,
            FederalTaxId = model.FederalTaxId,
            ResaleCertificateNumber = model.ResaleCertificateNumber,
            Industry = model.Industry,
            CustomerGroup = model.CustomerGroup,
            SalesRepresentative = model.SalesRepresentative,
            ContactFirstName = model.ContactFirstName,
            ContactLastName = model.ContactLastName,
            ContactTitle = model.ContactTitle,
            ContactEmail = model.ContactEmail,
            ContactMobile = model.ContactMobile,
            ContactOfficePhone = model.ContactOfficePhone,
            ContactExtension = model.ContactExtension,
            BillingCountry = model.BillingCountry,
            BillingStreetAddress = model.BillingStreetAddress,
            BillingSuite = model.BillingSuite,
            BillingCity = model.BillingCity,
            BillingState = model.BillingState,
            BillingZipCode = model.BillingZipCode,
            BillingCounty = model.BillingCounty,
            SameAsBillingAddress = model.SameAsBillingAddress,
            ShippingStreetAddress = model.ShippingStreetAddress,
            ShippingCity = model.ShippingCity,
            ShippingState = model.ShippingState,
            ShippingZipCode = model.ShippingZipCode,
            OpeningBalance = model.OpeningBalance,
            CreditLimit = model.CreditLimit,
            PaymentTerms = model.PaymentTerms,
            PreferredPaymentMethod = model.PreferredPaymentMethod,
            SalesTaxCode = model.SalesTaxCode,
            IsTaxExempt = model.IsTaxExempt,
            Currency = model.Currency,
            DefaultPriceList = model.DefaultPriceList,
            CustomerDiscountPercent = model.CustomerDiscountPercent,
            PreferredWarehouse = model.PreferredWarehouse,
            DeliveryMethod = model.DeliveryMethod,
            DeliveryInstructions = model.DeliveryInstructions,
            ReceivingHours = model.ReceivingHours,
            LoadingDockInfo = model.LoadingDockInfo,
            PreferredPaintBrand = model.PreferredPaintBrand,
            PreferredColorCodes = model.PreferredColorCodes,
            FavoriteProducts = model.FavoriteProducts,
            ProjectType = model.ProjectType,
            ResaleCertificatePath = model.ResaleCertificatePath,
            ContractPath = model.ContractPath,
            CreditApplicationPath = model.CreditApplicationPath,
            OtherDocumentsPath = model.OtherDocumentsPath,
            InternalNotes = model.InternalNotes,
            CustomerSince = model.CustomerSince,
            TotalSales = model.TotalSales,
            OutstandingBalance = model.OutstandingBalance,
            LastInvoiceDate = model.LastInvoiceDate,
            LastPaymentDate = model.LastPaymentDate,
            LifetimeRevenue = model.LifetimeRevenue,
            AverageOrderValue = model.AverageOrderValue,
            CustomerRating = model.CustomerRating,
            IsVIP = model.IsVIP,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            CreatedBy = DemoEmail,
            UpdatedBy = DemoEmail
        };

        context.Customers.Add(customer);
        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // GET: Customer/Edit/5
    public IActionResult Edit(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var customer = context.Customers.Find(id);
        if (customer == null)
        {
            return NotFound();
        }

        var model = new CustomerFormViewModel
        {
            Id = customer.Id,
            CompanyId = customer.CompanyId,
            CustomerId = customer.CustomerId,
            IsActive = customer.IsActive,
            CustomerType = customer.CustomerType,
            BusinessName = customer.BusinessName,
            LegalCompanyName = customer.LegalCompanyName,
            DBA = customer.DBA,
            FederalTaxId = customer.FederalTaxId,
            ResaleCertificateNumber = customer.ResaleCertificateNumber,
            Industry = customer.Industry,
            CustomerGroup = customer.CustomerGroup,
            SalesRepresentative = customer.SalesRepresentative,
            ContactFirstName = customer.ContactFirstName,
            ContactLastName = customer.ContactLastName,
            ContactTitle = customer.ContactTitle,
            ContactEmail = customer.ContactEmail,
            ContactMobile = customer.ContactMobile,
            ContactOfficePhone = customer.ContactOfficePhone,
            ContactExtension = customer.ContactExtension,
            BillingCountry = customer.BillingCountry,
            BillingStreetAddress = customer.BillingStreetAddress,
            BillingSuite = customer.BillingSuite,
            BillingCity = customer.BillingCity,
            BillingState = customer.BillingState,
            BillingZipCode = customer.BillingZipCode,
            BillingCounty = customer.BillingCounty,
            SameAsBillingAddress = customer.SameAsBillingAddress,
            ShippingStreetAddress = customer.ShippingStreetAddress,
            ShippingCity = customer.ShippingCity,
            ShippingState = customer.ShippingState,
            ShippingZipCode = customer.ShippingZipCode,
            OpeningBalance = customer.OpeningBalance,
            CreditLimit = customer.CreditLimit,
            PaymentTerms = customer.PaymentTerms,
            PreferredPaymentMethod = customer.PreferredPaymentMethod,
            SalesTaxCode = customer.SalesTaxCode,
            IsTaxExempt = customer.IsTaxExempt,
            Currency = customer.Currency,
            DefaultPriceList = customer.DefaultPriceList,
            CustomerDiscountPercent = customer.CustomerDiscountPercent,
            PreferredWarehouse = customer.PreferredWarehouse,
            DeliveryMethod = customer.DeliveryMethod,
            DeliveryInstructions = customer.DeliveryInstructions,
            ReceivingHours = customer.ReceivingHours,
            LoadingDockInfo = customer.LoadingDockInfo,
            PreferredPaintBrand = customer.PreferredPaintBrand,
            PreferredColorCodes = customer.PreferredColorCodes,
            FavoriteProducts = customer.FavoriteProducts,
            ProjectType = customer.ProjectType,
            ResaleCertificatePath = customer.ResaleCertificatePath,
            ContractPath = customer.ContractPath,
            CreditApplicationPath = customer.CreditApplicationPath,
            OtherDocumentsPath = customer.OtherDocumentsPath,
            InternalNotes = customer.InternalNotes,
            CustomerSince = customer.CustomerSince,
            TotalSales = customer.TotalSales,
            OutstandingBalance = customer.OutstandingBalance,
            LastInvoiceDate = customer.LastInvoiceDate,
            LastPaymentDate = customer.LastPaymentDate,
            LifetimeRevenue = customer.LifetimeRevenue,
            AverageOrderValue = customer.AverageOrderValue,
            CustomerRating = customer.CustomerRating,
            IsVIP = customer.IsVIP
        };

        return View(model);
    }

    // POST: Customer/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CustomerFormViewModel model)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var customer = context.Customers.Find(model.Id);
        if (customer == null)
        {
            return NotFound();
        }

        customer.CustomerId = model.CustomerId;
        customer.IsActive = model.IsActive;
        customer.CustomerType = model.CustomerType;
        customer.BusinessName = model.BusinessName;
        customer.LegalCompanyName = model.LegalCompanyName;
        customer.DBA = model.DBA;
        customer.FederalTaxId = model.FederalTaxId;
        customer.ResaleCertificateNumber = model.ResaleCertificateNumber;
        customer.Industry = model.Industry;
        customer.CustomerGroup = model.CustomerGroup;
        customer.SalesRepresentative = model.SalesRepresentative;
        customer.ContactFirstName = model.ContactFirstName;
        customer.ContactLastName = model.ContactLastName;
        customer.ContactTitle = model.ContactTitle;
        customer.ContactEmail = model.ContactEmail;
        customer.ContactMobile = model.ContactMobile;
        customer.ContactOfficePhone = model.ContactOfficePhone;
        customer.ContactExtension = model.ContactExtension;
        customer.BillingCountry = model.BillingCountry;
        customer.BillingStreetAddress = model.BillingStreetAddress;
        customer.BillingSuite = model.BillingSuite;
        customer.BillingCity = model.BillingCity;
        customer.BillingState = model.BillingState;
        customer.BillingZipCode = model.BillingZipCode;
        customer.BillingCounty = model.BillingCounty;
        customer.SameAsBillingAddress = model.SameAsBillingAddress;
        customer.ShippingStreetAddress = model.ShippingStreetAddress;
        customer.ShippingCity = model.ShippingCity;
        customer.ShippingState = model.ShippingState;
        customer.ShippingZipCode = model.ShippingZipCode;
        customer.OpeningBalance = model.OpeningBalance;
        customer.CreditLimit = model.CreditLimit;
        customer.PaymentTerms = model.PaymentTerms;
        customer.PreferredPaymentMethod = model.PreferredPaymentMethod;
        customer.SalesTaxCode = model.SalesTaxCode;
        customer.IsTaxExempt = model.IsTaxExempt;
        customer.Currency = model.Currency;
        customer.DefaultPriceList = model.DefaultPriceList;
        customer.CustomerDiscountPercent = model.CustomerDiscountPercent;
        customer.PreferredWarehouse = model.PreferredWarehouse;
        customer.DeliveryMethod = model.DeliveryMethod;
        customer.DeliveryInstructions = model.DeliveryInstructions;
        customer.ReceivingHours = model.ReceivingHours;
        customer.LoadingDockInfo = model.LoadingDockInfo;
        customer.PreferredPaintBrand = model.PreferredPaintBrand;
        customer.PreferredColorCodes = model.PreferredColorCodes;
        customer.FavoriteProducts = model.FavoriteProducts;
        customer.ProjectType = model.ProjectType;
        customer.ResaleCertificatePath = model.ResaleCertificatePath;
        customer.ContractPath = model.ContractPath;
        customer.CreditApplicationPath = model.CreditApplicationPath;
        customer.OtherDocumentsPath = model.OtherDocumentsPath;
        customer.InternalNotes = model.InternalNotes;
        customer.CustomerRating = model.CustomerRating;
        customer.IsVIP = model.IsVIP;
        customer.UpdatedAtUtc = DateTime.UtcNow;
        customer.UpdatedBy = DemoEmail;

        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // GET: Customer/Delete/5
    public IActionResult Delete(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var customer = context.Customers.Find(id);
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    // POST: Customer/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var customer = context.Customers.Find(id);
        if (customer != null)
        {
            context.Customers.Remove(customer);
            context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

    private string GenerateCustomerId()
    {
        var prefix = "CST";
        var year = DateTime.UtcNow.Year.ToString().Substring(2);
        var count = context.Customers.Count() + 1;
        return $"{prefix}{year}-{count:D4}";
    }
}
