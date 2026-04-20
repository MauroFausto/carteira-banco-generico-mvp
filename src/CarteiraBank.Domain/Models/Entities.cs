using CarteiraBank.Domain.Core;

namespace CarteiraBank.Domain.Models;

public sealed class Cliente : Entity
{
    public string NomeCompleto { get; private set; } = string.Empty;
    public string Documento { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

    public List<SolicitacaoCredito> SolicitacoesCredito { get; private set; } = [];
    public List<Contrato> Contratos { get; private set; } = [];

    private Cliente() { }

    public Cliente(Guid id, string nomeCompleto, string documento, string email)
    {
        Id = id;
        NomeCompleto = nomeCompleto;
        Documento = documento;
        Email = email;
    }
}

public sealed class SolicitacaoCredito : Entity
{
    public Guid ClienteId { get; private set; }
    public Cliente? Cliente { get; private set; }
    public decimal ValorSolicitado { get; private set; }
    public int QuantidadeParcelasSolicitada { get; private set; }
    public string Finalidade { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Pendente";
    public string? MotivoDecisao { get; private set; }
    public Guid? IdSupervisorDecisor { get; private set; }
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    public DateTime? DecididoEm { get; private set; }

    private SolicitacaoCredito() { }

    public SolicitacaoCredito(Guid clienteId, decimal valorSolicitado, int quantidadeParcelasSolicitada, string finalidade)
    {
        ClienteId = clienteId;
        ValorSolicitado = valorSolicitado;
        QuantidadeParcelasSolicitada = quantidadeParcelasSolicitada;
        Finalidade = finalidade;
    }

    public void Aprovar(Guid idSupervisor, string? motivo)
    {
        Status = "Aprovada";
        MotivoDecisao = motivo;
        IdSupervisorDecisor = idSupervisor;
        DecididoEm = DateTime.UtcNow;
    }

    public void Negar(Guid idSupervisor, string? motivo)
    {
        Status = "Negada";
        MotivoDecisao = motivo;
        IdSupervisorDecisor = idSupervisor;
        DecididoEm = DateTime.UtcNow;
    }
}

public sealed class Contrato : Entity
{
    public Guid ClienteId { get; private set; }
    public Cliente? Cliente { get; private set; }
    public string NumeroContrato { get; private set; } = string.Empty;
    public decimal ValorPrincipal { get; private set; }
    public string Status { get; private set; } = "Ativo";
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

    public List<Parcela> Parcelas { get; private set; } = [];
    public List<Acordo> Acordos { get; private set; } = [];

    private Contrato() { }

    public Contrato(Guid clienteId, string numeroContrato, decimal valorPrincipal)
    {
        ClienteId = clienteId;
        NumeroContrato = numeroContrato;
        ValorPrincipal = valorPrincipal;
    }
}

public sealed class Parcela : Entity
{
    public Guid ContratoId { get; private set; }
    public Contrato? Contrato { get; private set; }
    public int Numero { get; private set; }
    public decimal Valor { get; private set; }
    public decimal ValorJuros { get; private set; }
    public decimal ValorMulta { get; private set; }
    public DateOnly DataVencimento { get; private set; }
    public string Status { get; private set; } = "Aberta";

    private Parcela() { }

    public Parcela(int numero, decimal valor, decimal valorJuros, decimal valorMulta, DateOnly dataVencimento)
    {
        Numero = numero;
        Valor = valor;
        ValorJuros = valorJuros;
        ValorMulta = valorMulta;
        DataVencimento = dataVencimento;
    }
}

public sealed class Acordo : Entity
{
    public Guid ContratoId { get; private set; }
    public Contrato? Contrato { get; private set; }
    public decimal ValorTotal { get; private set; }
    public int QuantidadeParcelas { get; private set; }
    public decimal ValorDesconto { get; private set; }
    public string Status { get; private set; } = "Ativo";
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

    public List<ParcelaAcordo> Parcelas { get; private set; } = [];

    private Acordo() { }

    public Acordo(Guid contratoId, decimal valorTotal, int quantidadeParcelas, decimal valorDesconto)
    {
        ContratoId = contratoId;
        ValorTotal = valorTotal;
        QuantidadeParcelas = quantidadeParcelas;
        ValorDesconto = valorDesconto;
    }
}

public sealed class ParcelaAcordo : Entity
{
    public Guid AcordoId { get; private set; }
    public Acordo? Acordo { get; private set; }
    public int Numero { get; private set; }
    public decimal Valor { get; private set; }
    public DateOnly DataVencimento { get; private set; }
    public string Status { get; private set; } = "Aberta";

    public Boleto? Boleto { get; private set; }

    private ParcelaAcordo() { }

    public ParcelaAcordo(int numero, decimal valor, DateOnly dataVencimento)
    {
        Numero = numero;
        Valor = valor;
        DataVencimento = dataVencimento;
    }
}

public sealed class Boleto : Entity
{
    public Guid ParcelaAcordoId { get; private set; }
    public ParcelaAcordo? ParcelaAcordo { get; private set; }
    public string CodigoBarras { get; private set; } = string.Empty;
    public string LinhaDigitavel { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Gerado";
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

    private Boleto() { }

    public Boleto(Guid parcelaAcordoId, string codigoBarras, string linhaDigitavel)
    {
        ParcelaAcordoId = parcelaAcordoId;
        CodigoBarras = codigoBarras;
        LinhaDigitavel = linhaDigitavel;
    }
}
