using CarteiraBank.Application.Services;
using CarteiraBank.Domain.Commands;
using CarteiraBank.Services.Api.Contracts;
using CarteiraBank.Services.Api.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraBank.Services.Api.Controllers;

[ApiController]
[Authorize(Policy = "SupervisorOnly")]
public sealed class AcordosController(IAcordoBoletoAppService acordoBoletoAppService) : ControllerBase
{
    [HttpPost("api/contratos/{id:guid}/acordos")]
    public async Task<IActionResult> CriarAcordo(
        Guid id,
        [FromBody] CriarAcordoRequisicao requisicao,
        CancellationToken cancellationToken)
    {
        var command = new CreateAgreementCommand
        {
            ContratoId = id,
            ValorDesconto = requisicao.ValorDesconto,
            QuantidadeParcelas = requisicao.QuantidadeParcelas
        };

        var result = await acordoBoletoAppService.CriarAcordoAsync(command, cancellationToken);
        if (!result.Success)
        {
            throw new ExcecaoEntradaUsuario(result.Message, "Revise os dados do acordo.");
        }

        return Ok(new { id = result.AggregateId });
    }

    [HttpPost("api/acordos/{id:guid}/boleto")]
    public async Task<IActionResult> EmitirBoleto(Guid id, CancellationToken cancellationToken)
    {
        var command = new IssueBilletCommand { ParcelaAcordoId = id };
        var result = await acordoBoletoAppService.EmitirBoletoAsync(command, cancellationToken);
        if (!result.Success)
        {
            throw new ExcecaoEntradaUsuario(result.Message, "Parcela do acordo invalida.");
        }

        return Ok(new { id = result.AggregateId });
    }
}
