using GeekShopping.CartAPI.DTOs;

namespace GeekShopping.CartAPI.Repositories
{
    public interface ICartRepository
    {
        Task<CartDTO> FindCartByUserId(string userId);
        Task<CartDTO> SaveOrUpdateCart(CartDTO cartDto);
        Task<bool> RemoveFromCart(long cartDetailsId);
        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
