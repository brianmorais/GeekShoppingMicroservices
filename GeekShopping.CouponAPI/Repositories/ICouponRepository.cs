using GeekShopping.CouponAPI.DTOs;

namespace GeekShopping.CouponAPI.Repositories
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCouponCode(string couponCode);
    }
}
