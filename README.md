# ASP.NET Core com Docker

Projeto ASP.NET Core 10 com dois fluxos Docker:

- desenvolvimento com `dotnet watch` e hot reload;
- produção com imagem multi-stage contendo apenas o runtime e a aplicação publicada.

## Estrutura

- `src/aspnet-api.csproj`: projeto .NET;
- `Dockerfile`: estágios `dev`, `publish` e `final`;
- `docker-compose.yml`: ambiente de desenvolvimento;
- `Makefile`: atalhos para os comandos mais usados.
- `.gitignore`: exclui artefatos de build, IDE e configurações locais.

O `Program.cs` permanece como ponto de entrada enxuto. Registros de casos de uso ficam em `Core/Application/DependencyInjection.cs`, registros de banco e repositórios em `Infrastructure/DependencyInjection.cs` e registros HTTP em `Presentation/DependencyInjection.cs`; o pipeline fica em `Presentation/ApplicationBuilderExtensions.cs`.

As classes e métodos do código-fonte mantido pelo projeto possuem documentação XML. O build gera o arquivo `aspnet-api.xml`, e o Swagger o utiliza para exibir descrições dos endpoints.

A aplicação usa a porta `8080` dentro do container. A porta externa é definida pelo Compose ou pelo comando `docker run`.

## VS Code

As configurações compartilhadas ficam em `.vscode/`:

- **Run and Debug - `.NET API (local)`:** compila e executa a aplicação localmente em `http://localhost:5264`;
- **Task - `docker: dev`:** inicia o Compose em primeiro plano com hot reload;
- **Task - `docker: dev background`:** inicia o Compose em segundo plano;
- **Task - `docker: down`:** para o ambiente Docker.

No container, o `launchSettings.json` é ignorado pelo `dotnet watch`; a aplicação usa `http://0.0.0.0:8080`.

## Desenvolvimento

Suba a API com hot reload:

```bash
docker compose up --build
```

Ou:

```bash
make dev
```

Este projeto não possui um alvo `make up`; use `make dev` para iniciar o Compose em modo de desenvolvimento.

A API ficará disponível em `http://localhost:5000`. O Swagger e o endpoint de saúde são:

```text
http://localhost:5000/swagger
http://localhost:5000/health
```

O Compose monta `./src` em `/src`. Alterações nos arquivos dentro de `src/` são detectadas pelo `dotnet watch`.
No container, o watcher ignora o `launchSettings.json` e usa `ASPNETCORE_URLS=http://0.0.0.0:8080`.
Os diretórios `bin/` e `obj/` usam volumes internos do container, evitando conflitos de permissão com o build local.

O PostgreSQL fica disponível no serviço `aspnet-api-postgres` e usa a porta `5432`. A aplicação usa esse hostname dentro da rede Docker; localmente, `appsettings.json` usa `localhost`.
Os dados do PostgreSQL ficam no volume Docker `aspnet-api-postgres-data` e sobrevivem a `docker compose down`. Remova esse volume apenas quando quiser apagar o banco.

Para ver os logs:

```bash
make logs
```

Para parar o ambiente:

```bash
make down
```

## Produção

Gere a imagem final, usando o estágio `final`:

```bash
docker build --target final -t aspnet-api-application:production .
```

Ou:

```bash
make prod-build
```

Execute a imagem:

```bash
docker run --rm -p 8080:8080 --name aspnet-api-application aspnet-api-application:production
```

Ou:

```bash
make prod-run
```

A API de produção ficará disponível em `http://localhost:8080`.

Em um servidor, você pode publicar outra porta externa sem alterar a aplicação:

```bash
docker run --rm -p 80:8080 --name aspnet-api-application aspnet-api-application:production
```

Nesse exemplo, o acesso externo será feito pela porta `80`, enquanto a aplicação continuará ouvindo na porta `8080` dentro do container.

## Endpoints

### Clientes

```text
GET    /customers
GET    /customers/{id}
POST   /customers
PUT    /customers/{id}
DELETE /customers/{id}
```

### Produtos

```text
GET    /products
GET    /products/{id}
POST   /products
PUT    /products/{id}
DELETE /products/{id}
```

Exemplo de produto:

```json
{
  "name": "Notebook",
  "description": "Notebook para trabalho",
  "price": 3499.9
}
```

Clientes e produtos são persistidos no PostgreSQL. As migrations ficam em `src/Migrations` e são aplicadas pelo comando `make migrate`.

## Entity Framework e migrations

O projeto já inclui `AppDbContext`, o provider PostgreSQL e a ferramenta `dotnet-ef` no estágio de desenvolvimento do Docker.

Para criar e aplicar uma migration:

```bash
make migrate MIGRATION_NAME=create_table_products
```

Esse comando:

1. aguarda o PostgreSQL ficar saudável;
2. cria os arquivos em `src/Migrations`;
3. aplica a migration no banco `MyDatabase`.

Para adicionar apenas um pacote NuGet:

```bash
make add-package PACKAGE_NAME=Nome.Do.Pacote
```

## Comandos de manutenção

Os comandos abaixo evitam depender do SDK instalado na máquina e mantêm os artefatos dentro do fluxo Docker:

```bash
make restore
make build
make clean
```

Para listar todos os atalhos:

```bash
make help
```

## Camadas da aplicação

- [Domain](src/Core/Domain/README.md): regras e contratos do negócio;
- [Application](src/Core/Application/README.md): casos de uso e orquestração;
- [Infrastructure](src/Infrastructure/README.md): persistência e integrações externas;
- [Presentation](src/Presentation/README.md): entrada HTTP, middlewares e composição da API.

Ao adicionar uma funcionalidade, mantenha a separação:

- entidade e contratos em `src/Core/Domain/<Feature>`;
- casos de uso e DTOs em `src/Core/Application/<Feature>`;
- persistência e integrações em `src/Infrastructure`;
- controllers em `src/Presentation/Controllers`.
