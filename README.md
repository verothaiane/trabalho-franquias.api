# Sistema de Gestão de Franquias - API REST

## Objetivo do Projeto
Desenvolver um sistema back-end completo para gestão de franquias utilizando C#, com arquitetura organizada, banco de dados relacional, autenticação, autorização, validações, regras de negócio, consultas e documentação da API. A proposta é simular um ambiente corporativo real no qual uma franqueadora precisa acompanhar diferentes unidades sem depender de planilhas, mensagens isoladas ou sistemas desconectados. O sistema centraliza informações da franqueadora e de suas unidades franqueadas, permitindo administrar usuários, unidades, produtos ou serviços, estoque, vendas, taxas de franquia, royalties, fornecedores, chamados e indicadores gerenciais.

## Tecnologias Utilizadas
O projeto demonstra a aplicação prática de diversas tecnologias e conceitos de desenvolvimento back-end:
* **Linguagem:** C#.
* **Framework:** ASP.NET Core Web API.
* **ORM:** Entity Framework Core para acesso e persistência dos dados.
* **Banco de Dados:** SQLite (Banco de dados relacional).
* **Segurança:** Autenticação e autorização via JWT (JSON Web Token) com perfis de acesso.
* **Padrões de Projeto:** Injeção de dependência, DTOs para entrada e saída de dados, e programação orientada a objetos (classes, métodos, encapsulamento).
* **Assincronismo:** Operações assíncronas com `async/await` nas rotinas de acesso a dados.
* **Documentação:** Swagger/OpenAPI.
* **Versionamento:** Git e GitHub.

## Requisitos de Execução
Para executar o projeto você precisará ter instalado em sua máquina:
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Git](https://git-scm.com/)
* Ferramenta de testes de API (Swagger já integrado, ou Postman / Insomnia / Bruno).

## Comandos para iniciar a API
**1. Clonar o repositório:**
`bash
git clone https://github.com/verothaiane/trabalho-franquias.api.git
cd trabalho-franquias.api
`
**2. Criar e aplicar o banco de dados:**
 É necessário recriar o banco de dados e aplicar as migrations em um ambiente limpo.
`bash
dotnet ef database update
`
**3. Iniciar a API:**
`bash
dotnet run
`
**4. Acessar a Documentação e Testar:**
Após iniciar a API, abra o navegador e acesse a interface do Swagger gerada automaticamente para testar os endpoints:
* **URL Padrão:** `http://localhost:5058/swagger` *(verifique a porta exata exibida no terminal após o comando dotnet run).*

### Credenciais de Acesso (Exemplo)
Para acessar os endpoints protegidos e gerar o token de acesso no `POST /api/Auth/login`, utilize as seguintes credenciais:
* **E-mail:** `admin@franquias.com`
* **Senha:** `123456`
