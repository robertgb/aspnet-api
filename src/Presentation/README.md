# 4. Presentation / API (Ponto de Entrada)

Recebe requisições HTTP, valida tokens de autorização e encaminha chamadas para a camada de aplicação. Depende da Application e pode referenciar a Infrastructure para registrar dependências na inicialização.

## Responsabilidades

- **Controllers / Minimal APIs:** endpoints HTTP enxutos que recebem DTOs e acionam handlers ou serviços de aplicação.
- **Middlewares:** tratamento global de exceções, logging, CORS e autenticação/autorização.
- **Injeção de dependência:** registro dos serviços e implementações na inicialização (`Program.cs`).

## Endpoints

O CRUD de clientes está disponível em `/customers`:

| Método   | Rota              | Descrição           |
| -------- | ----------------- | ------------------- |
| `GET`    | `/customers`      | Lista clientes      |
| `GET`    | `/customers/{id}` | Busca um cliente    |
| `POST`   | `/customers`      | Cria um cliente     |
| `PUT`    | `/customers/{id}` | Atualiza um cliente |
| `DELETE` | `/customers/{id}` | Remove um cliente   |

Exemplo de criação:

```json
{
  "name": "Maria Silva",
  "email": "maria@example.com"
}
```

Os clientes são armazenados em memória enquanto a aplicação está em execução.
