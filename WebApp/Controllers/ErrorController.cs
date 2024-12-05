using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class ErrorController : Controller

{
    public IActionResult Error404(string message)
    {
        ViewBag.Message = message;
        return View();
    }

    public IActionResult Error401(string message)
    {
        ViewBag.Message = message;
        return View();
    }

    public IActionResult Error500(string message)
    {
        ViewBag.Message = message;
        return View();
    }
}
