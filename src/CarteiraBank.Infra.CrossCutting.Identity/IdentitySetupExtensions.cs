using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarteiraBank.Infra.CrossCutting.Identity;

public static class IdentitySetupExtensions
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var habilitarAuthCabecalhoDev = configuration.GetValue<bool>("Autenticacao:HabilitarAuthCabecalhoDev");

        var autenticacaoBuilder = services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = "AutenticacaoDinamica";
                options.DefaultChallengeScheme = "AutenticacaoDinamica";
            })
            .AddPolicyScheme("AutenticacaoDinamica", "Cabecalho de desenvolvimento ou bearer", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    if (habilitarAuthCabecalhoDev && context.Request.Headers.ContainsKey("X-User-Id"))
                    {
                        return "HeaderAuth";
                    }

                    return JwtBearerDefaults.AuthenticationScheme;
                };
            });

        autenticacaoBuilder.AddScheme<AuthenticationSchemeOptions, HeaderAuthenticationHandler>("HeaderAuth", _ => { });
        autenticacaoBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var tenantId = configuration["AzureAd:TenantId"];
            var audience = configuration["AzureAd:Audience"];
            options.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
            options.Audience = audience;
            options.RequireHttpsMetadata = true;
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ClienteOnly", policy => policy.RequireRole("Cliente"));
            options.AddPolicy("SupervisorOnly", policy => policy.RequireRole("Supervisor"));
            options.AddPolicy("ClienteOrSupervisor", policy => policy.RequireRole("Cliente", "Supervisor"));
        });

        return services;
    }
}
