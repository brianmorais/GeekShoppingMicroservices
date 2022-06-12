using Microsoft.AspNetCore.Mvc;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Models;
using Microsoft.AspNetCore.Authorization;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;

namespace GeekShopping.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var products = await _service.FindAll();
        return View(products);
    }

    public IActionResult ProductCreate()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductViewModel product)
    {
        if (ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _service.Create(token, product);
            if (response != null)
                return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    public async Task<IActionResult> ProductUpdate(long id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var product = await _service.FindById(token, id);
        if (product != null)
            return View(product);

        return NotFound();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ProductUpdate(ProductViewModel product)
    {
        if (ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _service.Update(token, product);
            if (response != null)
                return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    [Authorize]
    public async Task<IActionResult> ProductDelete(long id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var product = await _service.FindById(token, id);
        if (product != null)
            return View(product);

        return NotFound();
    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> ProductDelete(ProductViewModel product)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _service.Delete(token, product.Id);
        if (response)
            return RedirectToAction(nameof(Index));
        
        return View(product);
    }
}