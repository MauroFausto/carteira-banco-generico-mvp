using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using CarteiraBank.Api.Data;
using CarteiraBank.Api.Domain;
using CarteiraBank.Api.Features.Auth;
using CarteiraBank.Api.Features.Contracts;
using CarteiraBank.Api.Features.Erros;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Context;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
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

    var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase")
        || string.Equals(Environment.GetEnvironmentVariable("USE_INMEMORY_DATABASE"), "true", StringComparison.OrdinalIgnoreCase);

    var habilitarAuthCabecalhoDev = builder.Configuration.GetValue<bool>("Autenticacao:HabilitarAuthCabecalhoDev");

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        if (useInMemoryDatabase)
        {
            options.UseInMemoryDatabase("carteira_bank_dev");
        }
        else
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        }
    });

    var autenticacaoBuilder = builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultScheme = "AutenticacaoDinamica";
            options.DefaultChallengeScheme = "AutenticacaoDinamica";
        })
        .AddPolicyScheme("AutenticacaoDinamica", "Cabecalho de desenvolvimento ou bearer", options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                if (habilitarAuthCabecalhoDev
                    && context.Request.Headers.ContainsKey("X-User-Id"))
                {
                    return "HeaderAuth";
                }

                return JwtBearerDefaults.AuthenticationScheme;
            };
        });

    autenticacaoBuilder.AddScheme<AuthenticationSchemeOptions, HeaderAuthenticationHandler>("HeaderAuth", _ => { });
    autenticacaoBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var tenantId = builder.Configuration["AzureAd:TenantId"];
        var audience = builder.Configuration["AzureAd:Audience"];
        options.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
        options.Audience = audience;
        options.RequireHttpsMetadata = true;
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("ClienteOnly", policy => policy.RequireRole("Cliente"));
        options.AddPolicy("SupervisorOnly", policy => policy.RequireRole("Supervisor"));
        options.AddPolicy("ClienteOrSupervisor", policy => policy.RequireRole("Cliente", "Supervisor"));
    });

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    });

    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();
    Log.Information("API Carteira Bank iniciada em modo {Ambiente}.", builder.Environment.EnvironmentName);

    app.MapOpenApi();

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

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} respondeu {StatusCode} em {Elapsed:0.0000} ms (Corr:{CorrelationId})";

        options.GetLevel = (_, _, ex) =>
            ex != null ? LogEventLevel.Error : LogEventLevel.Information;
    });

    app.UseMiddleware<MiddlewareExcecaoGlobal>();

    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    var frontendPath = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "..", "frontend"));
    if (Directory.Exists(frontendPath))
    {
        app.UseDefaultFiles(new DefaultFilesOptions
        {
            FileProvider = new PhysicalFileProvider(frontendPath),
            RequestPath = ""
        });
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(frontendPath),
            RequestPath = ""
        });
    }

    await using (var scope = app.Services.CreateAsyncScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureCreatedAsync();
        await SeedData.GarantirCargaInicialAsync(db, CancellationToken.None);
    }

    app.MapGet("/api/health", () => Results.Ok(new { status = "ok", service = "carteira-bank-api" }));

    app.MapGet("/api/clientes/me", async (ClaimsPrincipal usuario, AppDbContext db, ILoggerFactory loggerFactory, CancellationToken ct) =>
            await ExecutarComTratamentoAsync("ObterClienteLogado", loggerFactory, async () =>
            {
                var idUsuario = ObterIdUsuario(usuario);
                var cliente = await db.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == idUsuario, ct);
                if (cliente is null)
                {
                    throw new ExcecaoEntradaUsuario("Cliente nao encontrado.",
                        "Confirme se o identificador do usuario corresponde a um cliente cadastrado.");
                }

                return Results.Ok(new { cliente.Id, cliente.NomeCompleto, cliente.Documento, cliente.Email });
            }))
        .RequireAuthorization("ClienteOrSupervisor");

    app.MapPost("/api/solicitacoes-credito",
            async (ClaimsPrincipal usuario, CriarSolicitacaoCreditoRequisicao requisicao, AppDbContext db, ILoggerFactory loggerFactory, CancellationToken ct) =>
                await ExecutarComTratamentoAsync("CriarSolicitacaoCredito", loggerFactory, async () =>
                {
                    if (requisicao.ValorSolicitado <= 0 || requisicao.QuantidadeParcelasSolicitada <= 0)
                    {
                        throw new ExcecaoEntradaUsuario(
                            "Dados invalidos para criacao da solicitacao.",
                            "Informe valor solicitado e quantidade de parcelas maiores que zero.",
                            [
                                new DetalheErro("valorSolicitado", "Deve ser maior que zero."),
                                new DetalheErro("quantidadeParcelasSolicitada", "Deve ser maior que zero.")
                            ]);
                    }

                    var idUsuario = ObterIdUsuario(usuario);
                    var clienteExiste = await db.Clientes.AnyAsync(x => x.Id == idUsuario, ct);
                    if (!clienteExiste)
                    {
                        throw new ExcecaoEntradaUsuario(
                            "Cliente nao encontrado.",
                            "Verifique o identificador informado em X-User-Id.");
                    }

                    var solicitacao = new SolicitacaoCredito
                    {
                        ClienteId = idUsuario,
                        ValorSolicitado = requisicao.ValorSolicitado,
                        QuantidadeParcelasSolicitada = requisicao.QuantidadeParcelasSolicitada,
                        Finalidade = requisicao.Finalidade,
                        Status = "Pendente"
                    };

                    db.SolicitacoesCredito.Add(solicitacao);
                    await db.SaveChangesAsync(ct);
                    return Results.Ok(new { solicitacao.Id, solicitacao.Status });
                }))
        .RequireAuthorization("ClienteOnly");

    app.MapGet("/api/solicitacoes-credito",
            async (ClaimsPrincipal usuario, AppDbContext db, ILoggerFactory loggerFactory, CancellationToken ct) =>
                await ExecutarComTratamentoAsync("ListarSolicitacoesCredito", loggerFactory, async () =>
                {
                    if (usuario.IsInRole("Supervisor"))
                    {
                        var itens = await db.SolicitacoesCredito
                            .AsNoTracking()
                            .OrderByDescending(x => x.CriadoEm)
                            .Select(x => new
                            {
                                x.Id,
                                x.ClienteId,
                                x.ValorSolicitado,
                                x.QuantidadeParcelasSolicitada,
                                x.Finalidade,
                                x.Status,
                                x.MotivoDecisao,
                                x.CriadoEm
                            })
                            .ToListAsync(ct);
                        return Results.Ok(itens);
                    }

                    var idUsuario = ObterIdUsuario(usuario);
                    var itensDoCliente = await db.SolicitacoesCredito
                        .AsNoTracking()
                        .Where(x => x.ClienteId == idUsuario)
                        .OrderByDescending(x => x.CriadoEm)
                        .Select(x => new
                        {
                            x.Id,
                            x.ValorSolicitado,
                            x.QuantidadeParcelasSolicitada,
                            x.Finalidade,
                            x.Status,
                            x.MotivoDecisao,
                            x.CriadoEm
                        })
                        .ToListAsync(ct);
                    return Results.Ok(itensDoCliente);
                }))
        .RequireAuthorization("ClienteOrSupervisor");

    app.MapPost("/api/solicitacoes-credito/{id:guid}/decisao",
            async (Guid id, ClaimsPrincipal usuario, DecidirSolicitacaoCreditoRequisicao requisicao, AppDbContext db, ILoggerFactory loggerFactory, CancellationToken ct) =>
                await ExecutarComTratamentoAsync("DecidirSolicitacaoCredito", loggerFactory, async () =>
                {
                    var solicitacao = await db.SolicitacoesCredito.FirstOrDefaultAsync(x => x.Id == id, ct);
                    if (solicitacao is null)
                    {
                        throw new ExcecaoEntradaUsuario(
                            "Solicitacao nao encontrada.",
                            "Confirme o identificador da solicitacao e tente novamente.");
                    }

                    solicitacao.Status = requisicao.Aprovar ? "Aprovada" : "Negada";
                    solicitacao.MotivoDecisao = requisicao.Motivo;
                    solicitacao.DecididoEm = DateTime.UtcNow;
                    solicitacao.IdSupervisorDecisor = ObterIdUsuario(usuario);

                    if (requisicao.Aprovar)
                    {
                        var contrato = new Contrato
                        {
                            ClienteId = solicitacao.ClienteId,
                            NumeroContrato = $"CTR-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
                            ValorPrincipal = solicitacao.ValorSolicitado,
                            Status = "Ativo"
                        };

                        var valorParcela = Math.Round(solicitacao.ValorSolicitado / solicitacao.QuantidadeParcelasSolicitada, 2);
                        for (var i = 1; i <= solicitacao.QuantidadeParcelasSolicitada; i++)
                        {
                            contrato.Parcelas.Add(new Parcela
                            {
                                Numero = i,
                                Valor = valorParcela,
                                ValorJuros = Math.Round(valorParcela * 0.03m, 2),
                                ValorMulta = 0m,
                                DataVencimento = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(i)),
                                Status = "Aberta"
                            });
                        }

                        db.Contratos.Add(contrato);
                    }

                    await db.SaveChangesAsync(ct);
                    return Results.Ok(new { solicitacao.Id, solicitacao.Status });
                }))
        .RequireAuthorization("SupervisorOnly");

    app.MapGet("/api/contratos",
            async (ClaimsPrincipal usuario, AppDbContext db, ILoggerFactory loggerFactory, CancellationToken ct) =>
                await ExecutarComTratamentoAsync("ListarContratos", loggerFactory, async () =>
                {
                    if (usuario.IsInRole("Supervisor"))
                    {
                        var contratos = await db.Contratos.AsNoTracking().Select(x => new
                        {
                            x.Id,
                            x.ClienteId,
                            x.NumeroContrato,
                            x.ValorPrincipal,
                            x.Status,
                            QuantidadeParcelasAbertas = x.Parcelas.Count(i => i.Status == "Aberta")
                        }).ToListAsync(ct);
                        return Results.Ok(contratos);
                    }

                    var idUsuario = ObterIdUsuario(usuario);
                    var contratosCliente = await db.Contratos.AsNoTracking().Where(x => x.ClienteId == idUsuario).Select(x => new
                    {
                        x.Id,
                        x.NumeroContrato,
                        x.ValorPrincipal,
                        x.Status,
                        QuantidadeParcelasAbertas = x.Parcelas.Count(i => i.Status == "Aberta")
                    }).ToListAsync(ct);

                    return Results.Ok(contratosCliente);
                }))
        .RequireAuthorization("ClienteOrSupervisor");

    app.MapGet("/api/contratos/{id:guid}/saldo-devedor",
            async (Guid id, ClaimsPrincipal usuario, AppDbContext db, ILoggerFactory loggerFactory, CancellationToken ct) =>
                await ExecutarComTratamentoAsync("CalcularSaldoDevedor", loggerFactory, async () =>
                {
                    var contrato = await db.Contratos
                        .Include(x => x.Parcelas)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id, ct);

                    if (contrato is null)
                    {
                        throw new ExcecaoEntradaUsuario("Contrato nao encontrado.",
                            "Verifique o identificador do contrato.");
                    }

                    if (!usuario.IsInRole("Supervisor") && contrato.ClienteId != ObterIdUsuario(usuario))
                    {
                        return Results.Forbid();
                    }

                    var parcelasAbertas = contrato.Parcelas.Where(x => x.Status == "Aberta").ToList();
                    var principal = parcelasAbertas.Sum(x => x.Valor);
                    var juros = parcelasAbertas.Sum(x => x.ValorJuros);
                    var multa = parcelasAbertas.Sum(x => x.ValorMulta);
                    var total = principal + juros + multa;

                    return Results.Ok(new
                    {
                        contrato.Id,
                        contrato.NumeroContrato,
                        principal,
                        juros,
                        multa,
                        total,
                        calculadoEm = DateTime.UtcNow
                    });
                }))
        .RequireAuthorization("ClienteOrSupervisor");

    app.MapPost("/api/contratos/{id:guid}/acordos",
            async (Guid id, ClaimsPrincipal usuario, CriarAcordoRequisicao requisicao, AppDbContext db, ILoggerFactory loggerFactory, CancellationToken ct) =>
                await ExecutarComTratamentoAsync("CriarAcordo", loggerFactory, async () =>
                {
                    if (requisicao.QuantidadeParcelas <= 0)
                    {
                        throw new ExcecaoEntradaUsuario("Quantidade de parcelas invalida.",
                            "Informe uma quantidade de parcelas maior que zero.",
                            [new DetalheErro("quantidadeParcelas", "Deve ser maior que zero.")]);
                    }

                    var contrato = await db.Contratos.Include(x => x.Parcelas).FirstOrDefaultAsync(x => x.Id == id, ct);
                    if (contrato is null)
                    {
                        throw new ExcecaoEntradaUsuario("Contrato nao encontrado.",
                            "Verifique o identificador e tente novamente.");
                    }

                    var parcelasAbertas = contrato.Parcelas.Where(x => x.Status == "Aberta").ToList();
                    var valorBase = parcelasAbertas.Sum(x => x.Valor + x.ValorJuros + x.ValorMulta);
                    var valorComDesconto = Math.Max(0, valorBase - requisicao.ValorDesconto);
                    var valorParcela = Math.Round(valorComDesconto / requisicao.QuantidadeParcelas, 2);

                    var acordo = new Acordo
                    {
                        ContratoId = contrato.Id,
                        ValorTotal = valorComDesconto,
                        ValorDesconto = requisicao.ValorDesconto,
                        QuantidadeParcelas = requisicao.QuantidadeParcelas,
                        Status = "Ativo"
                    };

                    for (var i = 1; i <= requisicao.QuantidadeParcelas; i++)
                    {
                        acordo.Parcelas.Add(new ParcelaAcordo
                        {
                            Numero = i,
                            Valor = valorParcela,
                            DataVencimento = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(i)),
                            Status = "Aberta"
                        });
                    }

                    db.Acordos.Add(acordo);
                    await db.SaveChangesAsync(ct);
                    return Results.Ok(new { acordo.Id, acordo.ValorTotal, acordo.QuantidadeParcelas });
                }))
        .RequireAuthorization("SupervisorOnly");

    app.MapPost("/api/acordos/{id:guid}/boleto",
            async (Guid id, AppDbContext db, ILoggerFactory loggerFactory, CancellationToken ct) =>
                await ExecutarComTratamentoAsync("EmitirBoleto", loggerFactory, async () =>
                {
                    var parcelaAcordo = await db.ParcelasAcordo.FirstOrDefaultAsync(x => x.Id == id, ct);
                    if (parcelaAcordo is null)
                    {
                        throw new ExcecaoEntradaUsuario("Parcela do acordo nao encontrada.",
                            "Verifique o identificador da parcela do acordo.");
                    }

                    var boleto = new Boleto
                    {
                        ParcelaAcordoId = parcelaAcordo.Id,
                        CodigoBarras = $"{Random.Shared.NextInt64(100000000000, 999999999999)}",
                        LinhaDigitavel = $"{Random.Shared.NextInt64(10000, 99999)}.{Random.Shared.NextInt64(10000, 99999)}"
                    };

                    db.Boletos.Add(boleto);
                    await db.SaveChangesAsync(ct);
                    return Results.Ok(new { boleto.Id, boleto.CodigoBarras, boleto.LinhaDigitavel });
                }))
        .RequireAuthorization("SupervisorOnly");

    app.MapGet("/api/boletos/{id:guid}/pdf",
            async (Guid id, ClaimsPrincipal usuario, AppDbContext db, ILoggerFactory loggerFactory, CancellationToken ct) =>
                await ExecutarComTratamentoAsync("BaixarBoletoPdf", loggerFactory, async () =>
                {
                    var boleto = await db.Boletos
                        .Include(x => x.ParcelaAcordo)
                        .ThenInclude(x => x!.Acordo)
                        .ThenInclude(x => x!.Contrato)
                        .FirstOrDefaultAsync(x => x.Id == id, ct);

                    if (boleto is null)
                    {
                        throw new ExcecaoEntradaUsuario("Boleto nao encontrado.",
                            "Confirme o identificador do boleto.");
                    }

                    var idClienteDono = boleto.ParcelaAcordo?.Acordo?.Contrato?.ClienteId;
                    if (!usuario.IsInRole("Supervisor") && idClienteDono != ObterIdUsuario(usuario))
                    {
                        return Results.Forbid();
                    }

                    var pdf = GerarPdfSimples($"Boleto {boleto.Id}\nLinha Digitavel: {boleto.LinhaDigitavel}");
                    return Results.File(pdf, "application/pdf", $"boleto-{id}.pdf");
                }))
        .RequireAuthorization("ClienteOrSupervisor");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Falha fatal na inicializacao da API.");
    Console.Error.WriteLine(ex);
    throw;
}
finally
{
    Log.CloseAndFlush();
}

