# Requisitos Nao Funcionais

## Seguranca

- Autenticacao via Microsoft Identity.
- Autorizacao RBAC com policy-based access.
- Controles OWASP Top 10 aplicados.
- Auditoria para operacoes criticas.

## Privacidade e Compliance

- Aderencia a LGPD (base legal, minimizacao, DSAR, retencao).
- Mascaramento de dados sensiveis em interfaces e logs.

## Qualidade

- TDD obrigatorio.
- Pipeline com testes automatizados.
- Code review obrigatorio para merge.

## Performance

- Resposta de APIs de consulta principal dentro de SLO definido pelo time.
- Indices SQL para consultas de fila, parcelas e boletos.

## Operabilidade

- Logs estruturados com correlation id.
- Monitoramento de erros, latencia e fila de processamento de PDF.
