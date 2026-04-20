using Microsoft.AspNetCore.Mvc;

namespace CarteiraBank.Services.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
        => Ok(new { status = "ok", service = "carteira-bank-api" });
}
