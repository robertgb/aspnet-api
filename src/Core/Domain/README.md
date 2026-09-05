# 1. Domain (O Coração da Aplicação)

Não possui dependência de outros projetos ou bibliotecas externas, incluindo o Entity Framework.

## Responsabilidades

- **Entidades e objetos de valor:** modelam conceitos do negócio e suas invariantes.
- **Agregados e raízes de agregação:** agrupam entidades relacionadas e garantem consistência.
- **Eventos de domínio:** notificam alterações importantes no estado da aplicação.
- **Interfaces de repositório:** definem contratos de acesso a dados sem tecnologia específica.
- **Exceções de domínio:** representam violações de regras de negócio.
