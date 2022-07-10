using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponViewModel> GetCoupon(string token, string code);
    }
}
