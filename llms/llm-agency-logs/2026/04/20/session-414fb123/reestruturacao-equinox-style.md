## 2026-04-20 - 11:20
- Criada solution `CarteiraBankMvp.slnx` com 10 projetos na estrutura Equinox-style;
- Reimplementados building blocks DDD em `CarteiraBank.Domain.Core`;
- Migrados dominio, aplicacao, infraestrutura de dados, bus, identity e IoC para arquitetura em camadas;
- Migrados endpoints para controllers na API e configurados OpenAPI nativo com Scalar;
- Criados testes de arquitetura e testes unitarios de seguranca/validacao;
- Removidos arquivos legados da API antiga e atualizados scripts/CI/docker/README para a nova solution.

--- Seção Opcional Detalhamento ---
Refatoracao completa para remover dependencias NetDevPack e Swashbuckle, mantendo .NET 10 com pacote OpenAPI nativo e organizacao por camadas.

--- TimeLine das mudanças no código/documentos ---
- Etapa 1: scaffolding da solution e referencias entre projetos;
- Etapas 2-8: implementacao de dominio, dados, crosscutting, application, API e testes;
- Etapa 9: limpeza de legado e atualizacao de documentacao/pipeline/scripts.
