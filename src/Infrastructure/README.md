# 3. Infrastructure (Detalhes Técnicos)

Implementa os contratos definidos nas camadas internas. Depende das camadas Application e Domain.

## Responsabilidades

- **Persistência de dados:** configurações do Entity Framework Core, migrations, `DbContext` e implementações concretas dos repositórios.
- **Serviços externos:** conectores para provedores de e-mail, APIs externas, cache e outros recursos de infraestrutura.
