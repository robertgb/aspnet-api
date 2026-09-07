using Microsoft.EntityFrameworkCore;
using aspnet_api.Core.Domain.Customers;
using aspnet_api.Core.Domain.Products;

namespace aspnet_api.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();

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
