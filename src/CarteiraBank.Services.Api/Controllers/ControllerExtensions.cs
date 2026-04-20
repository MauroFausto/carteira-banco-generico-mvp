using System.Security.Claims;

namespace CarteiraBank.Services.Api.Controllers;

internal static class ControllerExtensions
{
    internal static Guid ObterIdUsuario(this ClaimsPrincipal usuario)
    {
        var raw = usuario.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? usuario.FindFirstValue("oid")
                  ?? usuario.FindFirstValue("sub");

        return Guid.TryParse(raw, out var value)
            ? value
            : Guid.Parse("11111111-1111-1111-1111-111111111111");
    }
}
