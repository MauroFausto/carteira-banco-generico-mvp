using CarteiraBank.Application.Services;
using CarteiraBank.Services.Api.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraBank.Services.Api.Controllers;

[ApiController]
[Route("api/contratos")]
[Authorize(Policy = "ClienteOrSupervisor")]
public sealed class ContratosController(ICreditoAppService creditoAppService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var contratos = User.IsInRole("Supervisor")
            ? await creditoAppService.ListarContratosAsync(null, cancellationToken)
            : await creditoAppService.ListarContratosAsync(User.ObterIdUsuario(), cancellationToken);

        return Ok(contratos);
    }

    [HttpGet("{id:guid}/saldo-devedor")]
    public async Task<IActionResult> ObterSaldoDevedor(Guid id, CancellationToken cancellationToken)
    {
        var saldo = await creditoAppService.ObterSaldoDevedorAsync(id, cancellationToken);
        if (saldo is null)
        {
            throw new ExcecaoEntradaUsuario("Contrato nao encontrado.", "Verifique o identificador do contrato.");
        }

        return Ok(saldo);
    }
}