static async Task<IResult> ExecutarComTratamentoAsync(string operacao, ILoggerFactory loggerFactory, Func<Task<IResult>> acao)
{
    var logger = loggerFactory.CreateLogger("OperacoesNegocio");
    try
    {
        logger.LogInformation("Iniciando operacao {Operacao}.", operacao);
        return await acao();
    }
    catch
    {
        logger.LogError("Falha durante a operacao {Operacao}.", operacao);
        throw;
    }
    finally
    {
        logger.LogInformation("Finalizada operacao {Operacao}.", operacao);
    }
}

static Guid ObterIdUsuario(ClaimsPrincipal usuario)
{
    var raw = usuario.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? usuario.FindFirstValue("oid")
              ?? usuario.FindFirstValue("sub");

    return Guid.TryParse(raw, out var value)
        ? value
        : Guid.Parse("11111111-1111-1111-1111-111111111111");
}

static byte[] GerarPdfSimples(string mensagem)
{
    var sanitizado = mensagem.Replace("(", "[").Replace(")", "]");
    var pdf = $"""
              %PDF-1.1
              1 0 obj<</Type/Catalog/Pages 2 0 R>>endobj
              2 0 obj<</Type/Pages/Kids[3 0 R]/Count 1>>endobj
              3 0 obj<</Type/Page/Parent 2 0 R/MediaBox[0 0 612 792]/Contents 4 0 R/Resources<</Font<</F1 5 0 R>>>>>>endobj
              4 0 obj<</Length 44>>stream
              BT /F1 12 Tf 72 720 Td ({sanitizado}) Tj ET
              endstream endobj
              5 0 obj<</Type/Font/Subtype/Type1/BaseFont/Helvetica>>endobj
              xref
              0 6
              0000000000 65535 f
              0000000010 00000 n
              0000000062 00000 n
              0000000118 00000 n
              0000000246 00000 n
              0000000348 00000 n
              trailer<</Root 1 0 R/Size 6>>
              startxref
              420
              %%EOF
              """;
    return Encoding.ASCII.GetBytes(pdf);
}
