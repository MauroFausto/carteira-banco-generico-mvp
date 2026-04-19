# Decisoes Arquiteturais (ADRs)

## ADR-001 - Adotar monolito modular

- **Contexto:** MVP com prazo curto e necessidade de fronteiras de dominio claras.
- **Decisao:** usar monolito modular com modulos isolados por contratos.
- **Consequencias:** operacao simples e deploy unico; exige disciplina para evitar acoplamento.

## ADR-002 - Persistencia em PostgreSQL com EF Core

- **Contexto:** necessidade de produtividade, migracoes e rastreabilidade.
- **Decisao:** banco unico PostgreSQL com modelagem por modulo via EF Core.
- **Consequencias:** consistencia transacional e rapidez no desenvolvimento; cuidado com consultas cruzadas.

## ADR-003 - Auth com Microsoft Identity e RBAC

- **Contexto:** dois perfis de acesso com regras operacionais distintas.
- **Decisao:** autenticar via Microsoft Identity e autorizar via policies RBAC.
- **Consequencias:** seguranca padronizada; obrigatorio validar ownership para evitar IDOR/BOLA.

## ADR-004 - PDFs sem CDN

- **Contexto:** restricao explicita de nao usar CDN para boletos.
- **Decisao:** armazenar documentos em storage privado e servir por endpoint autenticado.
- **Consequencias:** maior controle de seguranca e auditoria; requer estrategia de backup e retention.

## ADR-005 - TDD como padrao obrigatorio

- **Contexto:** necessidade de qualidade e evolucao segura em modulo financeiro.
- **Decisao:** adotar TDD desde o inicio, com DoD dependente de testes.
- **Consequencias:** feedback rapido e menor regressao; demanda disciplina tecnica e revisao consistente.
