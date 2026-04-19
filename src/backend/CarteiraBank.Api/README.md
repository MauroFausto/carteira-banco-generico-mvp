# CarteiraBank API

## Stack

- ASP.NET Core (.NET 10)
- EF Core + PostgreSQL
- RBAC por policies (`Cliente`, `Supervisor`)
- Frontend Bulma servido de `src/frontend`

## Banco de dados (desenvolvimento)

- O projeto vem com `UseInMemoryDatabase: false` em `appsettings.json` para usar PostgreSQL.
- PostgreSQL `18.3` foi provisionado via Docker local:

```bash
docker run --name carteira-pg-18 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=carteira_bank -p 5432:5432 -d postgres:18.3
```

- Para fallback local sem banco, altere `UseInMemoryDatabase` para `true`.

## Observacao sobre Auth

Para desenvolvimento local, a autenticacao pode usar cabecalhos (controlada por `Autenticacao:HabilitarAuthCabecalhoDev`):
- `X-User-Id`
- `X-User-Role` (`Cliente` ou `Supervisor`)

Em producao, o caminho esperado e Microsoft Identity via `JwtBearer`.

## Como executar

1. Configure `ConnectionStrings:DefaultConnection` em `appsettings.json`.
2. Rode a API no diretorio deste projeto:

```bash
dotnet restore
dotnet run
```

3. Abra `http://localhost:5099` para acessar o frontend.
4. OpenAPI em `/openapi/v1.json`.
