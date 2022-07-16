using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers;

[ApiController]
[Route("api/v1/carts")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _repository;

    public CartController(ICartRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("find-cart/{id}")]
    public async Task<ActionResult<CartDTO>> FindById(string id)
    {
        var response = await _repository.FindCartByUserId(id);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartDTO>> AddCart([FromBody] CartDTO cart)
    {
        var response = await _repository.SaveOrUpdateCart(cart);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartDTO>> UpdateCart([FromBody] CartDTO cart)
    {
        var response = await _repository.SaveOrUpdateCart(cart);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpDelete("remove-cart/{id}")]
    public async Task<ActionResult<CartDTO>> RemoveCart(int id)
    {
        var response = await _repository.RemoveFromCart(id);

        if (!response)
            return BadRequest();

        return Ok(response);
    }

    [HttpPost("apply-coupon")]
    public async Task<ActionResult<CartDTO>> ApplyCoupon([FromBody] CartDTO cart)
    {
        var response = await _repository.ApplyCoupon(cart.CartHeader.UserId, cart.CartHeader.CouponCode);

        if (!response)
            return NotFound();

        return Ok(response);
    }

    [HttpDelete("remove-coupon/{userId}")]
    public async Task<ActionResult<CartDTO>> RemoveCoupon(string userId)
    {
        var response = await _repository.RemoveCoupon(userId);

        if (!response)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderDTO>> Checkout([FromBody] CheckoutHeaderDTO dto)
    {
        var response = await _repository.FindCartByUserId(dto.UserId);

        if (response == null)
            return NotFound();

        dto.CartDetails = response.CartDetails;
        dto.DateTime = DateTime.Now;

        // RabbitMQ

        return Ok(dto);
    }
}
