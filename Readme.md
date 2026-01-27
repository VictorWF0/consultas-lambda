# Lambda de Consultas DynamoDB - testeConsultas

Esta lambda consulta uma tabela DynamoDB cujo nome está armazenado no AWS Secrets Manager.

## Arquitetura

```
Models/
├── UserInput.cs               # Modelo de entrada com enum para tipos de consulta
└── Ticket.cs                  # Modelo que representa um ticket do DynamoDB

Services/
├── Dynamo/
│   └── DynamoService.cs       # Serviço para consultas no DynamoDB
└── Secret/
    └── SecretService.cs       # Serviço para buscar secrets

Function.cs                    # Handler principal da Lambda
```

## Funcionalidades

### Tipos de Consulta Suportados

- **MaisRecente**: Retorna o registro mais recente baseado no timestamp
- **MaisAntigo**: Retorna o registro mais antigo baseado no timestamp  
- **Todos**: Retorna todos os registros da tabela

### Configuração

- **Secret Name**: `secret_test_joao`
- **Secret Key**: `table_name`
- **Table Name**: `Joao_RegisterIncidents`

## Exemplo de Uso

### Input da Lambda
```json
{
  "TipoConsulta": "MaisRecente"
}
```

### Output da Lambda
```json
{
  "tipoConsulta": "MaisRecente",
  "tabela": "Joao_RegisterIncidents",
  "dados": {
    "TicketNumber": "INC24185396",
    "Timestamp": "2025-12-30 09:15:32",
    "AssignmentGroup": "IT_Support",
    "Description": "Teste_Criação2",
    "Name": "Victor",
    "PhoneNumber": "+5567925090",
    "ShortDescription": "Criação_Ticket2"
  }
}
```

## Estrutura da Tabela DynamoDB

A tabela `Joao_RegisterIncidents` contém os seguintes campos:
- `ticket_number` (String): Número do ticket
- `timestamp` (String): Data e hora do registro
- `assignment_group` (String): Grupo responsável
- `description` (String): Descrição detalhada
- `name` (String): Nome do solicitante
- `phone_number` (String): Telefone do solicitante
- `short_description` (String): Descrição resumida

## Logs

A lambda gera logs detalhados incluindo:
- Tipo de consulta recebida
- Nome da tabela recuperada do secret
- Progresso da consulta (buscando registro mais recente/antigo/todos)
- Confirmação de sucesso
- Erros e stack traces em caso de falha

## Namespace

O projeto utiliza o namespace `testeConsultas` em todos os arquivos. - AWS Lambda DynamoDB Query Service

Este projeto é uma função AWS Lambda desenvolvida em .NET 10 para consultar dados armazenados no DynamoDB através de diferentes tipos de índices.

## Funcionalidades

O serviço permite realizar consultas no DynamoDB com os seguintes tipos:
- **MostRecent**: Retorna os dados mais recentes
- **Oldest**: Retorna os dados mais antigos  
- **All**: Retorna todos os dados disponíveis

## Estrutura do Projeto

```
Consultas/
├── Function.cs                    # Handler principal da Lambda
├── Models/
│   └── QueryInputModel.cs        # Modelo de entrada com enum QueryType
├── Services/
│   ├── Dynamo/
│   │   └── DynamoService.cs      # Serviço para interação com DynamoDB (em desenvolvimento)
│   └── Secret/
│       └── SecretService.cs      # Serviço para gerenciamento de secrets (em desenvolvimento)
├── Consultas.csproj              # Configurações do projeto .NET
└── aws-lambda-tools-defaults.json # Configurações padrão para deploy AWS
```

## Modelo de Entrada

A função aceita um JSON com o seguinte formato:

```json
{
  "Query": "MostRecent" | "Oldest" | "All"
}
```

## Status do Desenvolvimento

🚧 **Projeto em desenvolvimento**

- ✅ Estrutura básica da Lambda configurada
- ✅ Modelo de entrada definido com enum QueryType
- ✅ Handler principal implementado com logging básico
- 🔄 DynamoService em desenvolvimento
- 🔄 SecretService em desenvolvimento
- ⏳ Implementação das consultas DynamoDB pendente 

## Tecnologias Utilizadas

- **.NET 10**: Framework principal
- **AWS Lambda**: Plataforma de execução serverless
- **Amazon DynamoDB**: Banco de dados NoSQL (integração pendente)
- **AWS Secrets Manager**: Gerenciamento de credenciais (integração pendente)
- **System.Text.Json**: Serialização JSON nativa

## Configuração e Deploy

### Deploy via Visual Studio

Para fazer deploy da função para AWS Lambda, clique com o botão direito no projeto no Solution Explorer e selecione *Publish to AWS Lambda*.

Para visualizar sua função deployada, abra a janela Function View clicando duas vezes no nome da função mostrado abaixo do nó AWS Lambda na árvore do AWS Explorer.

Para testar sua função deployada, use a aba Test Invoke na janela Function View aberta.

Para configurar fontes de eventos para sua função deployada, use a aba Event Sources na janela Function View.

Para atualizar a configuração de runtime da sua função deployada, use a aba Configuration na janela Function View.

Para visualizar logs de execução das invocações da sua função, use a aba Logs na janela Function View.

### Deploy via Command Line

Uma vez que você tenha editado seu código, pode fazer deploy da aplicação usando o [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) via command line.

Instalar Amazon.Lambda.Tools Global Tools se ainda não estiver instalado:
```bash
dotnet tool install -g Amazon.Lambda.Tools
```

Se já estiver instalado, verificar se há nova versão disponível:
```bash
dotnet tool update -g Amazon.Lambda.Tools
```

Executar testes unitários:
```bash
cd "Consultas/test/Consultas.Tests"
dotnet test
```

Deploy da função para AWS Lambda:
```bash
cd "Consultas/src/Consultas"
dotnet lambda deploy-function
```

## Exemplo de Uso

### Requisição para dados mais recentes:
```json
{
  "Query": "MostRecent"
}
```

### Requisição para dados mais antigos:
```json
{
  "Query": "Oldest"
}
```

### Requisição para todos os dados:
```json
{
  "Query": "All"
}
```

## Próximos Passos

1. Implementar DynamoService com conexão ao DynamoDB
2. Implementar SecretService para gerenciamento seguro de credenciais
3. Adicionar lógica de consulta baseada no QueryType
4. Implementar tratamento de erros e validações
5. Adicionar testes unitários
6. Configurar CI/CD pipeline
