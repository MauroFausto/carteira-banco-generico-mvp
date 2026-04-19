# OWASP Checklist (.NET 10)

## Critico

- A01 Broken Access Control: negar por padrao, policy por recurso, validacao de ownership.
- A02 Cryptographic Failures: TLS 1.2+, criptografia em repouso, segredos em cofre.
- A06 Vulnerable Components: bloquear pipeline com vulnerabilidade critica sem mitigacao.
- A07 Authentication Failures: MFA para supervisor, lockout e protecao contra brute force.
- A09 Logging/Monitoring Failures: trilha de eventos de seguranca e correlacao de logs.

## Alto

- A03 Injection: consultas parametrizadas e validacao de entrada.
- A05 Security Misconfiguration: hardening de headers, CORS restritivo, erro generico em producao.
- A08 Software Integrity: assinatura de artefatos e protecao de pipeline.
- A10 SSRF: allowlist de destinos e bloqueio de metadata endpoint.

## Pipeline de Seguranca

- SAST em PR.
- Dependency scanning (NuGet + imagem).
- Secret scanning.
- SBOM por build.
- Gate por severidade.

## KPI/KRI Sugeridos

- % vulnerabilidades criticas corrigidas no SLA.
- MTTR por severidade.
- % builds com scan completo.
- Numero de segredos expostos por sprint.
