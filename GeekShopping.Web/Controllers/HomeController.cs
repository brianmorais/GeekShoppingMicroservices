using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShopping.Web.Models;
using Microsoft.AspNetCore.Authorization;
using GeekShopping.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace GeekShopping.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _service;

    public HomeController(ILogger<HomeController> logger, IProductService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task <IActionResult> Index()
    {
        var products = await _service.FindAll();
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token") ?? "";
        var product = await _service.FindById(token, id);
        return View(product);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public IActionResult Login()
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
}
