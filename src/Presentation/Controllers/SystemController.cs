using Microsoft.AspNetCore.Mvc;

namespace aspnet_api.Presentation.Controllers;

/// <summary>Expõe endpoints básicos de disponibilidade da API.</summary>
[ApiController]
public sealed class SystemController : ControllerBase
{
    /// <summary>Retorna uma mensagem indicando que a API está disponível.</summary>
    [HttpGet("/")]
    public IActionResult GetRoot()
    {
        return Ok(new { message = "ASP.NET API is running" });
    }

    /// <summary>Retorna o estado de saúde e o horário da verificação.</summary>
    [HttpGet("/health")]
    public IActionResult GetHealth()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow
        });
    }
}
