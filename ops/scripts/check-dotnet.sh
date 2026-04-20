#!/usr/bin/env bash
set -euo pipefail

export PATH="$HOME/.dotnet:$PATH"

dotnet restore "src/backend/CarteiraBank.Api/CarteiraBank.Api.csproj"
dotnet build "src/backend/CarteiraBank.Api/CarteiraBank.Api.csproj" --no-restore

dotnet restore "src/backend/CarteiraBank.Api.Tests/CarteiraBank.Api.Tests.csproj"
dotnet test "src/backend/CarteiraBank.Api.Tests/CarteiraBank.Api.Tests.csproj" --no-restore

echo "Build/Test local: OK."
