using Microsoft.Extensions.DependencyInjection;

namespace aspnet_api.Core.Application;

/// <summary>Registra os serviços da camada de aplicação.</summary>
public static class DependencyInjection
{
    /// <summary>Adiciona os serviços de aplicação ao container de DI.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Customers.CustomerService>();
        services.AddScoped<Products.ProductService>();

        return services;
    }
}
