using GeekShopping.CartAPI.DTOs;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.CartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly HttpClient _httpClient;
        private const string BasePath = "api/v1/coupons";

        public CouponService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CouponDTO> GetCoupon(string token, string couponCode)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{BasePath}/{couponCode}");

            if (response.StatusCode != HttpStatusCode.OK)
                return new CouponDTO();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CouponDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
