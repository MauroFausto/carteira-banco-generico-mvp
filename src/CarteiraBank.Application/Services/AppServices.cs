using CarteiraBank.Application.ViewModels;
using CarteiraBank.Domain.Commands;
using CarteiraBank.Domain.Core;
using CarteiraBank.Domain.Interfaces;
using CarteiraBank.Domain.Models;

namespace CarteiraBank.Application.Services;

public interface IClienteAppService
{
    Task<ClienteViewModel?> ObterClienteAsync(Guid clienteId, CancellationToken cancellationToken);
}

public interface ICreditoAppService
{
    Task<CommandResult> CriarSolicitacaoAsync(RegisterCreditRequestCommand command, CancellationToken cancellationToken);
    Task<CommandResult> DecidirSolicitacaoAsync(DecideCreditRequestCommand command, CancellationToken cancellationToken);
    Task<List<SolicitacaoCreditoViewModel>> ListarSolicitacoesAsync(Guid? clienteId, CancellationToken cancellationToken);
    Task<List<ContratoViewModel>> ListarContratosAsync(Guid? clienteId, CancellationToken cancellationToken);
    Task<SaldoDevedorViewModel?> ObterSaldoDevedorAsync(Guid contratoId, CancellationToken cancellationToken);
}

public interface IAcordoBoletoAppService
{
    Task<CommandResult> CriarAcordoAsync(CreateAgreementCommand command, CancellationToken cancellationToken);
    Task<CommandResult> EmitirBoletoAsync(IssueBilletCommand command, CancellationToken cancellationToken);
    Task<(byte[] Pdf, string FileName)?> BaixarBoletoPdfAsync(Guid boletoId, CancellationToken cancellationToken);
}

public sealed class ClienteAppService(IClienteRepository clienteRepository) : IClienteAppService
{
    public async Task<ClienteViewModel?> ObterClienteAsync(Guid clienteId, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.GetByIdAsync(clienteId, cancellationToken);
        return cliente is null
            ? null
            : new ClienteViewModel(cliente.Id, cliente.NomeCompleto, cliente.Documento, cliente.Email);
    }
}

public sealed class CreditoAppService(
    IMediatorHandler mediator,
    ICreditoRepository creditoRepository,
    IContratoRepository contratoRepository) : ICreditoAppService
{
    public Task<CommandResult> CriarSolicitacaoAsync(RegisterCreditRequestCommand command, CancellationToken cancellationToken)
        => mediator.SendCommandAsync(command, cancellationToken);

    public Task<CommandResult> DecidirSolicitacaoAsync(DecideCreditRequestCommand command, CancellationToken cancellationToken)
        => mediator.SendCommandAsync(command, cancellationToken);

    public async Task<List<SolicitacaoCreditoViewModel>> ListarSolicitacoesAsync(Guid? clienteId, CancellationToken cancellationToken)
    {
        var itens = clienteId.HasValue
            ? await creditoRepository.ListByClienteAsync(clienteId.Value, cancellationToken)
            : await creditoRepository.ListAllAsync(cancellationToken);

        return itens.Select(x => new SolicitacaoCreditoViewModel(
            x.Id,
            x.ClienteId,
            x.ValorSolicitado,
            x.QuantidadeParcelasSolicitada,
            x.Finalidade,
            x.Status,
            x.MotivoDecisao,
            x.CriadoEm)).ToList();
    }

    public async Task<List<ContratoViewModel>> ListarContratosAsync(Guid? clienteId, CancellationToken cancellationToken)
    {
        var contratos = clienteId.HasValue
            ? await contratoRepository.ListByClienteAsync(clienteId.Value, cancellationToken)
            : await contratoRepository.ListAllAsync(cancellationToken);

        return contratos.Select(x => new ContratoViewModel(
            x.Id,
            x.ClienteId,
            x.NumeroContrato,
            x.ValorPrincipal,
            x.Status,
            x.Parcelas.Count(i => i.Status == "Aberta"))).ToList();
    }

    public async Task<SaldoDevedorViewModel?> ObterSaldoDevedorAsync(Guid contratoId, CancellationToken cancellationToken)
    {
        var contrato = await contratoRepository.GetByIdWithParcelasAsync(contratoId, cancellationToken);
        if (contrato is null)
        {
            return null;
        }

        var parcelasAbertas = contrato.Parcelas.Where(x => x.Status == "Aberta").ToList();
        var principal = parcelasAbertas.Sum(x => x.Valor);
        var juros = parcelasAbertas.Sum(x => x.ValorJuros);
        var multa = parcelasAbertas.Sum(x => x.ValorMulta);
        var total = principal + juros + multa;

        return new SaldoDevedorViewModel(
            contrato.Id,
            contrato.NumeroContrato,
            principal,
            juros,
            multa,
            total,
            DateTime.UtcNow);
    }
}

public sealed class AcordoBoletoAppService(IMediatorHandler mediator, IBoletoRepository boletoRepository)
    : IAcordoBoletoAppService
{
    public Task<CommandResult> CriarAcordoAsync(CreateAgreementCommand command, CancellationToken cancellationToken)
        => mediator.SendCommandAsync(command, cancellationToken);

    public Task<CommandResult> EmitirBoletoAsync(IssueBilletCommand command, CancellationToken cancellationToken)
        => mediator.SendCommandAsync(command, cancellationToken);

    public async Task<(byte[] Pdf, string FileName)?> BaixarBoletoPdfAsync(Guid boletoId, CancellationToken cancellationToken)
    {
        var boleto = await boletoRepository.GetByIdWithContratoAsync(boletoId, cancellationToken);
        if (boleto is null)
        {
            return null;
        }

        var pdf = GerarPdfSimples($"Boleto {boleto.Id}\nLinha Digitavel: {boleto.LinhaDigitavel}");
        return (pdf, $"boleto-{boletoId}.pdf");
    }

    private static byte[] GerarPdfSimples(string mensagem)
    {
        var sanitizado = mensagem.Replace("(", "[").Replace(")", "]");
        var pdf = $"""
                  %PDF-1.1
                  1 0 obj<</Type/Catalog/Pages 2 0 R>>endobj
                  2 0 obj<</Type/Pages/Kids[3 0 R]/Count 1>>endobj
                  3 0 obj<</Type/Page/Parent 2 0 R/MediaBox[0 0 612 792]/Contents 4 0 R/Resources<</Font<</F1 5 0 R>>>>>>endobj
                  4 0 obj<</Length 44>>stream
                  BT /F1 12 Tf 72 720 Td ({sanitizado}) Tj ET
                  endstream endobj
                  5 0 obj<</Type/Font/Subtype/Type1/BaseFont/Helvetica>>endobj
                  xref
                  0 6
                  0000000000 65535 f
                  0000000010 00000 n
                  0000000062 00000 n
                  0000000118 00000 n
                  0000000246 00000 n
                  0000000348 00000 n
                  trailer<</Root 1 0 R/Size 6>>
                  startxref
                  420
                  %%EOF
                  """;
        return System.Text.Encoding.ASCII.GetBytes(pdf);
    }
}
