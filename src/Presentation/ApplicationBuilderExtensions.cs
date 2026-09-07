namespace aspnet_api.Presentation;

/// <summary>Configura o pipeline HTTP da camada de Presentation.</summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>Ativa Swagger em desenvolvimento e mapeia os controllers.</summary>
    public static WebApplication UsePresentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.MapControllers();

        return app;
    }
}
