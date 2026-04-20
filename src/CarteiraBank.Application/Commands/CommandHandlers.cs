using CarteiraBank.Domain.Commands;
using CarteiraBank.Domain.Core;
using CarteiraBank.Domain.Events;
using CarteiraBank.Domain.Interfaces;
using CarteiraBank.Domain.Models;

namespace CarteiraBank.Application.Commands;

public sealed class RegisterCreditRequestCommandHandler(
    ICreditoRepository creditoRepository,
    IMediatorHandler mediator) : IRequestHandler<RegisterCreditRequestCommand>
{
    public async Task<CommandResult> HandleAsync(RegisterCreditRequestCommand command, CancellationToken cancellationToken)
    {
        var solicitacao = new SolicitacaoCredito(
            command.ClienteId,
            command.ValorSolicitado,
            command.QuantidadeParcelasSolicitada,
            command.Finalidade);

        await creditoRepository.AddAsync(solicitacao, cancellationToken);
        await creditoRepository.UnitOfWork.CommitAsync(cancellationToken);
        await mediator.PublishEventAsync(new CreditRequestCreatedEvent(solicitacao.Id, solicitacao.ClienteId), cancellationToken);

        return CommandResult.Ok(solicitacao.Id);
    }
}

public sealed class DecideCreditRequestCommandHandler(
    ICreditoRepository creditoRepository,
    IContratoRepository contratoRepository,
    IMediatorHandler mediator) : IRequestHandler<DecideCreditRequestCommand>
{
    public async Task<CommandResult> HandleAsync(DecideCreditRequestCommand command, CancellationToken cancellationToken)
    {
        var solicitacao = await creditoRepository.GetByIdAsync(command.SolicitacaoId, cancellationToken);
        if (solicitacao is null)
        {
            return CommandResult.Fail("Solicitacao nao encontrada.");
        }

        if (command.Aprovar)
        {
            solicitacao.Aprovar(command.SupervisorId, command.Motivo);
            var contrato = new Contrato(
                solicitacao.ClienteId,
                $"CTR-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
                solicitacao.ValorSolicitado);

            var valorParcela = Math.Round(solicitacao.ValorSolicitado / solicitacao.QuantidadeParcelasSolicitada, 2);
            for (var i = 1; i <= solicitacao.QuantidadeParcelasSolicitada; i++)
            {
                contrato.Parcelas.Add(new Parcela(
                    i,
                    valorParcela,
                    Math.Round(valorParcela * 0.03m, 2),
                    0m,
                    DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(i))));
            }

            await contratoRepository.AddAsync(contrato, cancellationToken);
            await contratoRepository.UnitOfWork.CommitAsync(cancellationToken);
            await mediator.PublishEventAsync(new CreditApprovedEvent(solicitacao.Id, contrato.Id), cancellationToken);
            return CommandResult.Ok(solicitacao.Id);
        }

        solicitacao.Negar(command.SupervisorId, command.Motivo);
        await creditoRepository.UnitOfWork.CommitAsync(cancellationToken);
        await mediator.PublishEventAsync(new CreditDeniedEvent(solicitacao.Id), cancellationToken);
        return CommandResult.Ok(solicitacao.Id);
    }
}

public sealed class CreateAgreementCommandHandler(
    IContratoRepository contratoRepository,
    IAcordoRepository acordoRepository,
    IMediatorHandler mediator) : IRequestHandler<CreateAgreementCommand>
{
    public async Task<CommandResult> HandleAsync(CreateAgreementCommand command, CancellationToken cancellationToken)
    {
        var contrato = await contratoRepository.GetByIdWithParcelasAsync(command.ContratoId, cancellationToken);
        if (contrato is null)
        {
            return CommandResult.Fail("Contrato nao encontrado.");
        }

        var parcelasAbertas = contrato.Parcelas.Where(x => x.Status == "Aberta").ToList();
        var valorBase = parcelasAbertas.Sum(x => x.Valor + x.ValorJuros + x.ValorMulta);
        var valorComDesconto = Math.Max(0, valorBase - command.ValorDesconto);
        var valorParcela = Math.Round(valorComDesconto / command.QuantidadeParcelas, 2);

        var acordo = new Acordo(contrato.Id, valorComDesconto, command.QuantidadeParcelas, command.ValorDesconto);
        for (var i = 1; i <= command.QuantidadeParcelas; i++)
        {
            acordo.Parcelas.Add(new ParcelaAcordo(
                i,
                valorParcela,
                DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(i))));
        }

        await acordoRepository.AddAsync(acordo, cancellationToken);
        await acordoRepository.UnitOfWork.CommitAsync(cancellationToken);
        await mediator.PublishEventAsync(new AgreementCreatedEvent(acordo.Id, acordo.ContratoId), cancellationToken);

        return CommandResult.Ok(acordo.Id);
    }
}

public sealed class IssueBilletCommandHandler(
    IAcordoRepository acordoRepository,
    IBoletoRepository boletoRepository,
    IMediatorHandler mediator) : IRequestHandler<IssueBilletCommand>
{
    public async Task<CommandResult> HandleAsync(IssueBilletCommand command, CancellationToken cancellationToken)
    {
        var parcelaAcordo = await acordoRepository.GetParcelaAcordoByIdAsync(command.ParcelaAcordoId, cancellationToken);
        if (parcelaAcordo is null)
        {
            return CommandResult.Fail("Parcela do acordo nao encontrada.");
        }

        var boleto = new Boleto(
            parcelaAcordo.Id,
            $"{Random.Shared.NextInt64(100000000000, 999999999999)}",
            $"{Random.Shared.NextInt64(10000, 99999)}.{Random.Shared.NextInt64(10000, 99999)}");

        await boletoRepository.AddAsync(boleto, cancellationToken);
        await boletoRepository.UnitOfWork.CommitAsync(cancellationToken);
        await mediator.PublishEventAsync(new BilletIssuedEvent(boleto.Id, parcelaAcordo.Id), cancellationToken);

        return CommandResult.Ok(boleto.Id);
    }
}
