using AutoMapper;
using GeekShopping.CouponAPI.DTOs;
using GeekShopping.CouponAPI.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly MySqlContext _context;
        private readonly IMapper _mapper;

        public CouponRepository(IMapper mapper, MySqlContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CouponDTO> GetCouponByCouponCode(string couponCode)
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.CouponCode == couponCode);

            if (coupon != null)
                return _mapper.Map<CouponDTO>(coupon);

            return null;
        }
    }
}
