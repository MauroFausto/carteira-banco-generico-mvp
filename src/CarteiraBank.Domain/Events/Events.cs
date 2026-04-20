using CarteiraBank.Domain.Core;

namespace CarteiraBank.Domain.Events;

public sealed class CreditRequestCreatedEvent(Guid solicitacaoId, Guid clienteId) : Event
{
    public Guid SolicitacaoId { get; } = solicitacaoId;
    public Guid ClienteId { get; } = clienteId;
}

public sealed class CreditApprovedEvent(Guid solicitacaoId, Guid contratoId) : Event
{
    public Guid SolicitacaoId { get; } = solicitacaoId;
    public Guid ContratoId { get; } = contratoId;
}

public sealed class CreditDeniedEvent(Guid solicitacaoId) : Event
{
    public Guid SolicitacaoId { get; } = solicitacaoId;
}

public sealed class AgreementCreatedEvent(Guid acordoId, Guid contratoId) : Event
{
    public Guid AcordoId { get; } = acordoId;
    public Guid ContratoId { get; } = contratoId;
}

public sealed class BilletIssuedEvent(Guid boletoId, Guid parcelaAcordoId) : Event
{
    public Guid BoletoId { get; } = boletoId;
    public Guid ParcelaAcordoId { get; } = parcelaAcordoId;
}
