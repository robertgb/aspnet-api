using aspnet_api.Core.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace aspnet_api.Infrastructure.Repositories;

/// <summary>Implementa a persistência de produtos com EF Core.</summary>
public sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext context;

    /// <summary>Inicializa o repositório.</summary>
    public ProductRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <summary>Consulta todos os produtos sem rastreamento.</summary>
    public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Products
            .AsNoTracking()
            .OrderBy(product => product.CreatedAt)
            .ToArrayAsync(cancellationToken);
    }

    /// <summary>Consulta um produto pelo identificador.</summary>
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    /// <summary>Adiciona e persiste um produto.</summary>
    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return product;
    }

    /// <summary>Persiste alterações rastreadas pelo contexto.</summary>
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Remove um produto pelo identificador.</summary>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await context.Products.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (product is null)
        {
            return false;
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
