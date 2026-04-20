using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarteiraBank.Infra.Data.Context;

/// <summary>Design-time para <c>dotnet ef</c> (connection string via <c>ConnectionStrings__DefaultConnection</c> ou fallback local).</summary>
public sealed class CarteiraBankContextFactory : IDesignTimeDbContextFactory<CarteiraBankContext>
{
    public CarteiraBankContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=carteira_bank_dev;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<CarteiraBankContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new CarteiraBankContext(optionsBuilder.Options);
    }
}
