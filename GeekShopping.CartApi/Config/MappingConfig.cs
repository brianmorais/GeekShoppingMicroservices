using AutoMapper;
using GeekShopping.CartApi.DTOs;
using GeekShopping.CartApi.Models;

namespace GeekShopping.CartApi.Config
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