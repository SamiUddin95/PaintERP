using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintERP.Data;
using PaintERP.Models.Entities;

namespace PaintERP.Controllers;

public class WarehouseController(PaintErpDbContext context) : Controller
{
    private const string AuthCookie = "PaintErpAuth";
    private const string DemoEmail = "ops@usapainterp.com";

    private bool IsAuthorized() => Request.Cookies.TryGetValue(AuthCookie, out var token) && token == DemoEmail;

    // GET: Warehouse
    public IActionResult Index()
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var warehouses = context.Warehouses
            .Include(w => w.Company)
            .OrderBy(w => w.Name)
            .ToList();

        return View(warehouses);
    }

    // GET: Warehouse/Create
    public IActionResult Create()
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var companies = context.Companies.ToList();
        ViewBag.Companies = companies;
        return View();
    }

    // POST: Warehouse/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Warehouse warehouse)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        if (!ModelState.IsValid)
        {
            var companies = context.Companies.ToList();
            ViewBag.Companies = companies;
            return View(warehouse);
        }

        context.Warehouses.Add(warehouse);
        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // GET: Warehouse/Edit/5
    public IActionResult Edit(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var warehouse = context.Warehouses.Find(id);
        if (warehouse == null)
        {
            return NotFound();
        }

        var companies = context.Companies.ToList();
        ViewBag.Companies = companies;
        return View(warehouse);
    }

    // POST: Warehouse/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Warehouse warehouse)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        if (id != warehouse.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            var companies = context.Companies.ToList();
            ViewBag.Companies = companies;
            return View(warehouse);
        }

        context.Update(warehouse);
        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // GET: Warehouse/Delete/5
    public IActionResult Delete(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var warehouse = context.Warehouses
            .Include(w => w.Company)
            .FirstOrDefault(w => w.Id == id);

        if (warehouse == null)
        {
            return NotFound();
        }

        return View(warehouse);
    }

    // POST: Warehouse/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!IsAuthorized())
        {
            return RedirectToAction("Login", "Home");
        }

        var warehouse = context.Warehouses.Find(id);
        if (warehouse == null)
        {
            return NotFound();
        }

        context.Warehouses.Remove(warehouse);
        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
