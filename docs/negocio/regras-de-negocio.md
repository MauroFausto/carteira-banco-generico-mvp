# Regras de Negocio

## 1. Solicitacao de Credito

1. CPF/CNPJ do cliente deve ser valido.
2. Valor solicitado deve ser maior que zero e dentro da politica ativa da carteira.
3. Prazo (parcelas desejadas) deve respeitar minimo e maximo da politica.
4. Cliente nao pode ter mais de uma solicitacao em analise simultaneamente.

## 2. Avaliacao de Credito

1. Toda avaliacao deve registrar versao da politica de credito aplicada.
2. Solicitacao com score abaixo do limite configurado segue para negacao ou analise manual.
3. Solicitacao com restricao bloqueante deve ser negada.
4. Toda negacao exige motivo de negacao registrado.
5. Aprovacao manual exige supervisor responsavel e parecer.

## 3. Concessao de Contrato

1. Contrato so pode ser criado para solicitacao aprovada.
2. Contrato deve estar associado a cliente e carteira.
3. Parcelas do contrato devem ser geradas com vencimentos coerentes ao plano aprovado.

## 4. Calculo de Divida

1. Saldo devedor considera parcelas em aberto.
2. Calculo deve considerar principal, juros e multa parametrizados.
3. Todo calculo gera snapshot rastreavel com data/hora e usuario/processo.
4. Recalculo nao pode sobrescrever historico anterior (somente inserir novo snapshot).

## 5. Geracao de Acordo

1. Acordo deve vincular contrato e cliente.
2. Acordo deve registrar condicoes negociadas (desconto, parcelas, vencimentos).
3. Quantidade de parcelas do acordo deve respeitar limites definidos.
4. Acordo aprovado gera parcelas de acordo.

## 6. Emissao de Boleto

1. Boleto e vinculado a uma parcela (contrato ou acordo).
2. PDF deve conter dados corretos do devedor e da cobranca.
3. Documento deve ser armazenado fora do `wwwroot`.
4. Download exige autenticacao e autorizacao.

## 7. Auditoria

1. Operacoes criticas devem gerar trilha de auditoria.
2. Logs nao devem expor dados pessoais completos.
3. Acoes do supervisor devem ser rastreaveis.
