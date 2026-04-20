using CarteiraBank.Domain.Models;
using CarteiraBank.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarteiraBank.Infra.Data.Mappings;

public sealed class ClienteMapping : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.NomeCompleto).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Documento).HasMaxLength(18).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => x.Documento).IsUnique();
    }
}

public sealed class SolicitacaoCreditoMapping : IEntityTypeConfiguration<SolicitacaoCredito>
{
    public void Configure(EntityTypeBuilder<SolicitacaoCredito> builder)
    {
        builder.ToTable("credit_applications");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ValorSolicitado).HasColumnType("numeric(18,2)");
        builder.Property(x => x.Status).HasMaxLength(30).IsRequired();
    }
}

public sealed class ContratoMapping : IEntityTypeConfiguration<Contrato>
{
    public void Configure(EntityTypeBuilder<Contrato> builder)
    {
        builder.ToTable("contracts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.NumeroContrato).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ValorPrincipal).HasColumnType("numeric(18,2)");
        builder.HasIndex(x => x.NumeroContrato).IsUnique();
    }
}

public sealed class ParcelaMapping : IEntityTypeConfiguration<Parcela>
{
    public void Configure(EntityTypeBuilder<Parcela> builder)
    {
        builder.ToTable("installments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Valor).HasColumnType("numeric(18,2)");
        builder.Property(x => x.ValorJuros).HasColumnType("numeric(18,2)");
        builder.Property(x => x.ValorMulta).HasColumnType("numeric(18,2)");
    }
}

public sealed class AcordoMapping : IEntityTypeConfiguration<Acordo>
{
    public void Configure(EntityTypeBuilder<Acordo> builder)
    {
        builder.ToTable("agreements");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ValorTotal).HasColumnType("numeric(18,2)");
        builder.Property(x => x.ValorDesconto).HasColumnType("numeric(18,2)");
    }
}

public sealed class ParcelaAcordoMapping : IEntityTypeConfiguration<ParcelaAcordo>
{
    public void Configure(EntityTypeBuilder<ParcelaAcordo> builder)
    {
        builder.ToTable("agreement_installments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Valor).HasColumnType("numeric(18,2)");
    }
}

public sealed class BoletoMapping : IEntityTypeConfiguration<Boleto>
{
    public void Configure(EntityTypeBuilder<Boleto> builder)
    {
        builder.ToTable("billets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CodigoBarras).HasMaxLength(80).IsRequired();
        builder.Property(x => x.LinhaDigitavel).HasMaxLength(80).IsRequired();
    }
}

public sealed class EventStoreSqlDataMapping : IEntityTypeConfiguration<EventStoreSqlData>
{
    public void Configure(EntityTypeBuilder<EventStoreSqlData> builder)
    {
        builder.ToTable("stored_events");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Data).HasColumnType("text").IsRequired();
        builder.Property(x => x.User).HasMaxLength(200);
    }
}
