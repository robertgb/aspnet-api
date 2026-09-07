namespace aspnet_api.Core.Domain.Products;

/// <summary>Define a persistência de produtos para a camada de aplicação.</summary>
public interface IProductRepository
{
    /// <summary>Obtém todos os produtos ordenados por criação.</summary>
    Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    /// <summary>Obtém um produto pelo identificador.</summary>
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>Adiciona um produto.</summary>
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
    /// <summary>Persiste alterações pendentes.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    /// <summary>Remove um produto e informa se ele existia.</summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
