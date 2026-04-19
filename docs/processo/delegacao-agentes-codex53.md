# Delegacao de Tasks - 7 Agentes

## Premissas

- Execucao unica, sem sprints.
- 7 agentes em paralelo com escopo fechado por dominio.
- Integracao final centralizada.

## Agente 1 - Arquitetura

- **Escopo:** visao arquitetural, estrutura modular, ADRs, sequencias macro.
- **Arquivos alvo:** `docs/arquitetura/visao-arquitetural.md`, `docs/arquitetura/estrutura-modular.md`, `docs/arquitetura/decisoes-arquiteturais.md`, `docs/arquitetura/diagramas-sequencia.md`.
- **Entrada:** requisitos funcionais e restricoes tecnicas.
- **Saida:** padrao arquitetural final e contratos entre modulos.

## Agente 2 - Auth e RBAC

- **Escopo:** autenticacao, autorizacao, matriz de permissoes.
- **Arquivo alvo:** `docs/seguranca/auth-rbac.md`.
- **Entrada:** perfis Cliente e Supervisor.
- **Saida:** roles, policies, claims e fluxo de acesso.

## Agente 3 - Modelo de Dados e ERD

- **Escopo:** entidades de credito, avaliacao, acordo, boleto e auditoria.
- **Arquivos alvo:** `docs/banco-de-dados/erd.md`, `docs/banco-de-dados/dicionario-de-dados.md`.
- **Entrada:** regras de negocio e requisitos de rastreabilidade.
- **Saida:** ERD expandido e base de dicionario.

## Agente 4 - LGPD

- **Escopo:** compliance de privacidade e obrigacoes legais.
- **Arquivo alvo:** `docs/seguranca/lgpd-compliance.md`.
- **Entrada:** dados tratados por modulo.
- **Saida:** mapa de dados, base legal, DSAR, retencao e evidencias.

## Agente 5 - OWASP + Hardening

- **Escopo:** seguranca aplicacional e pipeline seguro.
- **Arquivo alvo:** `docs/seguranca/owasp-checklist.md`.
- **Entrada:** arquitetura, auth e superficie de API.
- **Saida:** checklist priorizado e controles de CI.

## Agente 6 - Boleto PDF sem CDN

- **Escopo:** estrategia de armazenamento e servico seguro de PDF.
- **Arquivo alvo:** `docs/pesquisas/pdf-serving.md`.
- **Entrada:** restricao de nao usar CDN.
- **Saida:** arquitetura recomendada e fluxo seguro de download.

## Agente 7 - Processo, TDD e Referencias

- **Escopo:** praticas XP/TDD, backlog e referencias de implementacao.
- **Arquivos alvo:** `docs/processo/praticas-xp.md`, `docs/processo/backlog-inicial.md`, `docs/pesquisas/tdd-referencias.md`.
- **Entrada:** escopo funcional completo.
- **Saida:** playbook de execucao e governanca de qualidade.

## Matriz de Dependencias

- Agente 1 depende parcialmente de todos os requisitos.
- Agente 2 depende de Agente 1 para padrao arquitetural.
- Agente 3 depende de Agente 1 e negocio.
- Agente 4 depende de Agente 3 para mapa de dados.
- Agente 5 depende de Agente 1 e Agente 2.
- Agente 6 depende de Agente 2 e Agente 3.
- Agente 7 e transversal e valida consistencia global.

## Criticos de Integracao

- Termos e entidades devem ser unificados pelo glossario.
- Roles/policies devem permanecer consistentes em requisitos, API e seguranca.
- Regras de dados pessoais devem aparecer em LGPD, ERD e OWASP sem contradicao.
