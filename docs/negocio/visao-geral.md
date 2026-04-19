# Visao Geral do Produto

## Objetivo

Implementar a carteira Banco Pascholotto para viabilizar:
- calculo de divida com rastreabilidade;
- geracao de acordos de negociacao;
- emissao de boletos em PDF;
- visualizacao em frontend para dois perfis de usuario.

## Perfis de Usuario

- **Cliente (tomador de emprestimo):** consulta dados proprios, solicita credito, acompanha contratos, acordos e boletos.
- **Supervisor de Gestao de Operacoes de Credito:** avalia solicitacoes, aprova/nega credito, formaliza acordos e acompanha indicadores operacionais.

## Escopo Funcional do MVP

1. Fluxo de autenticacao/autorizacao com Microsoft Identity e RBAC.
2. Fluxo de solicitacao e avaliacao de credito.
3. Calculo de saldo devedor com base em parcelas em aberto.
4. Geracao de acordo com condicoes negociadas.
5. Geracao e servico seguro de boletos PDF (sem CDN).
6. Frontend para consulta e operacao basica.

## Fora de Escopo no MVP

- Importacao de contratos e parcelas (dados podem existir previamente).
- Microservicos e distribuicao em multiplos deployables.
- Otimizacoes avancadas de analytics.

## Indicadores de Sucesso

- Tempo medio de avaliacao de solicitacao de credito.
- Taxa de aprovacao/negacao por regra.
- Tempo medio de geracao de boleto.
- Taxa de falha em downloads de PDF.
- Cobertura de testes nos modulos criticos.
