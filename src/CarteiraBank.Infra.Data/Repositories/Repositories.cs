using CarteiraBank.Domain.Core;
using CarteiraBank.Domain.Interfaces;
using CarteiraBank.Domain.Models;
using CarteiraBank.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarteiraBank.Infra.Data.Repositories;

public sealed class ClienteRepository(CarteiraBankContext context) : IClienteRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Task<Cliente?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => context.Clientes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}

public sealed class CreditoRepository(CarteiraBankContext context) : ICreditoRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Task AddAsync(SolicitacaoCredito solicitacao, CancellationToken cancellationToken)
        => context.SolicitacoesCredito.AddAsync(solicitacao, cancellationToken).AsTask();

    public Task<SolicitacaoCredito?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => context.SolicitacoesCredito.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<List<SolicitacaoCredito>> ListByClienteAsync(Guid clienteId, CancellationToken cancellationToken)
        => context.SolicitacoesCredito
            .Where(x => x.ClienteId == clienteId)
            .OrderByDescending(x => x.CriadoEm)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<List<SolicitacaoCredito>> ListAllAsync(CancellationToken cancellationToken)
        => context.SolicitacoesCredito
            .OrderByDescending(x => x.CriadoEm)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}

public sealed class ContratoRepository(CarteiraBankContext context) : IContratoRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Task AddAsync(Contrato contrato, CancellationToken cancellationToken)
        => context.Contratos.AddAsync(contrato, cancellationToken).AsTask();

    public Task<Contrato?> GetByIdWithParcelasAsync(Guid id, CancellationToken cancellationToken)
        => context.Contratos.Include(x => x.Parcelas)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<List<Contrato>> ListByClienteAsync(Guid clienteId, CancellationToken cancellationToken)
        => context.Contratos.Include(x => x.Parcelas)
            .Where(x => x.ClienteId == clienteId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<List<Contrato>> ListAllAsync(CancellationToken cancellationToken)
        => context.Contratos.Include(x => x.Parcelas)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}

public sealed class AcordoRepository(CarteiraBankContext context) : IAcordoRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Task AddAsync(Acordo acordo, CancellationToken cancellationToken)
        => context.Acordos.AddAsync(acordo, cancellationToken).AsTask();

    public Task<ParcelaAcordo?> GetParcelaAcordoByIdAsync(Guid id, CancellationToken cancellationToken)
        => context.ParcelasAcordo.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}

public sealed class BoletoRepository(CarteiraBankContext context) : IBoletoRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Task AddAsync(Boleto boleto, CancellationToken cancellationToken)
        => context.Boletos.AddAsync(boleto, cancellationToken).AsTask();

    public Task<Boleto?> GetByIdWithContratoAsync(Guid id, CancellationToken cancellationToken)
        => context.Boletos
            .Include(x => x.ParcelaAcordo)
            .ThenInclude(x => x!.Acordo)
            .ThenInclude(x => x!.Contrato)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
