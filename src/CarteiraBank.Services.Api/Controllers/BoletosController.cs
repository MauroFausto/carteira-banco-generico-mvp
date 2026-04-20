using CarteiraBank.Application.Services;
using CarteiraBank.Services.Api.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraBank.Services.Api.Controllers;

[ApiController]
[Route("api/boletos")]
[Authorize(Policy = "ClienteOrSupervisor")]
public sealed class BoletosController(IAcordoBoletoAppService acordoBoletoAppService) : ControllerBase
{
    [HttpGet("{id:guid}/pdf")]
    public async Task<IActionResult> BaixarPdf(Guid id, CancellationToken cancellationToken)
    {
        var pdf = await acordoBoletoAppService.BaixarBoletoPdfAsync(id, cancellationToken);
        if (pdf is null)
        {
            throw new ExcecaoEntradaUsuario("Boleto nao encontrado.", "Confirme o identificador do boleto.");
        }

        return File(pdf.Value.Pdf, "application/pdf", pdf.Value.FileName);
    }
}
