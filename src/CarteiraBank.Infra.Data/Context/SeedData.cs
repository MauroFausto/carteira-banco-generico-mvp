using CarteiraBank.Domain.Models;

namespace CarteiraBank.Infra.Data.Context;

public static class SeedData
{
    public static async Task GarantirCargaInicialAsync(CarteiraBankContext dbContext, CancellationToken cancellationToken)
    {
        if (dbContext.Clientes.Any())
        {
            return;
        }

        var cliente = new Cliente(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Cliente Demo",
            "12345678900",
            "cliente@demo.local");

        var contrato = new Contrato(
            cliente.Id,
            $"CTR-{DateTime.UtcNow:yyyyMMdd}-0001",
            3500m);

        contrato.Parcelas.Add(new Parcela(
            1,
            600m,
            45m,
            0m,
            DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-10))));

        contrato.Parcelas.Add(new Parcela(
            2,
            600m,
            45m,
            0m,
            DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(20))));

        dbContext.Clientes.Add(cliente);
        dbContext.Contratos.Add(contrato);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
