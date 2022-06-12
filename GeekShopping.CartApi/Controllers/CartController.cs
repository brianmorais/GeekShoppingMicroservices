using GeekShopping.CartApi.DTOs;
using GeekShopping.CartApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartApi.Controllers;

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
    public async Task<ActionResult<CartDTO>> FindById(string userId)
    {
        var response = await _repository.FindCartByUserId(userId);

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
}
