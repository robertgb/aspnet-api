using aspnet_api.Core.Domain.Customers;
using aspnet_api.Core.Domain.Products;
using aspnet_api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace aspnet_api.Infrastructure;

/// <summary>Registra banco de dados e repositórios da infraestrutura.</summary>
public static class DependencyInjection
{
    /// <summary>Configura PostgreSQL, EF Core e os repositórios.</summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
