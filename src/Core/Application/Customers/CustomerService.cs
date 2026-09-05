using aspnet_api.Core.Domain.Customers;

namespace aspnet_api.Core.Application.Customers;

public sealed class CustomerService
{
    private readonly ICustomerRepository repository;

    public CustomerService(ICustomerRepository repository)
    {
        this.repository = repository;
    }

    public IReadOnlyCollection<CustomerResponse> GetAll()
    {
        return repository.GetAll().Select(CustomerResponse.FromDomain).ToArray();
    }

    public CustomerResponse? GetById(Guid id)
    {
        return repository.GetById(id) is { } customer
            ? CustomerResponse.FromDomain(customer)
            : null;
    }

    public CustomerResponse Create(CreateCustomerRequest request)
    {
        var customer = Customer.Create(request.Name, request.Email);

        if (repository.GetByEmail(customer.Email) is not null)
        {
            throw new InvalidOperationException("A customer with this email already exists.");
        }

        return CustomerResponse.FromDomain(repository.Add(customer));
    }

    public CustomerResponse? Update(Guid id, UpdateCustomerRequest request)
    {
        var customer = repository.GetById(id);
        if (customer is null)
        {
            return null;
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var existingCustomer = repository.GetByEmail(email);
        if (existingCustomer is not null && existingCustomer.Id != id)
        {
            throw new InvalidOperationException("A customer with this email already exists.");
        }

        customer.Update(request.Name, request.Email);
        return CustomerResponse.FromDomain(customer);
    }

    public bool Delete(Guid id)
    {
        return repository.Delete(id);
    }
}

public sealed record CreateCustomerRequest(string Name, string Email);
public sealed record UpdateCustomerRequest(string Name, string Email);

public sealed record CustomerResponse(Guid Id, string Name, string Email, DateTime CreatedAt)
{
    public static CustomerResponse FromDomain(Customer customer)
    {
        return new CustomerResponse(customer.Id, customer.Name, customer.Email, customer.CreatedAt);
    }
}
