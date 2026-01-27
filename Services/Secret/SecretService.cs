using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;

namespace testeConsultas.Services.Secret;

// Serviço responsável por buscar informações no AWS Secrets Manager
public class SecretService
{
    private readonly IAmazonSecretsManager _secretsManager;

    // Nome do secret configurado no AWS
    private const string SECRET_NAME = "secret_test_joao";
    // Chave que contém o nome da tabela dentro do secret
    private const string TABLE_NAME_KEY = "table_name";

    // Construtor que recebe o cliente do Secrets Manager
    public SecretService(IAmazonSecretsManager secretsManager)
    {
        _secretsManager = secretsManager;
    }

    // Busca o nome da tabela DynamoDB armazenado no secret
    // Retorna: Nome da tabela
    public async Task<string> BuscarNomeDaTabela()
    {
        return await BuscarValorDoSecret(SECRET_NAME, TABLE_NAME_KEY);
    }

    // Busca o valor de uma chave específica dentro de um secret
    // secretName: Nome do secret no AWS (ex: "secret_test_joao")
    // key: Chave dentro do secret (ex: "table_name")
    // Retorna: Valor encontrado no secret
    private async Task<string> BuscarValorDoSecret(string secretName, string key)
    {
        // Faz a requisição para buscar o secret
        var request = new GetSecretValueRequest
        {
            SecretId = secretName
        };

        var response = await _secretsManager.GetSecretValueAsync(request);

        // O secret vem como JSON, então precisamos fazer o parse
        var secretJson = response.SecretString;
        var secretDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(secretJson);

        // Busca a chave específica no dicionário
        if (secretDictionary != null && secretDictionary.ContainsKey(key))
        {
            return secretDictionary[key];
        }

        throw new Exception($"Chave '{key}' não encontrada no secret '{secretName}'");
    }
}