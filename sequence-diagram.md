# Diagrama Sequencial - Lambda de Consultas DynamoDB

```mermaid
sequenceDiagram
    participant User as Cliente/Usuário
    participant Lambda as Function.cs
    participant Secret as SecretService
    participant SM as AWS Secrets Manager
    participant Dynamo as DynamoService
    participant DB as AWS DynamoDB
    participant Logger as CloudWatch Logs

    User->>Lambda: Invoke Lambda com QueryInputModel
    Note over User,Lambda: Input: {"Query": "Mais recente"}
    
    Lambda->>Logger: Log "Lambda acionada com sucesso"
    Lambda->>Logger: Log tipo de consulta recebida
    
    Lambda->>Secret: GetTableNameAsync()
    Note over Lambda,Secret: Buscar nome da tabela
    
    Secret->>SM: GetSecretValueAsync("secret_test_joao")
    SM-->>Secret: Retorna JSON com table_name
    Secret->>Secret: Parse JSON e extrai "table_name"
    Secret-->>Lambda: Retorna "Joao_RegisterIncidents"
    
    Lambda->>Logger: Log nome da tabela encontrado
    
    Lambda->>Dynamo: QueryTableAsync(tableName, QueryType)
    Note over Lambda,Dynamo: Consultar com regra de negócio
    
    Dynamo->>DB: ScanAsync(tableName)
    DB-->>Dynamo: Retorna todos os registros
    
    alt QueryType == MostRecent
        Dynamo->>Dynamo: GetMostRecentRecord()
        Note over Dynamo: Ordena por timestamp DESC, pega primeiro
    else QueryType == Oldest
        Dynamo->>Dynamo: GetOldestRecord()
        Note over Dynamo: Ordena por timestamp ASC, pega primeiro
    else QueryType == All
        Note over Dynamo: Retorna todos os registros
    end
    
    Dynamo-->>Lambda: Retorna registros filtrados
    
    Lambda->>Lambda: Converte AttributeValue para Object
    Lambda->>Lambda: Monta resposta JSON estruturada
    
    Lambda->>Logger: Log "Consulta finalizada" + total registros
    
    Lambda-->>User: Retorna JSON com resultado
    Note over Lambda,User: {"TipoConsulta": "Mais recente", "TotalRegistros": 1, "Registros": [...]}

    Note over User,DB: Fluxo de Erro (Opcional)
    alt Erro em qualquer etapa
        Lambda->>Logger: Log erro + stack trace
        Lambda->>Lambda: Monta resposta de erro
        Lambda-->>User: Retorna JSON com erro
        Note over Lambda,User: {"Erro": true, "Mensagem": "...", "TipoConsulta": "..."}
    end
```

## Descrição do Fluxo

1. **Entrada**: Cliente invoca a lambda com o modelo de entrada contendo o tipo de consulta
2. **Logging**: Lambda registra o início da execução e o tipo de consulta
3. **Secret Manager**: SecretService busca o nome da tabela no AWS Secrets Manager
4. **DynamoDB**: DynamoService consulta a tabela e aplica as regras de negócio:
   - **Mais recente**: Ordena por timestamp decrescente, retorna o primeiro
   - **Mais antigo**: Ordena por timestamp crescente, retorna o primeiro  
   - **Todos**: Retorna todos os registros
5. **Processamento**: Lambda converte os dados e monta a resposta estruturada
6. **Resposta**: Retorna JSON formatado com os resultados ou erro

## Componentes Envolvidos

- **Function.cs**: Orquestrador principal
- **SecretService**: Gerencia acesso ao Secrets Manager
- **DynamoService**: Gerencia consultas ao DynamoDB
- **QueryInputModel**: Modelo de entrada com validação
- **AWS Services**: Secrets Manager e DynamoDB
- **CloudWatch Logs**: Logging detalhado