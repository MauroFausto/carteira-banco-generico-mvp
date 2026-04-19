# Requisitos Funcionais

## Auth e Acesso (RF-AUTH)

- **RF-AUTH-01:** O sistema deve autenticar usuarios via Microsoft Identity.
- **RF-AUTH-02:** O sistema deve aplicar RBAC com duas roles: `Cliente` e `SupervisorGestaoOperacoesCredito`.
- **RF-AUTH-03:** O sistema deve bloquear acesso nao autorizado com retorno apropriado (401/403).

## Credito (RF-CRED)

- **RF-CRED-01:** O cliente deve poder criar solicitacao de credito.
- **RF-CRED-02:** O supervisor deve poder aprovar ou negar solicitacoes.
- **RF-CRED-03:** O sistema deve registrar parecer e motivo de negacao quando aplicavel.
- **RF-CRED-04:** O sistema deve aplicar regras de avaliacao configuradas por carteira.

## Divida (RF-DIV)

- **RF-DIV-01:** O sistema deve calcular saldo devedor a partir de parcelas em aberto.
- **RF-DIV-02:** O sistema deve registrar historico de calculos para rastreabilidade.

## Acordo (RF-ACD)

- **RF-ACD-01:** O supervisor deve poder gerar acordo vinculado a contrato e cliente.
- **RF-ACD-02:** O acordo deve registrar condicoes de negociacao (parcelas, valores e vencimentos).

## Boleto (RF-BOL)

- **RF-BOL-01:** O sistema deve gerar boleto para parcela de contrato/acordo.
- **RF-BOL-02:** O boleto deve ser disponibilizado em PDF.
- **RF-BOL-03:** O download de PDF deve validar ownership ou permissao de supervisor.

## Frontend (RF-FE)

- **RF-FE-01:** O sistema deve disponibilizar painel para cliente com contratos, acordos e boletos.
- **RF-FE-02:** O sistema deve disponibilizar painel para supervisor com fila de solicitacoes e operacoes.
- **RF-FE-03:** O frontend deve usar Bulma CSS como acelerador de desenvolvimento.
