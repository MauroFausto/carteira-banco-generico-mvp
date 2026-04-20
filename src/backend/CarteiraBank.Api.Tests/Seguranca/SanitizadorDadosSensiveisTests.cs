using CarteiraBank.Api.Features.Erros;

namespace CarteiraBank.Api.Tests.Seguranca;

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
    public void SanitizarTextoLivre_DeveMascararBearerEJwt()
    {
        var texto = "Authorization: Bearer abcdefghijklmnopqrstuvwx.abcdefghijklmnop.qrstuvwxyz012345";

        var resultado = SanitizadorDadosSensiveis.SanitizarTextoLivre(texto);

        Assert.DoesNotContain("Bearer abcdefghijklmnopqrstuvwx", resultado);
        Assert.Contains("***REDACTED***", resultado);
    }

    [Fact]
    public void SanitizarUsuario_DeveMascararEmailEGuid()
    {
        var email = "cliente@empresa.com";
        var guid = "11111111-1111-1111-1111-111111111111";

        var usuarioEmail = SanitizadorDadosSensiveis.SanitizarUsuario(email);
        var usuarioGuid = SanitizadorDadosSensiveis.SanitizarUsuario(guid);

        Assert.Equal("***REDACTED***", usuarioEmail);
        Assert.Equal("***REDACTED***", usuarioGuid);
    }
}
