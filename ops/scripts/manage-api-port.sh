#!/usr/bin/env bash
set -euo pipefail

PORT="${PORT:-5099}"
COMPOSE_FILE="${COMPOSE_FILE:-docker-compose.modulo.yml}"
SERVICE_NAME="${SERVICE_NAME:-modulo-app}"

get_pids() {
  ss -tlnp 2>/dev/null \
    | rg ":${PORT}\\s" \
    | sed -n 's/.*pid=\([0-9]\+\).*/\1/p' \
    | sort -u
}

show_status() {
  echo "Porta ${PORT}:"
  ss -tlnp 2>/dev/null | rg ":${PORT}\\s" || echo "  livre"
}

stop_container() {
  docker compose -f "${COMPOSE_FILE}" stop "${SERVICE_NAME}" >/dev/null 2>&1 || true
}

stop_host_api() {
  pkill -f "CarteiraBank\\.Api\\.dll" >/dev/null 2>&1 || true
  pkill -f "CarteiraBank\\.Services\\.Api\\.dll" >/dev/null 2>&1 || true
}

free_port() {
  stop_container
  stop_host_api

  local pids
  pids="$(get_pids || true)"
  if [[ -n "${pids}" ]]; then
    echo "Matando PIDs na porta ${PORT}: ${pids}"
    kill ${pids} >/dev/null 2>&1 || true
  fi
}

start_host_api() {
  export ConnectionStrings__DefaultConnection="${ConnectionStrings__DefaultConnection:-Host=localhost;Port=5432;Database=carteira_bank_dev;Username=postgres;Password=postgres}"
  dotnet run \
    --project src/CarteiraBank.Services.Api/CarteiraBank.Services.Api.csproj \
    --urls "http://localhost:${PORT}" \
    --no-launch-profile
}

start_container() {
  docker compose -f "${COMPOSE_FILE}" up -d "${SERVICE_NAME}"
}

usage() {
  cat <<'EOF'
Uso: ops/scripts/manage-api-port.sh <comando>

Comandos:
  status         Mostra quem usa a porta (default)
  free           Para container + mata processos locais da API
  start-host     Sobe a API no host com PostgreSQL
  stop-host      Para a API no host
  start-docker   Sobe a API via docker compose
  stop-docker    Para a API via docker compose

Variáveis:
  PORT=5099
  COMPOSE_FILE=docker-compose.modulo.yml
  SERVICE_NAME=modulo-app
  ConnectionStrings__DefaultConnection=...
EOF
}

command="${1:-status}"
case "${command}" in
  status) show_status ;;
  free) free_port; show_status ;;
  start-host) start_host_api ;;
  stop-host) stop_host_api ;;
  start-docker) start_container ;;
  stop-docker) stop_container ;;
  *) usage; exit 1 ;;
esac
