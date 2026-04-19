# Contrato REST (Base Inicial)

## Auth

- `POST /api/auth/login` - inicia fluxo de autenticacao.
- `GET /api/auth/me` - retorna perfil autenticado.

## Credito

- `POST /api/credito/solicitacoes` - cria solicitacao de credito (Cliente).
- `GET /api/credito/solicitacoes/{id}` - consulta solicitacao.
- `POST /api/credito/solicitacoes/{id}/aprovar` - aprova solicitacao (Supervisor).
- `POST /api/credito/solicitacoes/{id}/negar` - nega solicitacao (Supervisor).

## Divida

- `POST /api/divida/contratos/{contratoId}/calcular` - calcula saldo devedor.
- `GET /api/divida/contratos/{contratoId}/saldos` - historico de calculo.

## Acordo

- `POST /api/acordos` - cria acordo.
- `GET /api/acordos/{id}` - detalha acordo.

## Boleto

- `POST /api/boletos/{parcelaAcordoId}/emitir` - emite boleto.
- `GET /api/boletos/{id}/pdf` - download seguro do PDF.

## Regras de Seguranca

- Endpoints protegidos por policy.
- Cliente acessa apenas recursos proprios.
- Supervisor acessa recursos operacionais globais.
