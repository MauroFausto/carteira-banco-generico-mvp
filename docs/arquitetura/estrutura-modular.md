# Estrutura Modular

## Estrutura Sugerida

```text
src/
├── Shared/
│   ├── Shared.Kernel/
│   └── Shared.Infrastructure/
├── Modules/
│   ├── Auth/
│   │   ├── Auth.Domain/
│   │   ├── Auth.Application/
│   │   └── Auth.Infrastructure/
│   ├── Credito/
│   ├── Divida/
│   ├── Acordo/
│   └── Boleto/
├── Web/
└── Tests/
    ├── UnitTests/
    ├── IntegrationTests/
    └── ArchitectureTests/
```

## Regras de Dependencia

- `Domain` nao depende de `Application` ou `Infrastructure`.
- `Application` depende de `Domain` e `Contracts`.
- `Infrastructure` implementa portas definidas por `Application`.
- Um modulo nao acessa banco/tabelas de outro modulo diretamente.
- Integracao entre modulos deve ocorrer via contratos internos e eventos.

## Regras de Governanca

- Cada modulo deve ter testes unitarios proprios.
- Cada modulo deve explicitar suas APIs internas.
- Mudancas que atravessam modulos exigem ADR.
