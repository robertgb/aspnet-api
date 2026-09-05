namespace aspnet_api.Core.Domain.Products;

public interface IProductRepository
{
    IReadOnlyCollection<Product> GetAll();
    Product? GetById(Guid id);
    Product Add(Product product);
    bool Delete(Guid id);
}
