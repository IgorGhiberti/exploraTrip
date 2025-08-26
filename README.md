# Explora Trip
O Explora Trip é um aplicativo Web que visa facilitar a vida dos exploradores e aventureiros, com uma solução integrada para organizar as viagens centralizando informações essenciais como roteiro, orçamento e locais para visitar! Além de poder incluir participantes nas suas viagens para tornar essa organização ainda mais fácil e compartilhada com amigos e família!

### Funcionalidades
#### Projeto em Desenvolvimento, nem todas as funcionalidades estão concluídas ainda!

 #### Usuários
 - Gerenciamento de usuários (CRUD)
 - Cadastro com hash de senha
 - Login de usuário através de criptografia
 - Soft delete (apenas desativar os usuários, não deletá-los do banco)

#### Viagens
- Criar uma nova viagem
- Visualizar lista de viagens do usuário
- Editar e excluir viagem
- Associar orçamento à viagem
- Associar outros usuários à viagem (com papéis: editor ou visualizador)

#### Locais das viagens
- Adicionar locais à viagem (ex: “Praia de Boa Viagem”, “Marco Zero”)
- CRUD completo de locais da viagem
- Adicionar descrição, valor estimado, data/hora e tipo (passeio, refeição, etc.)
- Tags (restaurante, passeio, praia etc)
- Cada local pode ter (opcional) um orçamento, que descontará do orçamento da viagem

### Arquitetura

<img width="1651" height="1351" alt="Arquitetura ExploraTrip" src="https://github.com/user-attachments/assets/7bd5053f-8aad-493b-a0a9-d60ba0e00196" />




Esta arquitetura está seguindo o padrão Clean Archtecture, onde as responsabilidades são definidas da seguinte forma:

1. Domain (Core):
O domínio é onde reside todo o core da aplicação, é o "por quê" das coisas. Nele contém as entidades, o padrão de resultados da aplicação e também os VOs, que são responsáveis por fazer a validação e adicionar comportamentos a cada domínio, seguindo o padrão de domínios ricos.

 2. Application:
 A aplicação é onde reside todos os Use Cases, ou regras de negócios. Esta camada é a principal responsável por se comunicar com a Api e a camada de infraestrutura, através dos DTOs ditar o que pode ou não ser exposto para o client no frontend. 

3. Infrastructure:
A camada de infraestrutura é responsável por todas as configurações do Entity Framework Core e também por ser comunicar diretamente com o banco de dados, que é um contêiner do Postgres rodando dentro do Docker.

4. WebApi:
A última camada é a da API. Feita através do framework ASP.NET ela é responsável por gerir todos os endpoints que irão se comunicar com o client, possuindo um padrão de respostas, o result pattern, tratando os erros não esperados (5xx do servidor) com mensagens mais genéricas, evitando expor informações sensíveis da aplicação. E as respostas 2xx e 4xx são tratadas nas ResultExtensions, as extensões do ResultData do domínio. 

### Tecnologias
- C# com .NET Core
- ASP.NET
- Entity Framework Core
- Docker
- Postgres
- xUnit
