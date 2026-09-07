using aspnet_api.Core.Application.Customers;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_api.Presentation.Controllers;

/// <summary>Expõe as operações HTTP de clientes.</summary>
[ApiController]
[Route("customers")]
public sealed class CustomerController : ControllerBase
{
    private readonly CustomerService service;

    /// <summary>Inicializa o controller com o serviço de clientes.</summary>
    public CustomerController(CustomerService service)
    {
        this.service = service;
    }

    /// <summary>Retorna todos os clientes.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CustomerResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllAsync(cancellationToken));
    }

    /// <summary>Retorna um cliente pelo identificador.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await service.GetByIdAsync(id, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    /// <summary>Cria um cliente.</summary>
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await service.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { error = exception.Message });
        }
    }

    /// <summary>Atualiza um cliente.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> Update(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await service.UpdateAsync(id, request, cancellationToken);
            return customer is null ? NotFound() : Ok(customer);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { error = exception.Message });
        }
    }

    /// <summary>Remove um cliente.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        return await service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
    }
}