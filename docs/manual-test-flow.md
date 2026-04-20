## Fluxo de testes manual (API)

Base URL (host): `http://localhost:5099`

Headers (modo dev):
- `X-User-Id`: `<GUID do usuario>`
- `X-User-Role`: `Cliente` ou `Supervisor`

### 1) Health + OpenAPI
1. `GET /api/health`
2. `GET /openapi/v1.json`
3. `GET /scalar`

### 2) Cliente logado (role: Cliente ou Supervisor)
`GET /api/clientes/me`

Exemplo:
```bash
curl -s http://localhost:5099/api/clientes/me \
  -H "X-User-Id: 11111111-1111-1111-1111-111111111111" \
  -H "X-User-Role: Cliente"
```

### 3) Criar solicitacao de credito (role: Cliente)
`POST /api/solicitacoes-credito`

Body:
```json
{
  "valorSolicitado": 1500.0,
  "quantidadeParcelasSolicitada": 6,
  "finalidade": "Consolidar dividas"
}
```

Exemplo:
```bash
curl -s -X POST http://localhost:5099/api/solicitacoes-credito \
  -H "Content-Type: application/json" \
  -H "X-User-Id: 11111111-1111-1111-1111-111111111111" \
  -H "X-User-Role: Cliente" \
  -d '{"valorSolicitado":1500.0,"quantidadeParcelasSolicitada":6,"finalidade":"Consolidar dividas"}'
```
Guarde o `id` retornado.

### 4) Listar solicitacoes (role: Cliente ou Supervisor)
`GET /api/solicitacoes-credito`

Exemplo:
```bash
curl -s http://localhost:5099/api/solicitacoes-credito \
  -H "X-User-Id: 11111111-1111-1111-1111-111111111111" \
  -H "X-User-Role: Cliente"
```

### 5) Decidir solicitacao (role: Supervisor)
`POST /api/solicitacoes-credito/{id}/decisao`

Body:
```json
{
  "aprovar": true,
  "motivo": "Politica interna"
}
```

Exemplo:
```bash
curl -s -X POST http://localhost:5099/api/solicitacoes-credito/<ID_SOLICITACAO>/decisao \
  -H "Content-Type: application/json" \
  -H "X-User-Id: 22222222-2222-2222-2222-222222222222" \
  -H "X-User-Role: Supervisor" \
  -d '{"aprovar":true,"motivo":"Politica interna"}'
```
Guarde o `id` do contrato gerado (se aplicavel) ou recupere via lista de contratos.

### 6) Listar contratos (role: Cliente ou Supervisor)
`GET /api/contratos`

Exemplo:
```bash
curl -s http://localhost:5099/api/contratos \
  -H "X-User-Id: 11111111-1111-1111-1111-111111111111" \
  -H "X-User-Role: Cliente"
```
Guarde um `id` de contrato.

### 7) Saldo devedor do contrato (role: Cliente ou Supervisor)
`GET /api/contratos/{id}/saldo-devedor`

Exemplo:
```bash
curl -s http://localhost:5099/api/contratos/<ID_CONTRATO>/saldo-devedor \
  -H "X-User-Id: 11111111-1111-1111-1111-111111111111" \
  -H "X-User-Role: Cliente"
```

### 8) Criar acordo (role: Supervisor)
`POST /api/contratos/{id}/acordos`

Body:
```json
{
  "valorDesconto": 200.0,
  "quantidadeParcelas": 3
}
```

Exemplo:
```bash
curl -s -X POST http://localhost:5099/api/contratos/<ID_CONTRATO>/acordos \
  -H "Content-Type: application/json" \
  -H "X-User-Id: 22222222-2222-2222-2222-222222222222" \
  -H "X-User-Role: Supervisor" \
  -d '{"valorDesconto":200.0,"quantidadeParcelas":3}'
```
Guarde o `id` do acordo.

### 9) Emitir boleto (role: Supervisor)
`POST /api/acordos/{id}/boleto`

Exemplo:
```bash
curl -s -X POST http://localhost:5099/api/acordos/<ID_ACORDO>/boleto \
  -H "X-User-Id: 22222222-2222-2222-2222-222222222222" \
  -H "X-User-Role: Supervisor"
```
Guarde o `id` do boleto.

### 10) Baixar boleto em PDF (role: Cliente ou Supervisor)
`GET /api/boletos/{id}/pdf`

Exemplo:
```bash
curl -s -o boleto.pdf http://localhost:5099/api/boletos/<ID_BOLETO>/pdf \
  -H "X-User-Id: 11111111-1111-1111-1111-111111111111" \
  -H "X-User-Role: Cliente"
```
