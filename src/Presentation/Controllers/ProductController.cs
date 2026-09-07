using aspnet_api.Core.Application.Products;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_api.Presentation.Controllers;

/// <summary>Expõe as operações HTTP de produtos.</summary>
[ApiController]
[Route("products")]
public sealed class ProductController : ControllerBase
{
    private readonly ProductService service;

    /// <summary>Inicializa o controller com o serviço de produtos.</summary>
    public ProductController(ProductService service)
    {
        this.service = service;
    }

    /// <summary>Retorna todos os produtos.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProductResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllAsync(cancellationToken));
    }

    /// <summary>Retorna um produto pelo identificador.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await service.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    /// <summary>Cria um produto.</summary>
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await service.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    /// <summary>Atualiza um produto.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Update(Guid id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await service.UpdateAsync(id, request, cancellationToken);
            return product is null ? NotFound() : Ok(product);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    /// <summary>Remove um produto.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        return await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
    }
}
