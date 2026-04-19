# ERD Expandido

## Entidades Principais

- Usuario
- Cliente
- Supervisor
- Carteira
- SolicitacaoCredito
- AvaliacaoCredito
- RegraAvaliacao
- Contrato
- Parcela
- SaldoDevedor
- Acordo
- ParcelaAcordo
- Boleto
- ConsentimentoLGPD
- AuditLog

## Diagrama ERD

```mermaid
erDiagram
    USUARIO ||--o| CLIENTE : "e"
    USUARIO ||--o| SUPERVISOR : "e"
    CLIENTE ||--o{ SOLICITACAO_CREDITO : "solicita"
    SUPERVISOR ||--o{ AVALIACAO_CREDITO : "avalia"
    CARTEIRA ||--o{ REGRA_AVALIACAO : "define"
    SOLICITACAO_CREDITO ||--o{ AVALIACAO_CREDITO : "recebe"
    REGRA_AVALIACAO }o--o{ AVALIACAO_CREDITO : "aplicada em"
    SOLICITACAO_CREDITO ||--o| CONTRATO : "aprovada gera"
    CLIENTE ||--o{ CONTRATO : "possui"
    CONTRATO ||--o{ PARCELA : "tem"
    CONTRATO ||--o{ SALDO_DEVEDOR : "apura"
    CONTRATO ||--o{ ACORDO : "origina"
    ACORDO ||--o{ PARCELA_ACORDO : "divide-se em"
    PARCELA_ACORDO ||--o| BOLETO : "gera"
    CLIENTE ||--o{ CONSENTIMENTO_LGPD : "registra"
    USUARIO ||--o{ AUDIT_LOG : "executa"
```

## Fluxo de Avaliacao de Credito

```mermaid
flowchart TD
    A["Cliente solicita credito"] --> B["Aplicar regras de avaliacao"]
    B --> C{"Score atende limite?"}
    C -->|Sim| D["Aprovacao automatica"]
    C -->|Nao| E["Analise do Supervisor"]
    E --> F{"Aprovar?"}
    F -->|Sim| G["Gerar contrato"]
    F -->|Nao| H["Negar solicitacao com motivo"]
```
