using Microsoft.AspNetCore.Mvc;
using GeekShopping.Web.Services.Interfaces;

namespace GeekShopping.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<IActionResult> Index()
    {
        var products = await _service.FindAll();
        return View(products);
    }
}