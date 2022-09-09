using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartViewModel> FindCartByUserId(string token, string userId);
        Task<CartViewModel> AddItemToCart(string token, CartViewModel cart);
        Task<CartViewModel> UpdateCart(string token, CartViewModel cart);
        Task<bool> RemoveFromCart(string token, long cartId);
        Task<bool> ApplyCoupon(string token, CartViewModel cart);
        Task<bool> RemoveCoupon(string token, string userId);
        Task<object> Checkout(string token, CartHeaderViewModel cartHeader);
    }
}
