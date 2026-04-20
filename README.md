# Carteira Bank MVP (Equinox-Style)

Reestruturacao para arquitetura em camadas inspirada no Equinox Project, com `.NET 10`.

## Estrutura da solution

- `CarteiraBankMvp.slnx`
- `src/CarteiraBank.Services.Api`
- `src/CarteiraBank.Application`
- `src/CarteiraBank.Domain`
- `src/CarteiraBank.Domain.Core`
- `src/CarteiraBank.Infra.Data`
- `src/CarteiraBank.Infra.CrossCutting.Bus`
- `src/CarteiraBank.Infra.CrossCutting.Identity`
- `src/CarteiraBank.Infra.CrossCutting.IoC`
- `tests/CarteiraBank.Tests.Architecture`
- `tests/CarteiraBank.Tests.Unit`

## Stack tecnica

- ASP.NET Core + Controllers
- OpenAPI nativo (`Microsoft.AspNetCore.OpenApi`) + UI `Scalar.AspNetCore`
- EF Core 10 + PostgreSQL
- FluentValidation
- Serilog + Loki
- NetArchTest para testes de arquitetura
- Sem dependencias NetDevPack
- Sem Swashbuckle

## Execucao local

```bash
export PATH="$HOME/.dotnet:$PATH"
dotnet restore CarteiraBankMvp.slnx
dotnet build CarteiraBankMvp.slnx
dotnet test CarteiraBankMvp.slnx --no-build
dotnet run --project src/CarteiraBank.Services.Api/CarteiraBank.Services.Api.csproj --urls http://localhost:5099
```

Com PostgreSQL (`UseInMemoryDatabase` falso), defina `ConnectionStrings__DefaultConnection` (variavel de ambiente ou configuracao). Na subida da API, o banco relacional recebe `MigrateAsync` (migrations em `src/CarteiraBank.Infra.Data/Migrations`). Com banco em memoria, continua `EnsureCreatedAsync`.

Criar ou aplicar migrations manualmente (requer [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)):

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
export DOTNET_ROOT="${DOTNET_ROOT:-$HOME/.dotnet}"
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=carteira_bank_dev;Username=postgres;Password=postgres"

dotnet ef migrations add NomeDaMigration \
  --project src/CarteiraBank.Infra.Data/CarteiraBank.Infra.Data.csproj \
  --startup-project src/CarteiraBank.Services.Api/CarteiraBank.Services.Api.csproj \
  --context CarteiraBankContext

dotnet ef database update \
  --project src/CarteiraBank.Infra.Data/CarteiraBank.Infra.Data.csproj \
  --startup-project src/CarteiraBank.Services.Api/CarteiraBank.Services.Api.csproj \
  --context CarteiraBankContext
```

Endpoints principais:

- Health: `http://localhost:5099/api/health`
- OpenAPI JSON: `http://localhost:5099/openapi/v1.json`
- Scalar UI: `http://localhost:5099/scalar`

## Docker

O build da imagem usa o diretorio raiz do repositorio como contexto (solution + `src/`), para incluir migrations e referencias de projeto.

Compose para modulo unico:

```bash
docker compose -f docker-compose.modulo.yml up -d --build
```

Compose multi-modulo:

```bash
docker compose \
  -f docker-compose.base.yml \
  -f docker-compose.modulos.yml \
  -f docker-compose.override.yml \
  --env-file .env.example \
  up -d --build
```

## Qualidade e seguranca

- `ops/scripts/secret-scan-staged.sh`
- `ops/scripts/check-dotnet.sh`
- `ops/scripts/run-local-gates.sh`
- `.github/workflows/ci.yml`
