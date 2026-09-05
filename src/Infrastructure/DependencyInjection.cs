using aspnet_api.Core.Application.Customers;
using aspnet_api.Core.Application.Products;
using aspnet_api.Core.Domain.Customers;
using aspnet_api.Core.Domain.Products;
using Microsoft.Extensions.DependencyInjection;

namespace aspnet_api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
        services.AddScoped<CustomerService>();
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        services.AddScoped<ProductService>();

        return services;
    }
}
