## 2026-04-19 - 12:02
- Continuidade da adequacao de orquestracao de containers com foco em observabilidade e isolamento de banco por modulo;
- Criacao da stack local `docker-compose.modulo.yml` com `modulo-app`, `pgsql-modulo`, `loki` e `grafana`;
- Provisionamento de configuracoes de Loki e datasource do Grafana para habilitar consulta de logs centralizados;
- Atualizacao do `README.md` com passo a passo de subida, validacao e encerramento da stack;
- Atualizacao do `TODOs.md` com checkpoint concluido da stack base e pendencias de evolucao para multiplos modulos.
- Correcao de compatibilidade do `postgres:18.3` com ajuste de volume para `/var/lib/postgresql`;
- Validacao de stack em execucao (`docker compose ps`) e health da API em `http://localhost:5099/api/health`.

--- Secao Opcional Detalhamento ---
A stack foi desenhada para manter separacao entre rede de aplicacao e rede de observabilidade.
O modulo da aplicacao passa a publicar logs para Loki por variavel de ambiente, sem alterar o codigo da API.
Foram definidos volumes nomeados para persistencia de dados de PostgreSQL, Loki e Grafana.

--- TimeLine das mudancas no codigo/documentos ---
- Criado `docker-compose.modulo.yml`;
- Criado `ops/loki/local-config.yaml`;
- Criado `ops/grafana/provisioning/datasources/datasource.yml`;
- Atualizado `README.md` com nova secao de orquestracao local;
- Atualizado `TODOs.md` com status do checkpoint atual.
- Corrigido `docker-compose.modulo.yml` para volume do PostgreSQL 18.3 em `/var/lib/postgresql`;
- Validada subida completa da stack e endpoint de health da aplicacao.

## 2026-04-19 - 12:30
- Organizacao da estrategia de versionamento antes da proxima fase de implementacao;
- Definicao do fluxo trunk-based com `master` como trunk;
- Configuracao inicial do Commitizen no repositorio via `.cz.toml`;
- Documentacao do fluxo de commit e versionamento em `docs/versionamento-git.md`;
- Atualizacao de `README.md` e `TODOs.md` para refletir o novo padrao de trabalho.

--- TimeLine das mudancas no codigo/documentos ---
- Criado `.cz.toml`;
- Criado `docs/versionamento-git.md`;
- Atualizado `README.md` com secao de fluxo Git;
- Atualizado `TODOs.md` com checkpoint de versionamento.
