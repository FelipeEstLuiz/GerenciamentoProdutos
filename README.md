# Gerenciamento de Produtos

## Descrição
Aplicação criada para fins de treinamento, com o objetivo de gerenciar produtos utilizando tecnologias modernas e boas práticas de desenvolvimento.

## Tecnologias Utilizadas

### Backend
- **.NET 9**: Framework utilizado para a construção da API.
- **Swagger**: Documentação interativa da API ([`ConfigureSwaggerOptions`](src/Application.Api/Util/ConfigureSwaggerOptions.cs)).
- **Autenticação JWT**: Implementada para autenticação e autorização segura ([`TokenService`](src/Application.Core/Services/TokenService.cs)).
- **MediatR**: Utilizado para implementar o padrão CQRS e facilitar a comunicação entre componentes.
- **Asp.Versioning**: Utilizado para versionamento de rotas da API.
- **GlobalExceptionHandlerMiddleware**: Middleware para tratamento global de exceções.
- **ValidationBehaviour**: Pipeline de validação para comandos e queries no padrão CQRS.
- **Cache**: Implementado para otimizar o desempenho em operações de leitura.
- **Dapper**: Micro ORM utilizado para consultas otimizadas ao banco de dados.
- **Moq.Dapper**: Biblioteca para criação de mocks em testes que utilizam Dapper.
- **BCrypt**: Utilizado para hashing seguro de senhas.
- **Hangfire**: Utilizado para execução de serviços em background, como a atualização de produtos para "fora de estoque" que não foram vendidos há mais de 60 dias.

### Frontend
- **Angular 19**: Framework utilizado para a construção da interface do usuário.
- **Material Design**: Biblioteca de componentes visuais para Angular.

### Testes
- **xUnit**: Framework utilizado para testes unitários.
- **NSubstitute**: Biblioteca para criação de mocks e stubs em testes.
- **Moq.Dapper**: Mocking de consultas Dapper em testes.

### Outras Ferramentas
- **FluentValidation**: Biblioteca para validação de regras de negócio ([`CreateProdutoValidator`](src/Application.Core/Validators/Produto/CreateProdutoValidator.cs), [`UpdateProdutoValidator`](src/Application.Core/Validators/Produto/UpdateProdutoValidator.cs)).
- **DDD (Domain-Driven Design)**: Arquitetura baseada em domínios para organização do projeto.
- **Clean Architecture**: Estrutura do projeto seguindo os princípios de separação de responsabilidades.
- **SOLID**: Princípios de design orientado a objetos aplicados em toda a aplicação.
- **SQServer**: Utilizado para armazenamento de dados temporários no ambiente de desenvolvimento.

## Inicialização do Banco de Dados

A aplicação possui uma classe chamada `DatabaseInitializer` que é responsável por configurar o banco de dados. Essa classe realiza as seguintes ações:

1. **Criação do Banco de Dados**: Verifica se o banco de dados especificado na string de conexão já existe. Caso não exista, ele será criado automaticamente.
2. **Execução de Scripts de Criação de Tabelas**: Executa o script SQL localizado em `src/Application.Infraestructure.Data/Scripts/CreateTables.sql`, que cria as tabelas necessárias para o funcionamento do sistema.
3. **Inserção de Usuário Padrão**: Insere um usuário padrão no banco de dados para acesso inicial ao sistema. O usuário criado possui as seguintes credenciais:
   - **Nome**: `teste`
   - **Email**: `teste@teste.com`
   - **Senha**: `P#ssw0rd` (a senha é armazenada de forma segura utilizando hashing com BCrypt).

Essa funcionalidade garante que o ambiente esteja pronto para uso logo após a execução da aplicação.


## Como Executar

### Backend
1. Navegue até o diretório `src/Application.Api`.
2. Compile e execute o projeto utilizando o .NET CLI:
   ```bash
   dotnet run
   ```

### Frontend
1. Navegue até o diretório `src/Application.Client`.
2. Inicie o servidor de desenvolvimento:
   ```bash
   ng serve
   ```
3. Acesse a aplicação em `http://localhost:4200/`.

## Testes

### Testes Unitários
- Execute os testes unitários do backend:
  ```bash
  dotnet test
  ```

## Estrutura do Projeto

```plaintext
src/
├── Application.Api/               # API Backend
├── Application.Client/            # Aplicação Angular
├── Application.Core/              # Lógica de Negócio
├── Application.Domain/            # Entidades e Modelos
├── Application.Infraestructure.Data/ # Acesso a Dados
├── Application.Infraestructure.IOC/  # Injeção de Dependências
tests/
└── Tests/                         # Testes Automatizados
```

## Contribuição
Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou enviar pull requests.

## Licença
Este projeto é apenas para fins de treinamento e não possui uma licença específica.