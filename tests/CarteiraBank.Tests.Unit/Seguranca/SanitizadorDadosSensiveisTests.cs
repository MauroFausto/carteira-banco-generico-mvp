using CarteiraBank.Services.Api.Middleware;

namespace CarteiraBank.Tests.Unit.Seguranca;

public sealed class SanitizadorDadosSensiveisTests
{
    [Fact]
    public void SanitizarQueryString_DeveMascararTokenESenha()
    {
        var query = "?token=abc123456789&password=super-secreta&clienteId=11111111-1111-1111-1111-111111111111";

        var resultado = SanitizadorDadosSensiveis.SanitizarQueryString(query);

        Assert.DoesNotContain("abc123456789", resultado);
        Assert.DoesNotContain("super-secreta", resultado);
        Assert.Contains("token=***REDACTED***", resultado);
        Assert.Contains("password=***REDACTED***", resultado);
    }

    [Fact]
    public void SanitizarUsuario_DeveMascararEmailEGuid()
    {
        var email = "cliente@empresa.com";
        var guid = "11111111-1111-1111-1111-111111111111";

        Assert.Equal("***REDACTED***", SanitizadorDadosSensiveis.SanitizarUsuario(email));
        Assert.Equal("***REDACTED***", SanitizadorDadosSensiveis.SanitizarUsuario(guid));
    }
}
