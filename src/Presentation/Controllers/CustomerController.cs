using aspnet_api.Core.Application.Customers;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_api.Presentation.Controllers;

[ApiController]
[Route("customers")]
public sealed class CustomerController : ControllerBase
{
    private readonly CustomerService service;

    public CustomerController(CustomerService service)
    {
        this.service = service;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<CustomerResponse>> GetAll()
    {
        return Ok(service.GetAll());
    }

    [HttpGet("{id:guid}")]
    public ActionResult<CustomerResponse> GetById(Guid id)
    {
        var customer = service.GetById(id);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public ActionResult<CustomerResponse> Create(CreateCustomerRequest request)
    {
        try
        {
            var customer = service.Create(request);
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

    [HttpPut("{id:guid}")]
    public ActionResult<CustomerResponse> Update(Guid id, UpdateCustomerRequest request)
    {
        try
        {
            var customer = service.Update(id, request);
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

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        return service.Delete(id) ? NoContent() : NotFound();
    }
}