using aspnet_api.Core.Application.Products;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_api.Presentation.Controllers;

[ApiController]
[Route("products")]
public sealed class ProductController : ControllerBase
{
    private readonly ProductService service;

    public ProductController(ProductService service)
    {
        this.service = service;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<ProductResponse>> GetAll()
    {
        return Ok(service.GetAll());
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProductResponse> GetById(Guid id)
    {
        var product = service.GetById(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public ActionResult<ProductResponse> Create(CreateProductRequest request)
    {
        try
        {
            var product = service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public ActionResult<ProductResponse> Update(Guid id, UpdateProductRequest request)
    {
        try
        {
            var product = service.Update(id, request);
            return product is null ? NotFound() : Ok(product);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        return service.Delete(id) ? NoContent() : NotFound();
    }
}
