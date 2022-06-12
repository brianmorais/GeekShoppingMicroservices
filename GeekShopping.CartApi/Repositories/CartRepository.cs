using AutoMapper;
using GeekShopping.CartApi.DTOs;
using GeekShopping.CartApi.Models;
using GeekShopping.CartApi.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartApi.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly MySqlContext _context;
        private readonly IMapper _mapper;

        public CartRepository(IMapper mapper, MySqlContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<CartDTO> FindCartByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveFromCart(long cartDetailsId)
        {
            throw new NotImplementedException();
        }

        public async Task<CartDTO> SaveOrUpdateCart(CartDTO cartDto)
        {
            throw new NotImplementedException();
        }
    }
}
