# Dicionario de Dados (Base Inicial)

## Padroes

- Banco: PostgreSQL.
- Chave primaria: `uuid`.
- Datas: `timestamptz`.
- Campos de auditoria: `created_at`, `created_by`, `updated_at`, `updated_by`.

## Tabelas Essenciais

## USUARIO
- `id` (uuid, PK)
- `email` (varchar, unique)
- `role` (varchar)
- `ativo` (bool)

## CLIENTE
- `id` (uuid, PK)
- `usuario_id` (uuid, FK -> USUARIO)
- `nome` (varchar)
- `cpf_cnpj` (varchar, unique)

## SUPERVISOR
- `id` (uuid, PK)
- `usuario_id` (uuid, FK -> USUARIO)
- `nome` (varchar)

## SOLICITACAO_CREDITO
- `id` (uuid, PK)
- `cliente_id` (uuid, FK -> CLIENTE)
- `carteira_id` (uuid, FK -> CARTEIRA)
- `valor_solicitado` (numeric)
- `parcelas_desejadas` (int)
- `status` (varchar)

## AVALIACAO_CREDITO
- `id` (uuid, PK)
- `solicitacao_id` (uuid, FK -> SOLICITACAO_CREDITO)
- `supervisor_id` (uuid, FK -> SUPERVISOR)
- `score_calculado` (numeric)
- `resultado` (varchar)
- `parecer` (text)

## CONTRATO
- `id` (uuid, PK)
- `cliente_id` (uuid, FK -> CLIENTE)
- `numero_contrato` (varchar, unique)
- `status` (varchar)

## PARCELA
- `id` (uuid, PK)
- `contrato_id` (uuid, FK -> CONTRATO)
- `numero_parcela` (int)
- `valor_original` (numeric)
- `data_vencimento` (date)
- `status` (varchar)

## ACORDO
- `id` (uuid, PK)
- `contrato_id` (uuid, FK -> CONTRATO)
- `cliente_id` (uuid, FK -> CLIENTE)
- `valor_total_acordo` (numeric)
- `quantidade_parcelas` (int)
- `status` (varchar)

## PARCELA_ACORDO
- `id` (uuid, PK)
- `acordo_id` (uuid, FK -> ACORDO)
- `numero_parcela` (int)
- `valor` (numeric)
- `data_vencimento` (date)

## BOLETO
- `id` (uuid, PK)
- `parcela_acordo_id` (uuid, FK -> PARCELA_ACORDO)
- `nosso_numero` (varchar, unique)
- `codigo_barras` (varchar)
- `linha_digitavel` (varchar)
- `caminho_arquivo` (varchar)

## CONSENTIMENTO_LGPD
- `id` (uuid, PK)
- `cliente_id` (uuid, FK -> CLIENTE)
- `finalidade` (varchar)
- `consentido` (bool)
- `versao_politica` (varchar)

## AUDIT_LOG
- `id` (uuid, PK)
- `usuario_id` (uuid, FK -> USUARIO)
- `entidade` (varchar)
- `entidade_id` (uuid)
- `acao` (varchar)
- `dados_anteriores` (jsonb)
- `dados_novos` (jsonb)
