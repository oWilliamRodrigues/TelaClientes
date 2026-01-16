# 🧩 TelaClientes Geen

Aplicação desenvolvida em **C# com ASP.NET MVC** durante a **capacitação técnica da empresa Geen**, utilizando o **framework proprietário Europa** e **PostgreSQL** como banco de dados.  

O projeto tem como objetivo o desenvolvimento de uma **tela completa de cadastro e gerenciamento de clientes**, explorando conceitos de arquitetura em camadas, persistência de dados e boas práticas no uso do padrão MVC.

---

## 🎯 Objetivo

Criar uma aplicação web funcional que permita **gerenciar clientes e seus contatos**, oferecendo operações completas de **inclusão, edição, exclusão**, além de **alteração de situação em lote** e **exportação de dados**.

---

## ⚙️ Tecnologias Utilizadas

- **C# / ASP.NET MVC**  
- **Framework Europa** (proprietário da Geen)  
- **PostgreSQL**  
- **DBeaver** (modelagem e consultas SQL)  
- **Entity Framework (NHibernate)**  
- **HTML, CSS e JavaScript**

---

## 🧠 Arquitetura

O projeto segue uma **estrutura em camadas**, contemplando:  
- **Controller:** gerenciamento das requisições e ações da interface.  
- **Service:** regras de negócio e validações.  
- **Repository:** acesso e manipulação dos dados.  
- **View:** exibição e interação com o usuário, utilizando os componentes do framework Europa.

---

## 🚀 Funcionalidades

- **Listagem de Clientes**  
  - Exibe todas as informações relevantes de cada cliente.  
  - Mostra apenas o contato principal; caso não exista, exibe o último contato cadastrado.  
  - Permite **filtrar** clientes por nome, endereço e situação.

- **Inclusão de Clientes**  
  - Campos iniciais vazios e prontos para preenchimento.  
  - Inclusão de contatos via **modal**, com validação para apenas um contato principal de cada tipo.  
  - Opção de **cancelar** o cadastro e retornar à tela anterior sem salvar alterações.

- **Edição de Clientes**  
  - Permite alterar dados do cliente mantendo a estrutura e relacionamentos de contatos.

- **Exclusão de Clientes**  
  - Remove o cliente e todos os registros relacionados de forma **segura e integrada**.

- **Alteração de Situação em Lote**  
  - Atualiza a situação de múltiplos clientes simultaneamente (**Ativo**, **Suspenso** ou **Cancelado**).

- **Exportação de Dados**  
  - Exporta a listagem de clientes de forma **completa ou filtrada**.

---

## 🗃️ Estrutura do Banco de Dados

O banco foi modelado em **PostgreSQL** e contempla as seguintes entidades principais:

- **Clientes**
- **Endereços**
- **Contatos**

Cada cliente pode possuir múltiplos contatos, com vínculo relacional e controle de contato principal.

---

## 📦 Como Executar o Projeto

1. Clone este repositório:
   ```bash
   git clone https://github.com/oWilliamRodrigues/TelaClientes.git
2. Abra o projeto no Visual Studio.

3. Configure a string de conexão no arquivo appsettings.json para apontar para seu banco PostgreSQL local.

4. Crie as tabelas necessárias executando os scripts SQL incluídos (caso existam).

5. Execute o projeto:
    dotnet run
   
6. Acesse a aplicação em http://localhost:5000
