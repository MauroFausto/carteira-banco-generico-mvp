# Criterios de Aceite (BDD)

## CA-AUTH-001 - Login com Microsoft Identity

**Given** usuario valido sem sessao ativa  
**When** acessa rota protegida  
**Then** deve ser redirecionado para autenticacao e receber acesso apos token valido

## CA-AUTH-002 - Bloqueio por role

**Given** usuario com role `Cliente`  
**When** tenta aprovar solicitacao de credito  
**Then** deve receber `403 Forbidden`

## CA-CRED-001 - Criar solicitacao de credito

**Given** cliente autenticado e dados validos  
**When** envia nova solicitacao  
**Then** sistema deve persistir solicitacao com status inicial e trilha de auditoria

## CA-CRED-002 - Negacao exige motivo

**Given** solicitacao em analise  
**When** supervisor nega o pedido  
**Then** sistema deve exigir e registrar motivo de negacao

## CA-DIV-001 - Calculo de saldo

**Given** contrato com parcelas em aberto  
**When** sistema executa calculo  
**Then** deve gerar snapshot de saldo com principal, juros, multa e total

## CA-ACD-001 - Geracao de acordo

**Given** contrato elegivel para negociacao  
**When** supervisor formaliza acordo  
**Then** sistema deve criar acordo e parcelas de acordo com condicoes definidas

## CA-BOL-001 - Emissao de boleto

**Given** parcela de acordo valida  
**When** sistema emite boleto  
**Then** deve gerar PDF e vincular metadados do documento a parcela

## CA-BOL-002 - Download com ownership

**Given** cliente autenticado  
**When** tenta baixar boleto que nao pertence a ele  
**Then** sistema deve negar acesso com `403 Forbidden`

## CA-LGPD-001 - Rastreabilidade

**Given** operacao em dado pessoal  
**When** a operacao e concluida  
**Then** sistema deve registrar evento auditavel com usuario, acao e timestamp
