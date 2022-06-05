using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> FindAll(string token);
        Task<ProductModel> FindById(string token, long id);
        Task<ProductModel> Create(string token, ProductModel product);
        Task<ProductModel> Update(string token, ProductModel product);
        Task<bool> Delete(string token, long id);
    }
}