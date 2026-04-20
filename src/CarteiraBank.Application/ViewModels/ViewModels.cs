namespace CarteiraBank.Application.ViewModels;

public sealed record ClienteViewModel(Guid Id, string NomeCompleto, string Documento, string Email);

public sealed record SolicitacaoCreditoViewModel(
    Guid Id,
    Guid ClienteId,
    decimal ValorSolicitado,
    int QuantidadeParcelasSolicitada,
    string Finalidade,
    string Status,
    string? MotivoDecisao,
    DateTime CriadoEm);

public sealed record ContratoViewModel(
    Guid Id,
    Guid ClienteId,
    string NumeroContrato,
    decimal ValorPrincipal,
    string Status,
    int QuantidadeParcelasAbertas);

public sealed record SaldoDevedorViewModel(
    Guid ContratoId,
    string NumeroContrato,
    decimal Principal,
    decimal Juros,
    decimal Multa,
    decimal Total,
    DateTime CalculadoEm);

public sealed record AcordoViewModel(Guid Id, decimal ValorTotal, int QuantidadeParcelas);

public sealed record BoletoViewModel(Guid Id, string CodigoBarras, string LinhaDigitavel);
