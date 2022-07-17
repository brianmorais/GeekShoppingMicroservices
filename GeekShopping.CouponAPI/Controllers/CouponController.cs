using GeekShopping.CouponAPI.DTOs;
using GeekShopping.CouponAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/v1/coupons")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _repository;

        public CouponController(ICouponRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{couponCode}")]
        [Authorize]
        public async Task<ActionResult<CouponDTO>> FindById(string couponCode)
        {
            var token = Request.Headers["Authorization"];
            var product = await _repository.GetCouponByCouponCode(couponCode);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}