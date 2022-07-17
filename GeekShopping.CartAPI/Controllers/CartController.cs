using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repositories;
using GeekShopping.CartAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers;

[ApiController]
[Route("api/v1/carts")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private readonly IRabbitMQMessageSender _rabbitMQMessageSender;
    private readonly ICouponService _couponService;

    public CartController(ICartRepository repository, IRabbitMQMessageSender rabbitMQMessageSender, ICouponService couponService)
    {
        _cartRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
        _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
    }

    [HttpGet("find-cart/{id}")]
    public async Task<ActionResult<CartDTO>> FindById(string id)
    {
        var response = await _cartRepository.FindCartByUserId(id);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartDTO>> AddCart([FromBody] CartDTO cart)
    {
        var response = await _cartRepository.SaveOrUpdateCart(cart);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartDTO>> UpdateCart([FromBody] CartDTO cart)
    {
        var response = await _cartRepository.SaveOrUpdateCart(cart);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpDelete("remove-cart/{id}")]
    public async Task<ActionResult<CartDTO>> RemoveCart(int id)
    {
        var response = await _cartRepository.RemoveFromCart(id);

        if (!response)
            return BadRequest();

        return Ok(response);
    }

    [HttpPost("apply-coupon")]
    public async Task<ActionResult<CartDTO>> ApplyCoupon([FromBody] CartDTO cart)
    {
        var response = await _cartRepository.ApplyCoupon(cart.CartHeader.UserId, cart.CartHeader.CouponCode);

        if (!response)
            return NotFound();

        return Ok(response);
    }

    [HttpDelete("remove-coupon/{userId}")]
    public async Task<ActionResult<CartDTO>> RemoveCoupon(string userId)
    {
        var response = await _cartRepository.RemoveCoupon(userId);

        if (!response)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderDTO>> Checkout([FromBody] CheckoutHeaderDTO dto)
    {
        if (dto?.UserId == null)
            return BadRequest();
            
        var response = await _cartRepository.FindCartByUserId(dto.UserId);

        if (response == null)
            return NotFound();

        if (!string.IsNullOrEmpty(dto.CouponCode))
        {
            var token = Request.Headers["Authorization"];
            var coupon = await _couponService.GetCoupon(token, dto.CouponCode);

            if (coupon.DiscountAmount != dto.DiscountAmount)
                return StatusCode(412);
        }

        dto.CartDetails = response.CartDetails;
        dto.DateTime = DateTime.Now;

        _rabbitMQMessageSender.SendMessage(dto, "checkoutqueue");

        return Ok(dto);
    }
}
