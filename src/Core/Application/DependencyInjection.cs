using Microsoft.Extensions.DependencyInjection;

namespace aspnet_api.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Customers.CustomerService>();
        services.AddScoped<Products.ProductService>();

        return services;
    }
}
