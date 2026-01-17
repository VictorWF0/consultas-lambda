# Consultas - AWS Lambda DynamoDB Query Service

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
