using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using PaintERP.Models;

using PaintERP.Models.ViewModels;

using PaintERP.Services;



namespace PaintERP.Controllers;



public class HomeController(IDashboardService dashboardService) : Controller

{

    private const string DemoEmail = "ops@usapainterp.com";

    private const string DemoPassword = "USA-Ready24!";

    private const string AuthCookie = "PaintErpAuth";



    public IActionResult Index()

    {

        return RedirectToAction(nameof(Login));

    }



    public IActionResult Login()

    {

        if (IsAuthorized())

        {

            return RedirectToAction(nameof(Dashboard));

        }



        var model = new LoginViewModel

        {

            Email = DemoEmail,

            KeepSignedIn = true

        };



        return View(model);

    }



    [HttpPost]

    [ValidateAntiForgeryToken]

    public IActionResult Login(LoginViewModel model)

    {

        if (!ModelState.IsValid)

        {

            return View(model);

        }



        if (!string.Equals(model.Email?.Trim(), DemoEmail, StringComparison.OrdinalIgnoreCase)

            || model.Password != DemoPassword)

        {

            ModelState.AddModelError(string.Empty, "Invalid email or password. Use the provided PaintERP demo credentials.");

            return View(model);

        }



        var cookieOptions = new CookieOptions

        {

            HttpOnly = true,

            IsEssential = true,

            Expires = DateTimeOffset.UtcNow.AddHours(model.KeepSignedIn ? 24 : 2)

        };

        Response.Cookies.Append(AuthCookie, DemoEmail, cookieOptions);



        return RedirectToAction(nameof(Dashboard));

    }



    public IActionResult Logout()

    {

        Response.Cookies.Delete(AuthCookie);

        return RedirectToAction(nameof(Login));

    }



    public async Task<IActionResult> Dashboard()

    {

        if (!IsAuthorized())

        {

            return RedirectToAction(nameof(Login));

        }



        var viewModel = await dashboardService.BuildExecutiveDashboardAsync();

        return View(viewModel);

    }



    public IActionResult UserManagement()

    {

        if (!IsAuthorized())

        {

            return RedirectToAction(nameof(Login));

        }



        var viewModel = UserManagementViewModel.BuildDemo();

        return View(viewModel);

    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Error()

    {

        return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });

    }



    private bool IsAuthorized() => Request.Cookies.TryGetValue(AuthCookie, out var token) && token == DemoEmail;

}

