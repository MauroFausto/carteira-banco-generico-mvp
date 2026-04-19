using CarteiraBank.Api.Domain;

namespace CarteiraBank.Api.Data;

public static class SeedData
{
    public static async Task GarantirCargaInicialAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (dbContext.Clientes.Any())
        {
            return;
        }

        var cliente = new Cliente
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            NomeCompleto = "Cliente Demo",
            Documento = "12345678900",
            Email = "cliente@demo.local"
        };

        var contrato = new Contrato
        {
            Id = Guid.NewGuid(),
            ClienteId = cliente.Id,
            NumeroContrato = $"CTR-{DateTime.UtcNow:yyyyMMdd}-0001",
            ValorPrincipal = 3500m
        };

        contrato.Parcelas.Add(new Parcela
        {
            Numero = 1,
            Valor = 600m,
            ValorJuros = 45m,
            ValorMulta = 0m,
            DataVencimento = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-10)),
            Status = "Aberta"
        });

        contrato.Parcelas.Add(new Parcela
        {
            Numero = 2,
            Valor = 600m,
            ValorJuros = 45m,
            ValorMulta = 0m,
            DataVencimento = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(20)),
            Status = "Aberta"
        });

        dbContext.Clientes.Add(cliente);
        dbContext.Contratos.Add(contrato);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
