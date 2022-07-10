using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        public CartController(ICartService cartService, IProductService productService, ICouponService couponService)
        {
            _cartService = cartService;
            _productService = productService;
            _couponService = couponService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userCart = await FindUserCart();
            return View(userCart);
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartViewModel model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            var response = await _cartService.ApplyCoupon(token, model);

            if (response)
                return RedirectToAction(nameof(Index));

            return View();
        }

        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            var response = await _cartService.RemoveCoupon(token, userId);

            if (response)
                return RedirectToAction(nameof(Index));

            return View();
        }

        public async Task<IActionResult> Remove(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            var response = await _cartService.RemoveFromCart(token, id);

            if (response)
                return RedirectToAction(nameof(Index));

            return View();
        }

        private async Task<CartViewModel> FindUserCart()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

            var response = await _cartService.FindCartByUserId(token, userId);

            if (response?.CartHeader != null)
            {
                if (!string.IsNullOrEmpty(response.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon(token, response.CartHeader.CouponCode);

                    if (coupon?.CouponCode != null)
                        response.CartHeader.DiscountAmount = coupon.DiscountAmount;
                }

                foreach (var detail in response.CartDetails)
                {
                    response.CartHeader.PurchaseAmount += detail.Product.Price * detail.Count;
                }

                response.CartHeader.PurchaseAmount -= response.CartHeader.DiscountAmount;
            }

            return response;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userCart = await FindUserCart();
            return View(userCart);
        }
    }
}
