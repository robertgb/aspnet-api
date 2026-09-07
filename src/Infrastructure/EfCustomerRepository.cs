using aspnet_api.Core.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace aspnet_api.Infrastructure;

public sealed class EfCustomerRepository : ICustomerRepository
{
    private readonly AppDbContext context;

    public EfCustomerRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyCollection<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.CreatedAt)
            .ToArrayAsync(cancellationToken);
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Customers.FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return context.Customers.FirstOrDefaultAsync(customer => customer.Email == normalizedEmail, cancellationToken);
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await context.Customers.AddAsync(customer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return customer;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

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
