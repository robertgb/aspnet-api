# 2. Application (Casos de Uso)

Orquestra o fluxo de dados e interações entre a camada de apresentação e a de domínio. Depende apenas da camada Domain.

## Responsabilidades

- **Casos de uso / handlers:** implementação das intenções do usuário, podendo usar CQRS e MediatR.
- **DTOs (Data Transfer Objects):** objetos de entrada e saída para evitar expor entidades de domínio.
- **Interfaces de serviços externos:** contratos para e-mail, pagamentos, mensageria e outros gateways.
- **Validações de entrada:** validação dos dados recebidos antes do processamento dos casos de uso.
