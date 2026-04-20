namespace CarteiraBank.Services.Api.Middleware;

public sealed class ExcecaoEntradaUsuario : Exception
{
    public ExcecaoEntradaUsuario(string mensagemUsuario, string orientacao, IReadOnlyList<DetalheErro>? detalhes = null)
        : base(mensagemUsuario)
    {
        MensagemUsuario = mensagemUsuario;
        Orientacao = orientacao;
        Detalhes = detalhes ?? [];
    }

    public string MensagemUsuario { get; }
    public string Orientacao { get; }
    public IReadOnlyList<DetalheErro> Detalhes { get; }
}

public sealed record DetalheErro(string Campo, string Erro);

public sealed record RespostaErroApi(
    bool Sucesso,
    string Codigo,
    string Mensagem,
    string Orientacao,
    string TraceId,
    DateTime Timestamp,
    IReadOnlyList<DetalheErro>? Detalhes = null);
