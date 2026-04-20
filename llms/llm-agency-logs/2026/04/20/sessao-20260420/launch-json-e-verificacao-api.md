## 2026-04-20 - sessão
- Criado/atualizado `.vscode/launch.json` com perfis de debug (in-memory e PostgreSQL local) e variáveis alinhadas ao `launchSettings.json`.
- Verificação: `dotnet build` na API com sucesso; `dotnet run` com `USE_INMEMORY_DATABASE=true` e requisição HTTP retornou 200 em `/openapi/v1.json`.
- Porta 5099 liberada (parada do container `modulo-app` e processos locais da API) e criado script `ops/scripts/manage-api-port.sh` para gerenciar porta/instâncias.
- Criado fluxo de testes manual em `docs/manual-test-flow.md` com rotas, headers e exemplos curl.

--- TimeLine das mudanças no código/documentos ---
- `.vscode/launch.json` — perfis nomeados, `ASPNETCORE_URLS`, opção in-memory e opção PostgreSQL.
