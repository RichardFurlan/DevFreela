# DevFreela

DevFreela é uma aplicação de gerenciamento de projetos que utiliza Arquitetura Limpa, CQRS e Entity Framework. Gerencie projetos, habilidades e usuários de forma eficaz com uma API moderna e bem estruturada.

## 🚀 Funcionalidades

- **Projetos**: Criação, atualização, exclusão, iniciar e concluir projetos, além de adicionar comentários.
- **Habilidades**: Listar e adicionar habilidades.
- **Usuários**: Criar, atualizar perfil (foto e habilidades), e obter detalhes.

## 🛠️ Tecnologias

- .NET 8.0
- Entity Framework Core
- MediatR
- ASP.NET Core

## 📜 Endpoints da API

### Projetos

- **GET** `/api/projects` - Listar projetos
- **GET** `/api/projects/{id}` - Obter detalhes de um projeto
- **POST** `/api/projects` - Criar um novo projeto
- **PUT** `/api/projects/{id}` - Atualizar um projeto
- **DELETE** `/api/projects/{id}` - Excluir um projeto
- **PUT** `/api/projects/{id}/start` - Iniciar um projeto
- **PUT** `/api/projects/{id}/complete` - Concluir um projeto
- **POST** `/api/projects/{id}/comments` - Adicionar um comentário a um projeto

### Habilidades

- **GET** `/api/skills` - Listar habilidades
- **POST** `/api/skills` - Adicionar uma habilidade

### Usuários

- **GET** `/api/users/{id}` - Obter detalhes de um usuário
- **POST** `/api/users` - Criar um novo usuário
- **POST** `/api/users/{id}/profile-picture` - Atualizar foto de perfil
- **POST** `/api/users/skill` - Adicionar habilidades a um usuário

## 🚀 Começando

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/seu-repositorio/devfreela.git
