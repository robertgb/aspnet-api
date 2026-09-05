namespace aspnet_api.Core.Domain.Customers;

public interface ICustomerRepository
{
    IReadOnlyCollection<Customer> GetAll();
    Customer? GetById(Guid id);
    Customer? GetByEmail(string email);
    Customer Add(Customer customer);
    bool Delete(Guid id);
}
