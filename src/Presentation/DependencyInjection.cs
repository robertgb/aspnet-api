using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace aspnet_api.Presentation;

/// <summary>Registra componentes HTTP e documentação da API.</summary>
public static class DependencyInjection
{
    /// <summary>Adiciona controllers, exploração de endpoints e Swagger.</summary>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var documentationFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var documentationPath = Path.Combine(AppContext.BaseDirectory, documentationFile);
            options.IncludeXmlComments(documentationPath, includeControllerXmlComments: true);
        });

        return services;
    }
}
