using CarteiraBank.Domain.Core;
using CarteiraBank.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarteiraBank.Infra.Data.Context;

public sealed class CarteiraBankContext(DbContextOptions<CarteiraBankContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<SolicitacaoCredito> SolicitacoesCredito => Set<SolicitacaoCredito>();
    public DbSet<Contrato> Contratos => Set<Contrato>();
    public DbSet<Parcela> Parcelas => Set<Parcela>();
    public DbSet<Acordo> Acordos => Set<Acordo>();
    public DbSet<ParcelaAcordo> ParcelasAcordo => Set<ParcelaAcordo>();
    public DbSet<Boleto> Boletos => Set<Boleto>();
    public DbSet<EventStoreSqlData> EventStore => Set<EventStoreSqlData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarteiraBankContext).Assembly);
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        => await SaveChangesAsync(cancellationToken) > 0;
}

public sealed class EventStoreSqlData
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public DateTime OccurredOn { get; set; }
    public string? User { get; set; }
}
