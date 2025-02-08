# Hackaton - Pedro Daguis

Este repositório contém um projeto desenvolvido para atender aos requisitos do Hackaton da turma 4NETT do Pós Graduação FIAP. Neste arquivo README, você encontrará um guia passo a passo para abrir o projeto em seu ambiente local.

## Sobre o Projeto

Meu Hackaton é uma aplicação back end simples que permite aos pacientes, conveniados a Health&Med, realizarem agendamentos de consultas disponibilizadas através de agendas criadas por médicos. Foi desenvolvido com as seguintes tecnologias:

- Linguagem: C# 13
- Framework: .NET Core 9
- Banco de Dados: MongoDB

A aplicação possui as funcionalidades básicas de CRUD (Create, Read, Update, Delete) para gerenciar todas as tarefas das entidades.

## 🛠️ Outras Ferramentas Utilizadas

- JWT (Para gerenciamento de tokens de autenticação no sistema)
- Fluent Validation (Para validação dos dados de entrada)
- Docker (Para conteinirização da aplicação)
- Kubernetes (Para escalabidade e gerenciamento de containers)

## Pré-requisitos

Antes de começar, certifique-se de que você tenha as seguintes ferramentas instaladas em sua máquina:

- Docker 🐳
- Docker Compose 🐙
- .NET Core 9
- WSL (Caso Windows)

## 🚀 Passo a passo interativo

Siga os passos abaixo para iniciar o projeto em seu ambiente local:

1️⃣ **Clone o repositório**

   Clique no botão "Clone" acima ou execute o seguinte comando no terminal:

   ```bash
   git clone https://github.com/seu-usuario/nome-do-repositorio.git
   ```

   Isso criará uma cópia local do repositório em seu ambiente.

2️⃣ **Navegue até o diretório do projeto**

   ```bash
   cd nome-do-repositorio
   ```

   Isso criará uma cópia local do repositório em seu ambiente.

   3️⃣ **Dê um build no projeto**

   ```bash
   dotnet build
   ```

   🐳 Isso iniciará os contêineres necessários para o projeto, incluindo o servidor Laravel e o cliente React.

4️⃣ **Inicialize os contêineres Docker**

   Navegue até o diretório raiz do projeto e execute o seguinte comando para iniciar os contêineres Docker:

   ```bash
   docker-compose up -d
   ```

   🐳 Isso iniciará os contêineres necessários para o projeto, incluindo o servidor Laravel e o cliente React.

   5️⃣ **Acesse a aplicação:**

   Abra o seu navegador e digite o seguinte endereço:

   ```
   http://localhost:5094
   ```
