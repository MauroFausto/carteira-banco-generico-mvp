# Referencias para Monolito Modular (.NET)

## Repositorios e Materiais

- [meysamhadeli/booking-modular-monolith](https://github.com/meysamhadeli/booking-modular-monolith)
- [dipjyotisikder/dotnet-modular-monolith](https://github.com/dipjyotisikder/dotnet-modular-monolith)
- [ardalis/modulith](https://github.com/ardalis/modulith)

## Padrões Aproveitaveis

- Fronteiras claras por modulo e contratos.
- Vertical slices por feature.
- Testes de arquitetura para dependencia entre camadas.
- Integracao interna por eventos/contratos, nao por acesso direto a persistencia alheia.

## Decisoes para este projeto

- Monolito modular com um deploy.
- Modulos: Auth, Credito, Divida, Acordo, Boleto, Shared, Web.
- Persistencia em PostgreSQL com EF Core.
- Regras de dependencia documentadas e testadas.
