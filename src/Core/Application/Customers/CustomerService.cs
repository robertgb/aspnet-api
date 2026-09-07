using aspnet_api.Core.Domain.Customers;

namespace aspnet_api.Core.Application.Customers;

public sealed class CustomerService
{
    private readonly ICustomerRepository repository;

    public CustomerService(ICustomerRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IReadOnlyCollection<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await repository.GetAllAsync(cancellationToken);
        return customers.Select(CustomerResponse.FromDomain).ToArray();
    }

    public async Task<CustomerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await repository.GetByIdAsync(id, cancellationToken) is { } customer
            ? CustomerResponse.FromDomain(customer)
            : null;
    }

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = Customer.Create(request.Name, request.Email);

        if (await repository.GetByEmailAsync(customer.Email, cancellationToken) is not null)
        {
            throw new InvalidOperationException("A customer with this email already exists.");
        }

        return CustomerResponse.FromDomain(await repository.AddAsync(customer, cancellationToken));
    }

    public async Task<CustomerResponse?> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await repository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var existingCustomer = await repository.GetByEmailAsync(email, cancellationToken);
        if (existingCustomer is not null && existingCustomer.Id != id)
        {
            throw new InvalidOperationException("A customer with this email already exists.");
        }

        customer.Update(request.Name, request.Email);
        await repository.SaveChangesAsync(cancellationToken);
        return CustomerResponse.FromDomain(customer);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return repository.DeleteAsync(id, cancellationToken);
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
