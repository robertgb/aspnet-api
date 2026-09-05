namespace aspnet_api.Core.Domain.Products;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Product()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    private Product(Guid id, string name, string? description, decimal price)
    {
        Id = id;
        Name = ValidateName(name);
        Description = description?.Trim() ?? string.Empty;
        Price = ValidatePrice(price);
        CreatedAt = DateTime.UtcNow;
    }

    public static Product Create(string name, string? description, decimal price)
    {
        return new Product(Guid.NewGuid(), name, description, price);
    }

    public void Update(string name, string? description, decimal price)
    {
        Name = ValidateName(name);
        Description = description?.Trim() ?? string.Empty;
        Price = ValidatePrice(price);
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        return name.Trim();
    }

    private static decimal ValidatePrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        }

        return decimal.Round(price, 2);
    }
}
