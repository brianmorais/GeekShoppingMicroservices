using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> FindAll();
        Task<ProductViewModel> FindById(string token, long id);
        Task<ProductViewModel> Create(string token, ProductViewModel product);
        Task<ProductViewModel> Update(string token, ProductViewModel product);
        Task<bool> Delete(string token, long id);
    }
}