# Carteira Banco Generico MVP

Aplicacao full stack em `.NET 10` com:
- API ASP.NET Core (`src/backend/CarteiraBank.Api`)
- Frontend Bulma + JS (`src/frontend`)
- PostgreSQL (`18.3`) para persistencia
- Serilog com logs em arquivo e sink Loki (Grafana)

## 1) Como executar em modo debug

## Opcao A - Debug rapido com CLI

No diretorio da API:

```bash
cd src/backend/CarteiraBank.Api
export PATH="$HOME/.dotnet:$PATH"
dotnet restore
dotnet build
dotnet run --urls http://localhost:5099
```

Acessos:
- Frontend: `http://localhost:5099`
- Health: `http://localhost:5099/api/health`
- OpenAPI: `http://localhost:5099/openapi/v1.json`

## Opcao B - Debug com hot reload

```bash
cd src/backend/CarteiraBank.Api
export PATH="$HOME/.dotnet:$PATH"
dotnet watch run --urls http://localhost:5099
```

## Opcao C - Debug com PostgreSQL real

Suba o PostgreSQL 18.3:

```bash
docker run --name carteira-pg-18 \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=carteira_bank \
  -p 5432:5432 \
  -d postgres:18.3
```

Em `src/backend/CarteiraBank.Api/appsettings.json`:
- `UseInMemoryDatabase`: `false`
- `ConnectionStrings:DefaultConnection`: `Host=localhost;Port=5432;Database=carteira_bank;Username=postgres;Password=postgres`

## Opcao D - Debug sem PostgreSQL (fallback)

Em `src/backend/CarteiraBank.Api/appsettings.json`:
- `UseInMemoryDatabase`: `true`

## Autenticacao em desenvolvimento

Para testes locais, o modo dev aceita headers:
- `X-User-Id`
- `X-User-Role` (`Cliente` ou `Supervisor`)

Essa opcao e controlada por:
- `Autenticacao:HabilitarAuthCabecalhoDev`

## 2) Deploy em container Docker

## 2.1 Dockerfile da API

Arquivo sugerido: `src/backend/CarteiraBank.Api/Dockerfile`

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "CarteiraBank.Api.csproj"
RUN dotnet publish "CarteiraBank.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "CarteiraBank.Api.dll"]
```

## 2.2 Build da imagem

```bash
cd src/backend/CarteiraBank.Api
docker build -t carteira-bank-api:dev .
```

## 2.3 Run da API em container

```bash
docker run --name carteira-bank-api \
  -p 5099:8080 \
  -e UseInMemoryDatabase=false \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=carteira_bank;Username=postgres;Password=postgres" \
  -d carteira-bank-api:dev
```

## 2.4 Logs do container

```bash
docker logs -f carteira-bank-api
```

Logs locais da aplicacao:
- `src/backend/CarteiraBank.Api/logs/`

## 3) Observabilidade (estado atual)

- Serilog ativo com sink de arquivo texto legivel
- Serilog sink Grafana Loki configurado em `appsettings.json` (`http://localhost:3100`)
- Middleware global de excecao com:
  - resposta amigavel para `400` (orientacao de ajuste)
  - resposta generica para `500` (com `traceId`)
  - log tecnico detalhado para diagnostico

## 4) Postman

Arquivos prontos:
- `postman/CarteiraBank-MVP.postman_collection.json`
- `postman/CarteiraBank-MVP.local.postman_environment.json`

## 5) Orquestracao local: Grafana + Loki + Modulo da Aplicacao + PGSQL dedicado

Foi adicionada uma stack de orquestracao para execucao local do modulo atual em:
- `docker-compose.modulo.yml`

Arquivos de apoio:
- `ops/loki/local-config.yaml`
- `ops/grafana/provisioning/datasources/datasource.yml`

### 5.1 Subir a stack

```bash
docker compose -f docker-compose.modulo.yml up -d --build
```

### 5.2 Servicos expostos

- Aplicacao (modulo): `http://localhost:5099`
- Health API: `http://localhost:5099/api/health`
- PostgreSQL dedicado do modulo: `localhost:5432`
- Loki: `http://localhost:3100`
- Grafana: `http://localhost:3000` (usuario `admin`, senha `admin`)

### 5.3 Logs centralizados no Grafana

1. Acesse o Grafana.
2. Abra **Explore**.
3. Selecione o datasource **Loki** (provisionado automaticamente).
4. Consulte com uma query inicial, por exemplo:

```logql
{app="carteira-bank-api"}
```

### 5.4 Derrubar a stack

```bash
docker compose -f docker-compose.modulo.yml down
```

Para remover tambem os volumes persistentes:

```bash
docker compose -f docker-compose.modulo.yml down -v
```
