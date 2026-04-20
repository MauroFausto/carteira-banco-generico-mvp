using CarteiraBank.Application.Commands;
using CarteiraBank.Application.Services;
using CarteiraBank.Domain.Commands;
using CarteiraBank.Domain.Core;
using CarteiraBank.Domain.Interfaces;
using CarteiraBank.Infra.CrossCutting.Bus;
using CarteiraBank.Infra.Data.Context;
using CarteiraBank.Infra.Data.EventSourcing;
using CarteiraBank.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarteiraBank.Infra.CrossCutting.IoC;

public static class NativeInjectorBootStrapper
{
    public static IServiceCollection AddDependencyInjection(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var useInMemoryDatabase = configuration.GetValue<bool>("UseInMemoryDatabase")
            || string.Equals(Environment.GetEnvironmentVariable("USE_INMEMORY_DATABASE"), "true", StringComparison.OrdinalIgnoreCase);

        services.AddDbContext<CarteiraBankContext>(options =>
        {
            if (useInMemoryDatabase)
            {
                options.UseInMemoryDatabase("carteira_bank_dev");
                return;
            }

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IMediatorHandler, InMemoryBus>();
        services.AddScoped<IEventStore, SqlEventStore>();

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ICreditoRepository, CreditoRepository>();
        services.AddScoped<IContratoRepository, ContratoRepository>();
        services.AddScoped<IAcordoRepository, AcordoRepository>();
        services.AddScoped<IBoletoRepository, BoletoRepository>();

        services.AddScoped<IRequestHandler<RegisterCreditRequestCommand>, RegisterCreditRequestCommandHandler>();
        services.AddScoped<IRequestHandler<DecideCreditRequestCommand>, DecideCreditRequestCommandHandler>();
        services.AddScoped<IRequestHandler<CreateAgreementCommand>, CreateAgreementCommandHandler>();
        services.AddScoped<IRequestHandler<IssueBilletCommand>, IssueBilletCommandHandler>();

        services.AddScoped<IClienteAppService, ClienteAppService>();
        services.AddScoped<ICreditoAppService, CreditoAppService>();
        services.AddScoped<IAcordoBoletoAppService, AcordoBoletoAppService>();

        return services;
    }
}
