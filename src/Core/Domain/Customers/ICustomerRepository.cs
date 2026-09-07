namespace aspnet_api.Core.Domain.Customers;

/// <summary>Define a persistência de clientes para a camada de aplicação.</summary>
public interface ICustomerRepository
{
    /// <summary>Obtém todos os clientes ordenados por criação.</summary>
    Task<IReadOnlyCollection<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
    /// <summary>Obtém um cliente pelo identificador.</summary>
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>Obtém um cliente pelo e-mail.</summary>
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    /// <summary>Adiciona um cliente.</summary>
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default);
    /// <summary>Persiste alterações pendentes.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    /// <summary>Remove um cliente e informa se ele existia.</summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
