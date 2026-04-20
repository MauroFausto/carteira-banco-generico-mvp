using CarteiraBank.Application.Services;
using CarteiraBank.Domain.Commands;
using CarteiraBank.Services.Api.Contracts;
using CarteiraBank.Services.Api.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraBank.Services.Api.Controllers;

[ApiController]
[Route("api/solicitacoes-credito")]
public sealed class CreditoController(ICreditoAppService creditoAppService) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "ClienteOnly")]
    public async Task<IActionResult> CriarSolicitacao(
        [FromBody] CriarSolicitacaoCreditoRequisicao requisicao,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCreditRequestCommand
        {
            ClienteId = User.ObterIdUsuario(),
            ValorSolicitado = requisicao.ValorSolicitado,
            QuantidadeParcelasSolicitada = requisicao.QuantidadeParcelasSolicitada,
            Finalidade = requisicao.Finalidade
        };

        var result = await creditoAppService.CriarSolicitacaoAsync(command, cancellationToken);
        if (!result.Success)
        {
            throw new ExcecaoEntradaUsuario(result.Message, "Verifique os dados informados.");
        }

        return Ok(new { id = result.AggregateId, status = "Pendente" });
    }

    [HttpGet]
    [Authorize(Policy = "ClienteOrSupervisor")]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var itens = User.IsInRole("Supervisor")
            ? await creditoAppService.ListarSolicitacoesAsync(null, cancellationToken)
            : await creditoAppService.ListarSolicitacoesAsync(User.ObterIdUsuario(), cancellationToken);

        return Ok(itens);
    }

    [HttpPost("{id:guid}/decisao")]
    [Authorize(Policy = "SupervisorOnly")]
    public async Task<IActionResult> Decidir(
        Guid id,
        [FromBody] DecidirSolicitacaoCreditoRequisicao requisicao,
        CancellationToken cancellationToken)
    {
        var command = new DecideCreditRequestCommand
        {
            SolicitacaoId = id,
            SupervisorId = User.ObterIdUsuario(),
            Aprovar = requisicao.Aprovar,
            Motivo = requisicao.Motivo
        };

        var result = await creditoAppService.DecidirSolicitacaoAsync(command, cancellationToken);
        if (!result.Success)
        {
            throw new ExcecaoEntradaUsuario(result.Message, "Confirme os dados da solicitacao.");
        }

        return Ok(new { id = result.AggregateId, status = requisicao.Aprovar ? "Aprovada" : "Negada" });
    }
}
