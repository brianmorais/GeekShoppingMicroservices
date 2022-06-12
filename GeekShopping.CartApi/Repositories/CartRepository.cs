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
            var cartHeader = await _context.CartHeaders
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cartHeader != null)
            {
                var cartDetailsToRemove = _context.CartDetails
                    .Where(c => c.CartHeaderId == cartHeader.Id);

                _context.CartDetails.RemoveRange(cartDetailsToRemove);
                _context.CartHeaders.Remove(cartHeader);

                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<CartDTO> FindCartByUserId(string userId)
        {
            var cart = new Cart();
            cart.CartHeader = await _context.CartHeaders
                .FirstOrDefaultAsync(c => c.UserId == userId);

            cart.CartDetails = _context.CartDetails
                .Where(c => c.CartHeaderId == cart.CartHeader.Id)
                .Include(c => c.Product);            

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveFromCart(long cartDetailsId)
        {
            try
            {
                var cartDetails = await _context.CartDetails
                    .FirstOrDefaultAsync(c => c.Id == cartDetailsId);

                var total = _context.CartDetails
                    .Where(c => c.CartHeaderId == cartDetails.CartHeaderId)
                    .Count();

                _context.CartDetails.Remove(cartDetails);

                if (total == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders
                        .FirstOrDefaultAsync(c => c.Id == cartDetails.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<CartDTO> SaveOrUpdateCart(CartDTO cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == cartDto.CartDetails
                .FirstOrDefault().ProductId);

            if (product == null)
            {
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }

            var cartHeader = await _context.CartHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

            if (cartHeader == null)
            {
                _context.CartHeaders.Add(cart.CartHeader);
                await _context.SaveChangesAsync();

                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                var cartDetail = await _context.CartDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.ProductId == cart.CartDetails
                    .FirstOrDefault().ProductId 
                        && c.CartHeaderId == cartHeader.Id);

                if (cartDetail == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
                    cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;
                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDTO>(cart);
        }
    }
}
