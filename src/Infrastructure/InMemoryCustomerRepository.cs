using System.Collections.Concurrent;
using aspnet_api.Core.Domain.Customers;

namespace aspnet_api.Infrastructure;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly ConcurrentDictionary<Guid, Customer> customers = new();

    public IReadOnlyCollection<Customer> GetAll()
    {
        return customers.Values.OrderBy(customer => customer.CreatedAt).ToArray();
    }

    public Customer? GetById(Guid id)
    {
        return customers.TryGetValue(id, out var customer) ? customer : null;
    }

    public Customer? GetByEmail(string email)
    {
        return customers.Values.FirstOrDefault(customer => customer.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public Customer Add(Customer customer)
    {
        customers[customer.Id] = customer;
        return customer;
    }

    public bool Delete(Guid id)
    {
        return customers.TryRemove(id, out _);
    }
}
