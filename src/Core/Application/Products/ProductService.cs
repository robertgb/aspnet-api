using aspnet_api.Core.Domain.Products;

namespace aspnet_api.Core.Application.Products;

public sealed class ProductService
{
    private readonly IProductRepository repository;

    public ProductService(IProductRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IReadOnlyCollection<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return products.Select(ProductResponse.FromDomain).ToArray();
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await repository.GetByIdAsync(id, cancellationToken) is { } product
            ? ProductResponse.FromDomain(product)
            : null;
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await repository.AddAsync(Product.Create(request.Name, request.Description, request.Price), cancellationToken);
        return ProductResponse.FromDomain(product);
    }

    public async Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        product.Update(request.Name, request.Description, request.Price);

        await repository.SaveChangesAsync(cancellationToken);
        return ProductResponse.FromDomain(product);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return repository.DeleteAsync(id, cancellationToken);
    }
}

public sealed record CreateProductRequest(string Name, string? Description, decimal Price);
public sealed record UpdateProductRequest(string Name, string? Description, decimal Price);

public sealed record ProductResponse(Guid Id, string Name, string Description, decimal Price, DateTime CreatedAt)
{
    public static ProductResponse FromDomain(Product product)
    {
        return new ProductResponse(product.Id, product.Name, product.Description, product.Price, product.CreatedAt);
    }
}
