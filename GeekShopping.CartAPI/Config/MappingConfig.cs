using AutoMapper;
using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Models;

namespace GeekShopping.CartAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMapps()
        {
            var mappingConfig = new MapperConfiguration(config => 
            {
                config.CreateMap<Product, ProductDTO>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
                config.CreateMap<Cart, CartDTO>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailDTO>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}