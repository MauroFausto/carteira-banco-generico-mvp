using CarteiraBank.Domain.Core;
using CarteiraBank.Domain.Models;

namespace CarteiraBank.Domain.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}

public interface ICreditoRepository : IRepository<SolicitacaoCredito>
{
    Task AddAsync(SolicitacaoCredito solicitacao, CancellationToken cancellationToken);
    Task<SolicitacaoCredito?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<SolicitacaoCredito>> ListByClienteAsync(Guid clienteId, CancellationToken cancellationToken);
    Task<List<SolicitacaoCredito>> ListAllAsync(CancellationToken cancellationToken);
}

public interface IContratoRepository : IRepository<Contrato>
{
    Task AddAsync(Contrato contrato, CancellationToken cancellationToken);
    Task<Contrato?> GetByIdWithParcelasAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Contrato>> ListByClienteAsync(Guid clienteId, CancellationToken cancellationToken);
    Task<List<Contrato>> ListAllAsync(CancellationToken cancellationToken);
}

public interface IAcordoRepository : IRepository<Acordo>
{
    Task AddAsync(Acordo acordo, CancellationToken cancellationToken);
    Task<ParcelaAcordo?> GetParcelaAcordoByIdAsync(Guid id, CancellationToken cancellationToken);
}

public interface IBoletoRepository : IRepository<Boleto>
{
    Task AddAsync(Boleto boleto, CancellationToken cancellationToken);
    Task<Boleto?> GetByIdWithContratoAsync(Guid id, CancellationToken cancellationToken);
}
