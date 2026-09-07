namespace aspnet_api.Core.Domain.Products;

/// <summary>Representa um produto do catálogo.</summary>
public sealed class Product
{
    /// <summary>Identificador único do produto.</summary>
    public Guid Id { get; private set; }
    /// <summary>Nome do produto.</summary>
    public string Name { get; private set; }
    /// <summary>Descrição do produto.</summary>
    public string Description { get; private set; }
    /// <summary>Preço com duas casas decimais.</summary>
    public decimal Price { get; private set; }
    /// <summary>Data e hora UTC de criação.</summary>
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

    /// <summary>Cria um produto aplicando as regras de domínio.</summary>
    /// <param name="name">Nome do produto.</param>
    /// <param name="description">Descrição opcional.</param>
    /// <param name="price">Preço não negativo.</param>
    /// <returns>Produto criado.</returns>
    public static Product Create(string name, string? description, decimal price)
    {
        return new Product(Guid.NewGuid(), name, description, price);
    }

    /// <summary>Atualiza os dados do produto e reaplica suas regras.</summary>
    /// <param name="name">Novo nome.</param>
    /// <param name="description">Nova descrição opcional.</param>
    /// <param name="price">Novo preço não negativo.</param>
    public void Update(string name, string? description, decimal price)
    {
        Name = ValidateName(name);
        Description = description?.Trim() ?? string.Empty;
        Price = ValidatePrice(price);
    }

    /// <summary>Valida e normaliza o nome do produto.</summary>
    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        return name.Trim();
    }

    /// <summary>Valida e arredonda o preço do produto.</summary>
    private static decimal ValidatePrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        }

        return decimal.Round(price, 2);
    }
}
