using System.Text.RegularExpressions;

namespace CarteiraBank.Services.Api.Middleware;

public static partial class SanitizadorDadosSensiveis
{
    private static readonly Regex QueryPairsRegex = QueryPairs();
    private static readonly Regex SensitiveValueRegex = SensitiveValue();
    private static readonly Regex UserIdentifierRegex = UserIdentifier();

    private static readonly HashSet<string> SensitiveKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        "access_token",
        "refresh_token",
        "id_token",
        "token",
        "authorization",
        "apikey",
        "api_key",
        "password",
        "passwd",
        "pwd",
        "secret",
        "client_secret"
    };

    public static string SanitizarQueryString(string? queryString)
    {
        if (string.IsNullOrWhiteSpace(queryString))
        {
            return string.Empty;
        }

        var raw = queryString.StartsWith('?') ? queryString[1..] : queryString;
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        var sanitizedPairs = QueryPairsRegex
            .Matches(raw)
            .Select(m =>
            {
                var key = m.Groups["key"].Value;
                var value = m.Groups["value"].Success ? m.Groups["value"].Value : string.Empty;
                if (SensitiveKeys.Contains(key))
                {
                    return $"{key}=***REDACTED***";
                }

                return $"{key}={SanitizarTextoLivre(value)}";
            });

        return string.Join("&", sanitizedPairs);
    }

    public static string SanitizarUsuario(string? usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario))
        {
            return "anonimo";
        }

        return UserIdentifierRegex.IsMatch(usuario)
            ? "***REDACTED***"
            : SanitizarTextoLivre(usuario);
    }

    public static string SanitizarTextoLivre(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return string.Empty;
        }

        return SensitiveValueRegex.Replace(texto, "***REDACTED***");
    }

    [GeneratedRegex(@"(?<key>[^=&\s]+)(=(?<value>[^&]*))?")]
    private static partial Regex QueryPairs();

    [GeneratedRegex(@"(?i)(bearer\s+[a-z0-9\-_\.=]+|[a-z0-9_\-]{20,}\.[a-z0-9_\-]{10,}\.[a-z0-9_\-]{10,})")]
    private static partial Regex SensitiveValue();

    [GeneratedRegex(@"(?i)(^[^@\s]+@[^@\s]+\.[^@\s]+$|^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$)")]
    private static partial Regex UserIdentifier();
}
