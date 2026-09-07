using aspnet_api.Core.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace aspnet_api.Infrastructure.Repositories;

/// <summary>Implementa a persistência de clientes com EF Core.</summary>
public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext context;

    /// <summary>Inicializa o repositório.</summary>
    public CustomerRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <summary>Consulta todos os clientes sem rastreamento.</summary>
    public async Task<IReadOnlyCollection<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.CreatedAt)
            .ToArrayAsync(cancellationToken);
    }

    /// <summary>Consulta um cliente pelo identificador.</summary>
    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Customers.FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    /// <summary>Consulta um cliente pelo e-mail normalizado.</summary>
    public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return context.Customers.FirstOrDefaultAsync(customer => customer.Email == normalizedEmail, cancellationToken);
    }

    /// <summary>Adiciona e persiste um cliente.</summary>
    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await context.Customers.AddAsync(customer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return customer;
    }

    /// <summary>Persiste alterações rastreadas pelo contexto.</summary>
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Remove um cliente pelo identificador.</summary>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (customer is null)
        {
            return false;
        }

        context.Customers.Remove(customer);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
