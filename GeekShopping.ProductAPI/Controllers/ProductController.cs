using GeekShopping.ProductAPI.DTOs;
using GeekShopping.ProductAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> FindAll()
    {
        var product = await _repository.FindAll();

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> FindById(long id)
    {
        var product = await _repository.FindById(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }
}
