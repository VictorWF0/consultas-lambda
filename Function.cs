using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.SecretsManager;
using testeConsultas.Models;
using testeConsultas.Services.Secret;
using testeConsultas.Services.Dynamo;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace testeConsultas;

public class Function
{
    private readonly SecretService _secretService;
    private readonly DynamoService _dynamoService;

    // Construtor padrão - usado pela Lambda
    public Function()
    {
        // Inicializa os clientes da AWS
        var secretsManagerClient = new AmazonSecretsManagerClient();
        var dynamoClient = new AmazonDynamoDBClient();

        // Inicializa os serviços
        _secretService = new SecretService(secretsManagerClient);
        _dynamoService = new DynamoService(dynamoClient);
    }

    // Handler principal da Lambda
    // Recebe o input do usuário e retorna os dados da tabela DynamoDB
    // input: Input do usuário com o tipo de consulta
    // context: Contexto da Lambda
    // Retorna: JSON com os resultados da consulta
    public async Task<object> FunctionHandler(UserInput input, ILambdaContext context)
    {
        try
        {
            // Validação: verifica se o input é válido
            if (input == null)
            {
                context.Logger.LogError("Input nulo recebido.");
                return JsonSerializer.Serialize(new { erro = "Input inválido ou ausente" });
            }

            // Log do input recebido
            context.Logger.LogInformation($"Tipo de consulta recebida: {input.TipoConsulta}");

            // Passo 1: Buscar o nome da tabela no Secrets Manager
            context.Logger.LogInformation("Buscando nome da tabela no Secrets Manager...");
            string tableName = await _secretService.BuscarNomeDaTabela();
            context.Logger.LogInformation($"Nome da tabela obtido: {tableName}");

            // Passo 2: Executar a consulta no DynamoDB de acordo com o tipo solicitado
            object resultado;

            // Regra de negócio: verifica qual tipo de consulta o usuário quer
            switch (input.TipoConsulta)
            {
                case TipoConsulta.MaisRecente:
                    context.Logger.LogInformation("Buscando registro mais recente...");
                    resultado = await _dynamoService.BuscarMaisRecente(tableName);
                    break;

                case TipoConsulta.MaisAntigo:
                    context.Logger.LogInformation("Buscando registro mais antigo...");
                    resultado = await _dynamoService.BuscarMaisAntigo(tableName);
                    break;

                case TipoConsulta.Todos:
                    context.Logger.LogInformation("Buscando todos os registros...");
                    resultado = await _dynamoService.BuscarTodos(tableName);
                    break;

                default:
                    // Se o tipo de consulta não for reconhecido, retorna erro
                    var erro = $"Tipo de consulta inválido: {input.TipoConsulta}";
                    context.Logger.LogError(erro);
                    return JsonSerializer.Serialize(new { erro });
            }

            // Retorna o resultado em formato JSON
            var resultadoJson = JsonSerializer.Serialize(new
            {
                tipoConsulta = input.TipoConsulta,
                tabela = tableName,
                dados = resultado
            }, new JsonSerializerOptions
            {
                WriteIndented = true // Formata o JSON com indentação
            });

            context.Logger.LogInformation("Consulta realizada com sucesso!");
            return resultadoJson;
        }
        catch (Exception ex)
        {
            // Em caso de erro, loga os detalhes completos (incluindo stackTrace)
            context.Logger.LogError($"Erro ao processar a requisição: {ex.Message}");
            context.Logger.LogError($"StackTrace: {ex.StackTrace}");

            // Retorna apenas uma mensagem genérica ao usuário (sem expor detalhes internos)
            return JsonSerializer.Serialize(new { erro = "Erro interno ao processar a requisição. Verifique os logs para mais detalhes." });
        }
    }
}