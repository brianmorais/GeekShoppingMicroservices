using GeekShopping.ProductAPI.DTOs;

namespace GeekShopping.ProductAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> FindAll();
        Task<ProductDTO> FindById(long id);
        Task<ProductDTO> Create(ProductDTO productDto);
        Task<ProductDTO> Update(ProductDTO productDto);
        Task<bool> Delete(long id);
    }
}