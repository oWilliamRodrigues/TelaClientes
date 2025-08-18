# TelaClientes
Projeto desenvolvido em C# MVC durante a Capacitação da Geen.

A seguir, um resumo do que foi implementado no projeto, destacando funcionalidades e comportamento da aplicação:

## Objetivo
Desenvolver uma tela de cadastro e gerenciamento de clientes, permitindo a inclusão, edição, exclusão, alteração de situação em lote e exportação de dados.

## Funcionalidades

- **Listagem de Clientes**  
  - Exibe todas as informações relevantes do cliente.  
  - Mostra apenas o contato principal de cada cliente; caso não haja, exibe o último adicionado.  
  - Permite filtrar por nome, endereço e situação.

- **Inclusão de Clientes**  
  - Campos iniciais vazios e prontos para edição.  
  - Adição de contatos via modal, garantindo apenas uma forma principal de cada tipo.  
  - Ação de desistir retorna à tela anterior sem salvar alterações.

- **Edição de Clientes**  
  - Permite alterar os dados do cliente selecionado mantendo a estrutura de contatos.

- **Exclusão de Clientes**  
  - Remove o cliente e todos os dados relacionados de forma segura.

- **Alteração de Situação em Lote**  
  - Permite atualizar a situação de múltiplos clientes simultaneamente (Ativo, Suspenso ou Cancelado).

- **Exportação**  
  - Possibilidade de exportar os dados da listagem completa ou parcial.
