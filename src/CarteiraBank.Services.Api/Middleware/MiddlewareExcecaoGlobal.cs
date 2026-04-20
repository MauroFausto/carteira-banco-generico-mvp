using System.Diagnostics;
using System.Text.Json;

namespace CarteiraBank.Services.Api.Middleware;

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
                false,
                "ERRO_VALIDACAO",
                ex.MensagemUsuario,
                ex.Orientacao,
                traceId,
                DateTime.UtcNow,
                ex.Detalhes);

            await context.Response.WriteAsync(JsonSerializer.Serialize(resposta, JsonOptions));
        }
        catch (Exception ex)
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
            var querySanitizada = SanitizadorDadosSensiveis.SanitizarQueryString(context.Request.QueryString.ToString());
            var usuarioSanitizado = SanitizadorDadosSensiveis.SanitizarUsuario(context.User?.Identity?.Name);
            logger.LogError(ex,
                "Erro interno nao tratado. Metodo={Metodo} Caminho={Caminho} Query={Query} Usuario={Usuario} TraceId={TraceId}",
                context.Request.Method,
                context.Request.Path,
                querySanitizada,
                usuarioSanitizado,
                traceId);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var resposta = new RespostaErroApi(
                false,
                "ERRO_INTERNO",
                "Ocorreu um erro interno. Tente novamente mais tarde.",
                "Se o problema persistir, contate o suporte e informe o traceId.",
                traceId,
                DateTime.UtcNow);

            await context.Response.WriteAsync(JsonSerializer.Serialize(resposta, JsonOptions));
        }
    }
}
