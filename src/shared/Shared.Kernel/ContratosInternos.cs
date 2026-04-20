namespace Shared.Kernel;

public interface IClienteConsultaInterna
{
    Task<bool> ClienteExisteAsync(Guid clienteId, CancellationToken cancellationToken);
}

public interface IContratoConsultaInterna
{
    Task<ContratoResumoInterno?> ObterResumoContratoAsync(Guid contratoId, CancellationToken cancellationToken);
}

public sealed record ContratoResumoInterno(Guid ContratoId, Guid ClienteId, string Status);
