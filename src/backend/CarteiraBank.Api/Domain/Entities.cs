namespace CarteiraBank.Api.Domain;

public sealed class Cliente
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NomeCompleto { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public List<SolicitacaoCredito> SolicitacoesCredito { get; set; } = [];
    public List<Contrato> Contratos { get; set; } = [];
}

public sealed class SolicitacaoCredito
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public decimal ValorSolicitado { get; set; }
    public int QuantidadeParcelasSolicitada { get; set; }
    public string Finalidade { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendente";
    public string? MotivoDecisao { get; set; }
    public Guid? IdSupervisorDecisor { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? DecididoEm { get; set; }
}

public sealed class Contrato
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public string NumeroContrato { get; set; } = string.Empty;
    public decimal ValorPrincipal { get; set; }
    public string Status { get; set; } = "Ativo";
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public List<Parcela> Parcelas { get; set; } = [];
    public List<Acordo> Acordos { get; set; } = [];
}

public sealed class Parcela
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ContratoId { get; set; }
    public Contrato? Contrato { get; set; }
    public int Numero { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorJuros { get; set; }
    public decimal ValorMulta { get; set; }
    public DateOnly DataVencimento { get; set; }
    public string Status { get; set; } = "Aberta";
}

public sealed class Acordo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ContratoId { get; set; }
    public Contrato? Contrato { get; set; }
    public decimal ValorTotal { get; set; }
    public int QuantidadeParcelas { get; set; }
    public decimal ValorDesconto { get; set; }
    public string Status { get; set; } = "Ativo";
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public List<ParcelaAcordo> Parcelas { get; set; } = [];
}

public sealed class ParcelaAcordo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AcordoId { get; set; }
    public Acordo? Acordo { get; set; }
    public int Numero { get; set; }
    public decimal Valor { get; set; }
    public DateOnly DataVencimento { get; set; }
    public string Status { get; set; } = "Aberta";

    public Boleto? Boleto { get; set; }
}

public sealed class Boleto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ParcelaAcordoId { get; set; }
    public ParcelaAcordo? ParcelaAcordo { get; set; }
    public string CodigoBarras { get; set; } = string.Empty;
    public string LinhaDigitavel { get; set; } = string.Empty;
    public string Status { get; set; } = "Gerado";
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
