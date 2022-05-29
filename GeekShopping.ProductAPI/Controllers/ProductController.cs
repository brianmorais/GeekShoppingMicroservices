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

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> Create(ProductDTO productDto)
    {
        if (productDto == null)
            return BadRequest();

        var product = await _repository.Create(productDto);
        return Ok(product);
    }

    [HttpPut]
    public async Task<ActionResult<ProductDTO>> Update(ProductDTO productDto)
    {
        if (productDto == null)
            return BadRequest();

        var product = await _repository.Update(productDto);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductDTO>> Delete(long id)
    {
        var status = await _repository.Delete(id);

        if (status)
            return Ok(status);

        return BadRequest();
    }
}
