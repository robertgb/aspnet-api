using Microsoft.AspNetCore.Mvc;

namespace aspnet_api.Presentation.Controllers;

[ApiController]
public sealed class SystemController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult GetRoot()
    {
        return Ok(new { message = "ASP.NET API is running" });
    }

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
