using AutoMapper;
using GeekShopping.CouponAPI.DTOs;
using GeekShopping.CouponAPI.Models;

namespace GeekShopping.CouponAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMapps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDTO>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
