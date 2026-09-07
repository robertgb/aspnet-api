namespace aspnet_api.Core.Domain.Customers;

/// <summary>Representa um cliente do sistema.</summary>
public sealed class Customer
{
    /// <summary>Identificador único do cliente.</summary>
    public Guid Id { get; private set; }
    /// <summary>Nome do cliente.</summary>
    public string Name { get; private set; }
    /// <summary>E-mail normalizado do cliente.</summary>
    public string Email { get; private set; }
    /// <summary>Data e hora UTC de criação.</summary>
    public DateTime CreatedAt { get; private set; }

    private Customer()
    {
        Name = string.Empty;
        Email = string.Empty;
    }

    private Customer(Guid id, string name, string email)
    {
        Id = id;
        Name = ValidateName(name);
        Email = ValidateEmail(email);
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>Cria um cliente validando nome e e-mail.</summary>
    /// <param name="name">Nome do cliente.</param>
    /// <param name="email">E-mail do cliente.</param>
    /// <returns>Cliente criado.</returns>
    public static Customer Create(string name, string email)
    {
        return new Customer(Guid.NewGuid(), name, email);
    }

    /// <summary>Atualiza os dados do cliente.</summary>
    /// <param name="name">Novo nome.</param>
    /// <param name="email">Novo e-mail.</param>
    public void Update(string name, string email)
    {
        Name = ValidateName(name);
        Email = ValidateEmail(email);
    }

    /// <summary>Valida e normaliza o nome do cliente.</summary>
    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        return name.Trim();
    }

    /// <summary>Valida e normaliza o e-mail do cliente.</summary>
    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("A valid email is required.", nameof(email));
        }

        try
        {
            var normalizedEmail = email.Trim();
            var address = new System.Net.Mail.MailAddress(normalizedEmail).Address;
            if (!address.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("A valid email is required.", nameof(email));
            }
        }
        catch (FormatException)
        {
            throw new ArgumentException("A valid email is required.", nameof(email));
        }

        return email.Trim().ToLowerInvariant();
    }
}
