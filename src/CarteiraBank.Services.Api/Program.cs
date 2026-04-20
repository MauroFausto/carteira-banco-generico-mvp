using System.Diagnostics;
using CarteiraBank.Infra.CrossCutting.Identity;
using CarteiraBank.Infra.CrossCutting.IoC;
using CarteiraBank.Infra.Data.Context;
using CarteiraBank.Services.Api.Middleware;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Context;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.WithProperty("Aplicacao", "carteira-bank-api")
        .Enrich.WithProperty("Ambiente", context.HostingEnvironment.EnvironmentName)
        .WriteTo.File(
            path: "logs/carteira-bank-manual-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 15,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] [Corr:{CorrelationId}] [Trace:{TraceId}] {SourceContext} {Message:lj}{NewLine}{Exception}");
});

builder.Services.AddControllers();
builder.Services.AddOpenApi("v1");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddDependencyInjection(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "Carteira Bank API";
    options.Theme = ScalarTheme.Mars;
});

app.Use(async (context, next) =>
{
    const string correlationHeader = "X-Correlation-Id";
    var correlationId = context.Request.Headers.TryGetValue(correlationHeader, out var headerValue)
        && !string.IsNullOrWhiteSpace(headerValue)
        ? headerValue.ToString()
        : Guid.NewGuid().ToString("N");

    context.TraceIdentifier = correlationId;
    context.Response.Headers[correlationHeader] = correlationId;

    using (LogContext.PushProperty("CorrelationId", correlationId))
    using (LogContext.PushProperty("TraceId", Activity.Current?.TraceId.ToString()))
    {
        await next();
    }
});

app.UseSerilogRequestLogging();
app.UseMiddleware<MiddlewareExcecaoGlobal>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CarteiraBankContext>();
    await db.Database.EnsureCreatedAsync();
    await SeedData.GarantirCargaInicialAsync(db, CancellationToken.None);
}

app.Run();

public partial class Program;
