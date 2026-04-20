namespace CarteiraBank.Services.Api.Contracts;

public sealed record CriarSolicitacaoCreditoRequisicao(
    decimal ValorSolicitado,
    int QuantidadeParcelasSolicitada,
    string Finalidade);

public sealed record DecidirSolicitacaoCreditoRequisicao(
    bool Aprovar,
    string? Motivo);

public sealed record CriarAcordoRequisicao(
    decimal ValorDesconto,
    int QuantidadeParcelas);
