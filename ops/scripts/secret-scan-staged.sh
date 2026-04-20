#!/usr/bin/env bash
set -euo pipefail

if ! command -v rg >/dev/null 2>&1; then
  echo "rg (ripgrep) nao encontrado. Instale para habilitar o scan de segredos."
  exit 1
fi

if ! command -v git >/dev/null 2>&1; then
  echo "git nao encontrado."
  exit 1
fi

PATTERN='(AKIA[0-9A-Z]{16}|ASIA[0-9A-Z]{16}|ghp_[A-Za-z0-9]{36}|github_pat_[A-Za-z0-9_]{82}|xox[baprs]-[A-Za-z0-9-]{10,}|(api[_-]?key|token|secret|password|client_secret)\s*[:=]\s*["'"'"']?[A-Za-z0-9+/_\.-]{8,}["'"'"']?|Bearer\s+[A-Za-z0-9\-._~+/]+=*|[A-Za-z0-9_-]{20,}\.[A-Za-z0-9_-]{10,}\.[A-Za-z0-9_-]{10,})'

staged_content="$(git diff --cached --unified=0 --no-color | rg '^\+' || true)"

if [[ -z "${staged_content}" ]]; then
  echo "Secret scan: sem adicoes staged para analisar."
  exit 0
fi

matches="$(printf '%s\n' "${staged_content}" | rg -n "${PATTERN}" || true)"

if [[ -n "${matches}" ]]; then
  echo "ERRO: potencial segredo detectado em alteracoes staged."
  echo "Remova o segredo ou substitua por valor seguro antes de commitar."
  echo
  echo "${matches}"
  exit 1
fi

echo "Secret scan strict-block: OK."
