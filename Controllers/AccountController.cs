using Microsoft.AspNetCore.Mvc;

namespace PaintERP.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ActionName("Login")]
    public IActionResult LoginPost(string returnUrl = null)
    {
        // TODO: hook up authentication
        if (!string.IsNullOrEmpty(returnUrl))
            return LocalRedirect(returnUrl);
        return RedirectToAction("Dashboard", "Home");
    }
}
