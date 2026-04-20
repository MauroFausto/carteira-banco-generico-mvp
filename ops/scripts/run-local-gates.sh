#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "$0")/../.." && pwd)"

"${repo_root}/ops/scripts/secret-scan-staged.sh"
"${repo_root}/ops/scripts/check-dotnet.sh"

echo "Local gates: OK."
