using AutoMapper;
using GeekShopping.ProductAPI.DTOs;
using GeekShopping.ProductAPI.Models;
using GeekShopping.ProductAPI.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySqlContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(IMapper mapper, MySqlContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ProductDTO> Create(ProductDTO productDto)
        {
            var product = _mapper.Map<ProductDTO, Product>(productDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<Product, ProductDTO>(product);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product == null)
                    return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductDTO>> FindAll()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> FindById(long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> Update(ProductDTO productDto)
        {
            var product = _mapper.Map<ProductDTO, Product>(productDto);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<Product, ProductDTO>(product);
        }
    }
}