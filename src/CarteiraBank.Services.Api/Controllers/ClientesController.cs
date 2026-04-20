using CarteiraBank.Application.Services;
using CarteiraBank.Services.Api.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraBank.Services.Api.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize(Policy = "ClienteOrSupervisor")]
public sealed class ClientesController(IClienteAppService clienteAppService) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetClienteLogado(CancellationToken cancellationToken)
    {
        var clienteId = User.ObterIdUsuario();
        var cliente = await clienteAppService.ObterClienteAsync(clienteId, cancellationToken);
        if (cliente is null)
        {
            throw new ExcecaoEntradaUsuario(
                "Cliente nao encontrado.",
                "Confirme se o identificador do usuario corresponde a um cliente cadastrado.");
        }

        return Ok(cliente);
    }
}
