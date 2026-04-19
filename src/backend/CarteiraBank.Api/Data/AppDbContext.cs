using CarteiraBank.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarteiraBank.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<SolicitacaoCredito> SolicitacoesCredito => Set<SolicitacaoCredito>();
    public DbSet<Contrato> Contratos => Set<Contrato>();
    public DbSet<Parcela> Parcelas => Set<Parcela>();
    public DbSet<Acordo> Acordos => Set<Acordo>();
    public DbSet<ParcelaAcordo> ParcelasAcordo => Set<ParcelaAcordo>();
    public DbSet<Boleto> Boletos => Set<Boleto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("customers");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.NomeCompleto).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Documento).HasMaxLength(18).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200).IsRequired();
            entity.HasIndex(x => x.Documento).IsUnique();
        });

        modelBuilder.Entity<SolicitacaoCredito>(entity =>
        {
            entity.ToTable("credit_applications");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ValorSolicitado).HasColumnType("numeric(18,2)");
            entity.Property(x => x.Status).HasMaxLength(30).IsRequired();
        });

        modelBuilder.Entity<Contrato>(entity =>
        {
            entity.ToTable("contracts");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.NumeroContrato).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ValorPrincipal).HasColumnType("numeric(18,2)");
            entity.HasIndex(x => x.NumeroContrato).IsUnique();
        });

        modelBuilder.Entity<Parcela>(entity =>
        {
            entity.ToTable("installments");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Valor).HasColumnType("numeric(18,2)");
            entity.Property(x => x.ValorJuros).HasColumnType("numeric(18,2)");
            entity.Property(x => x.ValorMulta).HasColumnType("numeric(18,2)");
        });

        modelBuilder.Entity<Acordo>(entity =>
        {
            entity.ToTable("agreements");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ValorTotal).HasColumnType("numeric(18,2)");
            entity.Property(x => x.ValorDesconto).HasColumnType("numeric(18,2)");
        });

        modelBuilder.Entity<ParcelaAcordo>(entity =>
        {
            entity.ToTable("agreement_installments");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Valor).HasColumnType("numeric(18,2)");
        });

        modelBuilder.Entity<Boleto>(entity =>
        {
            entity.ToTable("billets");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CodigoBarras).HasMaxLength(80).IsRequired();
            entity.Property(x => x.LinhaDigitavel).HasMaxLength(80).IsRequired();
        });
    }
}
