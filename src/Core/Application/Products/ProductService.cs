using aspnet_api.Core.Domain.Products;

namespace aspnet_api.Core.Application.Products;

/// <summary>Orquestra os casos de uso de produtos.</summary>
public sealed class ProductService
{
    private readonly IProductRepository repository;

    public ProductService(IProductRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>Lista todos os produtos.</summary>
    public async Task<IReadOnlyCollection<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return products.Select(ProductResponse.FromDomain).ToArray();
    }

    /// <summary>Busca um produto pelo identificador.</summary>
    public async Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await repository.GetByIdAsync(id, cancellationToken) is { } product
            ? ProductResponse.FromDomain(product)
            : null;
    }

    /// <summary>Cria e persiste um produto.</summary>
    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await repository.AddAsync(Product.Create(request.Name, request.Description, request.Price), cancellationToken);
        return ProductResponse.FromDomain(product);
    }

    /// <summary>Atualiza um produto existente.</summary>
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

    /// <summary>Remove um produto.</summary>
    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return repository.DeleteAsync(id, cancellationToken);
    }
}

/// <summary>Dados necessários para criar um produto.</summary>
public sealed record CreateProductRequest(string Name, string? Description, decimal Price);
/// <summary>Dados permitidos na atualização de um produto.</summary>
public sealed record UpdateProductRequest(string Name, string? Description, decimal Price);

/// <summary>Representação de saída de um produto.</summary>
public sealed record ProductResponse(Guid Id, string Name, string Description, decimal Price, DateTime CreatedAt)
{
    /// <summary>Converte uma entidade de domínio para resposta da API.</summary>
    public static ProductResponse FromDomain(Product product)
    {
        return new ProductResponse(product.Id, product.Name, product.Description, product.Price, product.CreatedAt);
    }
}
