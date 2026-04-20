using System.Text.Json;
using CarteiraBank.Domain.Core;
using CarteiraBank.Domain.Events;
using CarteiraBank.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarteiraBank.Infra.Data.Context;

public static class SeedData
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = false };

    public static async Task GarantirCargaInicialAsync(CarteiraBankContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Clientes.AnyAsync(cancellationToken))
        {
            return;
        }

        var rng = new Random(20260420);
        var supervisorId = Guid.Parse("99999999-9999-9999-9999-999999999999");
        var baseUtc = DateTime.UtcNow;

        var clientes = new List<Cliente>
        {
            new(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Cliente Demo",
                "12345678900",
                "cliente@demo.local"),
            new(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "Maria Silva",
                "23456789001",
                "maria.silva@demo.local"),
            new(
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                "Joao Santos",
                "34567890012",
                "joao.santos@demo.local"),
            new(
                Guid.Parse("44444444-4444-4444-4444-444444444444"),
                "Ana Costa",
                "45678901023",
                "ana.costa@demo.local"),
            new(
                Guid.Parse("55555555-5555-5555-5555-555555555555"),
                "Pedro Oliveira",
                "56789012034",
                "pedro.oliveira@demo.local")
        };

        var finalidades = new[]
        {
            "Capital de giro",
            "Equipamentos",
            "Reforma de ponto comercial",
            "Expansao de frota",
            "Estoque",
            "Tecnologia e software"
        };

        var solicitacoes = new List<SolicitacaoCredito>();
        var aprovadas = new List<SolicitacaoCredito>();

        foreach (var cliente in clientes)
        {
            for (var i = 0; i < 10; i++)
            {
                var qParcelas = 6 + rng.Next(7);
                var valor = Math.Round((decimal)(2500 + rng.NextDouble() * 12000), 2);
                var sol = new SolicitacaoCredito(
                    cliente.Id,
                    valor,
                    qParcelas,
                    finalidades[rng.Next(finalidades.Length)]);

                if (i < 2)
                {
                    // Pendente
                }
                else if (i < 6)
                {
                    sol.Negar(supervisorId, "Politica interna: risco acima do limite.");
                }
                else
                {
                    sol.Aprovar(supervisorId, "Analise de credito aprovada.");
                    aprovadas.Add(sol);
                }

                solicitacoes.Add(sol);
            }
        }

        if (aprovadas.Count != 20)
        {
            throw new InvalidOperationException($"Seed esperava 20 solicitacoes aprovadas, obteve {aprovadas.Count}.");
        }

        var contratos = new List<Contrato>();
        for (var idx = 0; idx < aprovadas.Count; idx++)
        {
            var sol = aprovadas[idx];
            var contrato = new Contrato(
                sol.ClienteId,
                $"CTR-SEED-20260420-{idx + 1:D4}",
                sol.ValorSolicitado);

            var n = sol.QuantidadeParcelasSolicitada;
            var valorParcela = Math.Round(sol.ValorSolicitado / n, 2);
            var acumulado = 0m;
            for (var p = 1; p <= n; p++)
            {
                var valorP = p == n ? sol.ValorSolicitado - acumulado : valorParcela;
                acumulado += valorP;
                contrato.Parcelas.Add(new Parcela(
                    p,
                    valorP,
                    Math.Round(valorP * 0.03m, 2),
                    0m,
                    DateOnly.FromDateTime(baseUtc.Date.AddMonths(p))));
            }

            contratos.Add(contrato);
        }

        var acordos = new List<Acordo>();
        var boletoSeq = 0;

        Acordo CriarAcordoPrincipal(Contrato contrato)
        {
            var openSum = contrato.Parcelas.Sum(x => x.Valor + x.ValorJuros + x.ValorMulta);
            var desconto = Math.Round(openSum * (0.03m + (decimal)rng.NextDouble() * 0.07m), 2);
            var valorComDesconto = Math.Max(0, openSum - desconto);
            var qtd = 4 + rng.Next(4);
            var valorParcela = Math.Round(valorComDesconto / qtd, 2);
            var acordo = new Acordo(contrato.Id, valorComDesconto, qtd, desconto);
            for (var i = 1; i <= qtd; i++)
            {
                acordo.Parcelas.Add(new ParcelaAcordo(
                    i,
                    valorParcela,
                    DateOnly.FromDateTime(baseUtc.Date.AddMonths(i + 1))));
            }

            return acordo;
        }

        Acordo CriarAcordoComplementar(Contrato contrato)
        {
            var openSum = contrato.Parcelas.Sum(x => x.Valor + x.ValorJuros + x.ValorMulta);
            var valorTotal = Math.Round(openSum * 0.22m, 2);
            var qtd = 3;
            var desconto = 0m;
            var valorParcela = Math.Round(valorTotal / qtd, 2);
            var acordo = new Acordo(contrato.Id, valorTotal, qtd, desconto);
            for (var i = 1; i <= qtd; i++)
            {
                acordo.Parcelas.Add(new ParcelaAcordo(
                    i,
                    valorParcela,
                    DateOnly.FromDateTime(baseUtc.Date.AddMonths(i + 18))));
            }

            return acordo;
        }

        foreach (var contrato in contratos)
        {
            acordos.Add(CriarAcordoPrincipal(contrato));
        }

        for (var i = 0; i < 10; i++)
        {
            acordos.Add(CriarAcordoComplementar(contratos[i]));
        }

        var boletos = new List<Boleto>();
        foreach (var pa in acordos.SelectMany(a => a.Parcelas))
        {
            boletoSeq++;
            var codigo = $"34191{baseUtc:yyyyMMdd}{boletoSeq:D10}".PadRight(44, '0')[..44];
            var linha = $"{codigo[..5]}.{codigo.Substring(5, 5)} {codigo.Substring(10, 5)}.{codigo.Substring(15, 6)} {codigo.Substring(21, 1)} {codigo.Substring(22)}";
            linha = linha.Length <= 80 ? linha : linha[..80];
            boletos.Add(new Boleto(pa.Id, codigo, linha));
        }

        dbContext.Clientes.AddRange(clientes);
        dbContext.SolicitacoesCredito.AddRange(solicitacoes);
        dbContext.Contratos.AddRange(contratos);
        dbContext.Acordos.AddRange(acordos);
        dbContext.Boletos.AddRange(boletos);

        var t = 0;
        var eventos = new List<EventStoreSqlData>();

        void AddEvent(Event domainEvent, string? user = "seed")
        {
            t++;
            var occurred = baseUtc.AddSeconds(t);
            var type = domainEvent.GetType().FullName ?? domainEvent.GetType().Name;
            var data = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), JsonOptions);
            eventos.Add(new EventStoreSqlData
            {
                Id = Guid.NewGuid(),
                Type = type,
                Data = data,
                OccurredOn = occurred,
                User = user
            });
        }

        foreach (var s in solicitacoes)
        {
            AddEvent(new CreditRequestCreatedEvent(s.Id, s.ClienteId));
        }

        foreach (var s in solicitacoes.Where(x => x.Status == "Negada"))
        {
            AddEvent(new CreditDeniedEvent(s.Id));
        }

        for (var i = 0; i < aprovadas.Count; i++)
        {
            AddEvent(new CreditApprovedEvent(aprovadas[i].Id, contratos[i].Id));
        }

        foreach (var a in acordos)
        {
            AddEvent(new AgreementCreatedEvent(a.Id, a.ContratoId));
        }

        foreach (var b in boletos)
        {
            AddEvent(new BilletIssuedEvent(b.Id, b.ParcelaAcordoId));
        }

        dbContext.EventStore.AddRange(eventos);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
