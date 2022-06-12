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
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    public async Task <IActionResult> Index()
    {
        var products = await _productService.FindAll();
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var product = await _productService.FindById(token, id);
        return View(product);
    }

    [HttpPost]
    [Authorize]
    [ActionName("Details")]
    public async Task<IActionResult> DetailsPost(ProductViewModel product)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var cart = new CartViewModel
        {
            CartHeader = new CartHeaderViewModel
            {
                UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
            }
        };

        var cartDetail = new CartDetailViewModel
        {
            Count = product.Count,
            ProductId = product.Id,
            Product = await _productService.FindById(token, product.Id)
        };

        var cartDetails = new List<CartDetailViewModel>();
        cartDetails.Add(cartDetail);
        cart.CartDetails = cartDetails;

        var response = await _cartService.AddItemToCart(token, cart);

        if (response != null)
            return RedirectToAction(nameof(Index));
        
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
