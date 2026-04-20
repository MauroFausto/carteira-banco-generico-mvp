FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CarteiraBankMvp.slnx ./
COPY src/ ./src/

RUN dotnet restore "src/CarteiraBank.Services.Api/CarteiraBank.Services.Api.csproj"
RUN dotnet publish "src/CarteiraBank.Services.Api/CarteiraBank.Services.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CarteiraBank.Services.Api.dll"]
