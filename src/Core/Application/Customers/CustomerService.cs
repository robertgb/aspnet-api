using aspnet_api.Core.Domain.Customers;

namespace aspnet_api.Core.Application.Customers;

/// <summary>Orquestra os casos de uso de clientes.</summary>
public sealed class CustomerService
{
    private readonly ICustomerRepository repository;

    public CustomerService(ICustomerRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>Lista todos os clientes.</summary>
    public async Task<IReadOnlyCollection<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await repository.GetAllAsync(cancellationToken);
        return customers.Select(CustomerResponse.FromDomain).ToArray();
    }

    /// <summary>Busca um cliente pelo identificador.</summary>
    public async Task<CustomerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await repository.GetByIdAsync(id, cancellationToken) is { } customer
            ? CustomerResponse.FromDomain(customer)
            : null;
    }

    /// <summary>Cria um cliente e impede e-mails duplicados.</summary>
    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var customer = Customer.Create(request.Name, request.Email);

        if (await repository.GetByEmailAsync(customer.Email, cancellationToken) is not null)
        {
            throw new InvalidOperationException("A customer with this email already exists.");
        }

        return CustomerResponse.FromDomain(await repository.AddAsync(customer, cancellationToken));
    }

    /// <summary>Atualiza um cliente existente.</summary>
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

    /// <summary>Remove um cliente.</summary>
    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return repository.DeleteAsync(id, cancellationToken);
    }
}

/// <summary>Dados necessários para criar um cliente.</summary>
public sealed record CreateCustomerRequest(string Name, string Email);
/// <summary>Dados permitidos na atualização de um cliente.</summary>
public sealed record UpdateCustomerRequest(string Name, string Email);

/// <summary>Representação de saída de um cliente.</summary>
public sealed record CustomerResponse(Guid Id, string Name, string Email, DateTime CreatedAt)
{
    /// <summary>Converte uma entidade de domínio para resposta da API.</summary>
    public static CustomerResponse FromDomain(Customer customer)
    {
        return new CustomerResponse(customer.Id, customer.Name, customer.Email, customer.CreatedAt);
    }
}
