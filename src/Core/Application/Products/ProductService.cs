using aspnet_api.Core.Domain.Products;

namespace aspnet_api.Core.Application.Products;

public sealed class ProductService
{
    private readonly IProductRepository repository;

    public ProductService(IProductRepository repository)
    {
        this.repository = repository;
    }

    public IReadOnlyCollection<ProductResponse> GetAll()
    {
        return repository.GetAll().Select(ProductResponse.FromDomain).ToArray();
    }

    public ProductResponse? GetById(Guid id)
    {
        return repository.GetById(id) is { } product
            ? ProductResponse.FromDomain(product)
            : null;
    }

    public ProductResponse Create(CreateProductRequest request)
    {
        var product = Product.Create(request.Name, request.Description, request.Price);
        return ProductResponse.FromDomain(repository.Add(product));
    }

    public ProductResponse? Update(Guid id, UpdateProductRequest request)
    {
        var product = repository.GetById(id);
        if (product is null)
        {
            return null;
        }

        product.Update(request.Name, request.Description, request.Price);
        return ProductResponse.FromDomain(product);
    }

    public bool Delete(Guid id)
    {
        return repository.Delete(id);
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
