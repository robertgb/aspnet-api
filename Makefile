# Imagem usada pelos comandos .NET locais ao container.
DOTNET_IMAGE := mcr.microsoft.com/dotnet/sdk:8.0
IMAGE := aspnet-webapi
PROJECT := aspnet-api.csproj
COMPOSE := docker compose
RUN_DOTNET := docker run --rm -u "$$(id -u):$$(id -g)" -e HOME=/tmp -e DOTNET_CLI_HOME=/tmp -e NUGET_PACKAGES=/tmp/.nuget -v "$(PWD)/src:/src" -w /src $(DOTNET_IMAGE)

.PHONY: help dev down logs prod-build prod-run restore build clean

help:
	@echo "Comandos disponíveis:"
	@echo "  make dev        Inicia o ambiente de desenvolvimento com hot reload"
	@echo "  make down       Para os containers do Compose"
	@echo "  make logs       Exibe os logs do ambiente de desenvolvimento"
	@echo "  make prod-build Gera a imagem final de produção"
	@echo "  make prod-run   Executa a imagem de produção em localhost:8080"
	@echo "  make restore    Restaura os pacotes NuGet"
	@echo "  make build      Compila e publica o projeto em um build Docker"
	@echo "  make clean      Remove bin/ e obj/"

dev:
	$(COMPOSE) up --build webapi

down:
	$(COMPOSE) down

logs:
	$(COMPOSE) logs -f webapi

prod-build:
	docker build --target final -t $(IMAGE):production .

prod-run:
	docker run --rm -p 8080:8080 --name $(IMAGE) $(IMAGE):production

restore:
	$(RUN_DOTNET) dotnet restore $(PROJECT) $(DOTNET_ARGS)

build:
	docker build --target publish -t $(IMAGE):build .

clean:
	docker run --rm -v "$(PWD)/src:/src" -w /src $(DOTNET_IMAGE) sh -c 'rm -rf bin obj'