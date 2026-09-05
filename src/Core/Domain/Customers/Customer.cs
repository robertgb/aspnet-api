namespace aspnet_api.Core.Domain.Customers;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
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

    public static Customer Create(string name, string email)
    {
        return new Customer(Guid.NewGuid(), name, email);
    }

    public void Update(string name, string email)
    {
        Name = ValidateName(name);
        Email = ValidateEmail(email);
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        return name.Trim();
    }

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
