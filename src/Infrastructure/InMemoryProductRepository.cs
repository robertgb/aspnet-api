using System.Collections.Concurrent;
using aspnet_api.Core.Domain.Products;

namespace aspnet_api.Infrastructure;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> products = new();

    public IReadOnlyCollection<Product> GetAll()
    {
        return products.Values.OrderBy(product => product.CreatedAt).ToArray();
    }

    public Product? GetById(Guid id)
    {
        return products.TryGetValue(id, out var product) ? product : null;
    }

    public Product Add(Product product)
    {
        products[product.Id] = product;
        return product;
    }

    public bool Delete(Guid id)
    {
        return products.TryRemove(id, out _);
    }
}
