using GeekShopping.CartAPI.DTOs;

namespace GeekShopping.CartAPI.Services
{
    public interface ICouponService
    {
        Task<CouponDTO> GetCoupon(string token, string couponCode);
    }
}
