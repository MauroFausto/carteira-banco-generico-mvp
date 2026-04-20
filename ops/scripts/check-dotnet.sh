#!/usr/bin/env bash
set -euo pipefail

export PATH="$HOME/.dotnet:$PATH"

dotnet restore "CarteiraBankMvp.slnx"
dotnet build "CarteiraBankMvp.slnx" --no-restore
dotnet test "CarteiraBankMvp.slnx" --no-build

echo "Build/Test local: OK."
