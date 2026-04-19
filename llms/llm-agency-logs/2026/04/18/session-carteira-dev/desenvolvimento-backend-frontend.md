## 2026-04-18 - 12:58
- Inicio do desenvolvimento da aplicacao backend e frontend do MVP;
- Estruturacao inicial do projeto ASP.NET Core para API em `src/backend/CarteiraBank.Api`;
- Modelagem inicial de entidades de dominio para credito, contrato, acordo e boleto;
- Implementacao de persistencia com EF Core e PostgreSQL;
- Implementacao de endpoints principais de credito, contratos, saldo devedor, acordos e boleto;
- Implementacao de frontend inicial com Bulma em `src/frontend`;
- Integracao do frontend estatico servido pela API;
- Registro de observacao tecnica sobre indisponibilidade do binario `dotnet` no ambiente do agente.
- Correcao do PATH local para uso do binario em `~/.dotnet/dotnet`;
- Correcao de incompatibilidade Swagger/Swashbuckle para OpenAPI nativo;
- Ajuste de fallback para banco InMemory no desenvolvimento local;
- Validacao de execucao da API e chamadas principais de credito/contrato.
- Provisionamento de PostgreSQL 18.3 via Docker (`carteira-pg-18`);
- Refatoracao de entidades e funcoes de dominio para PT_BR;
- Inclusao de Serilog com sink de arquivo texto e sink Grafana Loki;
- Inclusao de middleware global de excecoes com respostas padronizadas para 400/500;
- Inclusao de logs detalhados para diagnostico em middleware e logs amigaveis para usuario no frontend.

--- Secao Opcional Detalhamento ---
O fluxo de desenvolvimento priorizou entrega funcional MVP com camada API e interface basica.
A autenticacao foi implementada em modo de desenvolvimento por cabecalho para permitir validacao de RBAC local enquanto a integracao final com Microsoft Identity e consolidada.
Foram executadas validacoes reais de endpoint para garantir contrato de resposta em falhas de validacao.

--- TimeLine das mudancas no codigo/documentos ---
- Criado `CarteiraBank.Api.csproj` e `appsettings.json`;
- Criado `Program.cs` com rotas e policies;
- Criados `Data/AppDbContext.cs` e `Data/SeedData.cs`;
- Criado `Domain/Entities.cs`;
- Criados arquivos frontend `index.html`, `css/styles.css` e `js/app.js`;
- Criado `README.md` tecnico de execucao.
- API validada em `http://localhost:5099/api/health`.
- Criados `Features/Erros/ExcecoesAplicacao.cs` e `Features/Erros/MiddlewareExcecaoGlobal.cs`;
- Atualizados `Domain/Entities.cs`, `Data/AppDbContext.cs`, `Data/SeedData.cs` e `Program.cs` para PT_BR;
- Atualizado frontend em `src/frontend/js/app.js` com mensagens amigaveis de feedback.

## 2026-04-18 - 13:58
- Criado `README.md` raiz com modos de debug e deploy Docker;
- Criado `src/backend/CarteiraBank.Api/Dockerfile` para containerizacao da API;
- Criado `TODOs.md` com backlog de orquestracao por modulo + Grafana/Loki + PostgreSQL por modulo;
- Registrada orientacao de continuidade para reinicio de sessao com foco em stack containerizada.
