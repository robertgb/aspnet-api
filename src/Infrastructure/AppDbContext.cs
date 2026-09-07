using Microsoft.EntityFrameworkCore;
using aspnet_api.Core.Domain.Customers;
using aspnet_api.Core.Domain.Products;

namespace aspnet_api.Infrastructure;

/// <summary>Contexto EF Core para persistência PostgreSQL.</summary>
public class AppDbContext : DbContext
{
    /// <summary>Inicializa o contexto com as opções configuradas pela aplicação.</summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>Conjunto persistido de clientes.</summary>
    public DbSet<Customer> Customers => Set<Customer>();
    /// <summary>Conjunto persistido de produtos.</summary>
    public DbSet<Product> Products => Set<Product>();

    /// <summary>Configura chaves, índices e tipos relacionais do domínio.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(customer => customer.Id);
            entity.HasIndex(customer => customer.Email).IsUnique();
            entity.Property(customer => customer.Name).IsRequired();
            entity.Property(customer => customer.Email).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.Id);
            entity.Property(product => product.Name).IsRequired();
            entity.Property(product => product.Description).IsRequired();
            entity.Property(product => product.Price).HasPrecision(18, 2);
        });
    }
}
