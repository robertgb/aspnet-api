# Imagem usada pelos comandos .NET locais ao container.
DOTNET_IMAGE := mcr.microsoft.com/dotnet/sdk:10.0
IMAGE := aspnet-api-application
PROJECT := aspnet-api.csproj
COMPOSE := docker compose
RUN_DOTNET := docker run --rm -u "$$(id -u):$$(id -g)" -e HOME=/tmp -e DOTNET_CLI_HOME=/tmp -e NUGET_PACKAGES=/tmp/.nuget -v "$(PWD)/src:/src" -w /src $(DOTNET_IMAGE)
FIX_PROJECT_PERMISSIONS := docker run --rm -e HOST_UID="$$(id -u)" -e HOST_GID="$$(id -g)" -v "$(PWD)/src:/src" -w /src $(DOTNET_IMAGE) sh -c 'chown "$$HOST_UID:$$HOST_GID" /src/$(PROJECT)'
MIGRATION_BUILD_DIR := /tmp/aspnet-api-build-$(shell id -u)
MIGRATION_VOLUMES := -v "$(MIGRATION_BUILD_DIR)/bin:/src/bin" -v "$(MIGRATION_BUILD_DIR)/obj:/src/obj"

.PHONY: help dev down logs prod-build prod-run restore add-package migrate build clean

help:
	@echo "Comandos disponíveis:"
	@echo "  make dev        Inicia o ambiente de desenvolvimento com hot reload"
	@echo "  make down       Para os containers do Compose"
	@echo "  make logs       Exibe os logs do ambiente de desenvolvimento"
	@echo "  make prod-build Gera a imagem final de produção"
	@echo "  make prod-run   Executa a imagem de produção em localhost:8080"
	@echo "  make restore    Restaura os pacotes NuGet"
	@echo "  make add-package PACKAGE_NAME=... Adiciona um pacote NuGet"
	@echo "  make migrate MIGRATION_NAME=... Cria e aplica uma migration EF Core"
	@echo "  make build      Compila e publica o projeto em um build Docker"
	@echo "  make clean      Remove bin/ e obj/"

dev:
	$(COMPOSE) up --build aspnet-api-application

down:
	$(COMPOSE) down

logs:
	$(COMPOSE) logs -f aspnet-api-application

prod-build:
	docker build --target final -t $(IMAGE):production .

prod-run:
	docker run --rm -p 8080:8080 --name $(IMAGE) $(IMAGE):production

restore:
	$(RUN_DOTNET) dotnet restore $(PROJECT) $(DOTNET_ARGS)

add-package:
	@test -n "$(PACKAGE_NAME)" || (echo "Erro: use PACKAGE_NAME=Nome.Do.Pacote" && exit 1)
	$(FIX_PROJECT_PERMISSIONS)
	$(RUN_DOTNET) dotnet add $(PROJECT) package $(PACKAGE_NAME)

migrate:
	@test -n "$(MIGRATION_NAME)" || (echo "Erro: use MIGRATION_NAME=NomeDaMigration" && exit 1)
	mkdir -p "$(MIGRATION_BUILD_DIR)/bin" "$(MIGRATION_BUILD_DIR)/obj"
	$(COMPOSE) run --build --rm --user "$$(id -u):$$(id -g)" $(MIGRATION_VOLUMES) aspnet-api-application sh -c 'dotnet restore && dotnet ef migrations add $(MIGRATION_NAME) --project $(PROJECT) --startup-project $(PROJECT) --output-dir Migrations'
	$(COMPOSE) run --build --rm --user "$$(id -u):$$(id -g)" $(MIGRATION_VOLUMES) aspnet-api-application sh -c 'dotnet restore && dotnet ef database update --project $(PROJECT) --startup-project $(PROJECT)'

build:
	docker build --target publish -t $(IMAGE):build .

clean:
	docker run --rm -v "$(PWD)/src:/src" -w /src $(DOTNET_IMAGE) sh -c 'rm -rf bin obj'