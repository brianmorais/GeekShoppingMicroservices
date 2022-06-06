using System.Net.Http.Headers;
using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        private const string BasePath = "api/v1/product";

        public ProductService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ProductModel> Create(string token, ProductModel product)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsJson(BasePath, product);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProductModel>();
            else 
                throw new Exception("Somthing went wrong when calling API");
        }

        public async Task<bool> Delete(string token, long id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.DeleteAsync($"{BasePath}/{id}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else 
                throw new Exception("Somthing went wrong when calling API");
        }

        public async Task<IEnumerable<ProductModel>> FindAll()
        {
            var response = await _client.GetAsync(BasePath);
            return await response.ReadContentAs<List<ProductModel>>();
        }

        public async Task<ProductModel> FindById(string token, long id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContentAs<ProductModel>();
        }

        public async Task<ProductModel> Update(string token, ProductModel product)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PutAsJson(BasePath, product);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProductModel>();
            else 
                throw new Exception("Somthing went wrong when calling API");
        }
    }
}