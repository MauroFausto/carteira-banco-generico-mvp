using System.Diagnostics;
using System.Text.Json;

namespace CarteiraBank.Api.Features.Erros;

public sealed class MiddlewareExcecaoGlobal(RequestDelegate next, ILogger<MiddlewareExcecaoGlobal> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false
    };

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ExcecaoEntradaUsuario ex)
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
            logger.LogWarning(ex,
                "Erro de entrada do usuario. Metodo={Metodo} Caminho={Caminho} TraceId={TraceId} Detalhes={Detalhes}",
                context.Request.Method,
                context.Request.Path,
                traceId,
                JsonSerializer.Serialize(ex.Detalhes, JsonOptions));

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var resposta = new RespostaErroApi(
                Sucesso: false,
                Codigo: "ERRO_VALIDACAO",
                Mensagem: ex.MensagemUsuario,
                Orientacao: ex.Orientacao,
                TraceId: traceId,
                Timestamp: DateTime.UtcNow,
                Detalhes: ex.Detalhes);

            await context.Response.WriteAsync(JsonSerializer.Serialize(resposta, JsonOptions));
        }
        catch (Exception ex)
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
            logger.LogError(ex,
                "Erro interno nao tratado. Metodo={Metodo} Caminho={Caminho} Query={Query} Usuario={Usuario} TraceId={TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString.ToString(),
                context.User?.Identity?.Name ?? "anonimo",
                traceId);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var resposta = new RespostaErroApi(
                Sucesso: false,
                Codigo: "ERRO_INTERNO",
                Mensagem: "Ocorreu um erro interno. Tente novamente mais tarde.",
                Orientacao: "Se o problema persistir, contate o suporte e informe o traceId.",
                TraceId: traceId,
                Timestamp: DateTime.UtcNow);

            await context.Response.WriteAsync(JsonSerializer.Serialize(resposta, JsonOptions));
        }
        finally
        {
            logger.LogDebug("Finalizada requisicao {Metodo} {Caminho} com status {StatusCode}.",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode);
        }
    }
}
