using Microsoft.AspNetCore.Mvc;
using PaintERP.Services;

namespace PaintERP.Helpers;

public static class ControllerExtensions
{
    public static IActionResult WithSuccess(this Controller controller, string message, string actionName = "Index")
    {
        controller.TempData["SuccessMessage"] = message;
        return controller.RedirectToAction(actionName);
    }

    public static IActionResult WithSuccess(this Controller controller, string message, string actionName, object routeValues)
    {
        controller.TempData["SuccessMessage"] = message;
        return controller.RedirectToAction(actionName, routeValues);
    }

    public static IActionResult WithError(this Controller controller, string message, string actionName = "Index")
    {
        controller.TempData["ErrorMessage"] = message;
        return controller.RedirectToAction(actionName);
    }

    public static IActionResult WithWarning(this Controller controller, string message, string actionName = "Index")
    {
        controller.TempData["WarningMessage"] = message;
        return controller.RedirectToAction(actionName);
    }

    public static IActionResult WithInfo(this Controller controller, string message, string actionName = "Index")
    {
        controller.TempData["InfoMessage"] = message;
        return controller.RedirectToAction(actionName);
    }

    public static IActionResult WithValidationErrors(this Controller controller, ValidationResult validationResult, object model = null)
    {
        foreach (var error in validationResult.Errors)
        {
            controller.ModelState.AddModelError(string.Empty, error);
        }

        foreach (var warning in validationResult.Warnings)
        {
            controller.ModelState.AddModelError(string.Empty, $"Warning: {warning}");
        }

        return model != null ? controller.View(model) : controller.View();
    }

    public static void AddSuccessMessage(this Controller controller, string message)
    {
        controller.TempData["SuccessMessage"] = message;
    }

    public static void AddErrorMessage(this Controller controller, string message)
    {
        controller.TempData["ErrorMessage"] = message;
    }

    public static void AddWarningMessage(this Controller controller, string message)
    {
        controller.TempData["WarningMessage"] = message;
    }

    public static void AddInfoMessage(this Controller controller, string message)
    {
        controller.TempData["InfoMessage"] = message;
    }
}
