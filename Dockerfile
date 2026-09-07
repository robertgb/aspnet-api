# Stage 1: Runtime Base (usado para dev e produção)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

# Stage 2: SDK para Build e Desenvolvimento
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dev
WORKDIR /src
RUN dotnet tool install --tool-path /tools dotnet-ef --version 10.0.11
ENV PATH="/tools:${PATH}"
COPY ["src/aspnet-api.csproj", "./"]
RUN dotnet restore
COPY src/ .

# Comando Padrão para Dev (habilita o Hot Reload)
ENV DOTNET_WATCH_SUPPRESS_LAUNCH_SETTINGS_FILE=1
CMD ["dotnet", "watch", "run", "--no-launch-profile"]

# Stage 3: Publicação da Aplicação
FROM dev AS publish
RUN dotnet publish "aspnet-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Imagem Final de Produção (leve)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "aspnet-api.dll"]