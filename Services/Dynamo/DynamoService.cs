using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using testeConsultas.Models;

namespace testeConsultas.Services.Dynamo;

// Serviço responsável por consultar informações no DynamoDB
public class DynamoService
{
    private readonly IAmazonDynamoDB _dynamoClient;

    // Construtor que recebe o cliente do DynamoDB
    public DynamoService(IAmazonDynamoDB dynamoClient)
    {
        _dynamoClient = dynamoClient;
    }

    // Busca o registro mais recente da tabela
    // tableName: Nome da tabela no DynamoDB
    // Retorna: Objeto Ticket com os dados do registro mais recente
    public async Task<Ticket?> BuscarMaisRecente(string tableName)
    {
        var request = new ScanRequest
        {
            TableName = tableName
        };

        var response = await _dynamoClient.ScanAsync(request);

        if (response.Items.Count > 0)
        {
            // Ordena os itens por timestamp (String) de forma decrescente
            // O formato "yyyy-MM-dd HH:mm:ss" permite ordenação lexicográfica correta
            var maisRecente = response.Items
                .OrderByDescending(item => item.ContainsKey("timestamp") ? item["timestamp"].S : string.Empty)
                .FirstOrDefault();

            // Converte o Dictionary do DynamoDB para objeto Ticket simples
            return maisRecente != null ? ConverterParaTicket(maisRecente) : null;
        }

        return null;
    }

    // Busca o registro mais antigo da tabela
    // tableName: Nome da tabela no DynamoDB
    // Retorna: Objeto Ticket com os dados do registro mais antigo
    public async Task<Ticket?> BuscarMaisAntigo(string tableName)
    {
        var request = new ScanRequest
        {
            TableName = tableName
        };

        var response = await _dynamoClient.ScanAsync(request);

        if (response.Items.Count > 0)
        {
            // Ordena os itens por timestamp (String) de forma crescente
            var maisAntigo = response.Items
                .OrderBy(item => item.ContainsKey("timestamp") ? item["timestamp"].S : string.Empty)
                .FirstOrDefault();

            // Converte o Dictionary do DynamoDB para objeto Ticket simples
            return maisAntigo != null ? ConverterParaTicket(maisAntigo) : null;
        }

        return null;
    }

    // Busca todos os registros da tabela
    // tableName: Nome da tabela no DynamoDB
    // Retorna: Lista de objetos Ticket com todos os registros
    public async Task<List<Ticket>> BuscarTodos(string tableName)
    {
        var request = new ScanRequest
        {
            TableName = tableName
        };

        var response = await _dynamoClient.ScanAsync(request);

        // Converte cada Dictionary do DynamoDB para objeto Ticket simples
        return response.Items.Select(item => ConverterParaTicket(item)).ToList();
    }

    // Converte um Dictionary do DynamoDB para um objeto Ticket simples
    // item: Dictionary com AttributeValue do DynamoDB
    // Retorna: Objeto Ticket com valores limpos
    private Ticket ConverterParaTicket(Dictionary<string, AttributeValue> item)
    {
        return new Ticket
        {
            // Pega o valor String (.S) de cada campo, ou string vazia se não existir
            TicketNumber = item.ContainsKey("ticket_number") ? item["ticket_number"].S : string.Empty,
            Timestamp = item.ContainsKey("timestamp") ? item["timestamp"].S : string.Empty,
            AssignmentGroup = item.ContainsKey("assignment_group") ? item["assignment_group"].S : string.Empty,
            Description = item.ContainsKey("description") ? item["description"].S : string.Empty,
            Name = item.ContainsKey("name") ? item["name"].S : string.Empty,
            PhoneNumber = item.ContainsKey("phone_number") ? item["phone_number"].S : string.Empty,
            ShortDescription = item.ContainsKey("short_description") ? item["short_description"].S : string.Empty
        };
    }
}