using Amazon.Lambda.Core;
using Consultas.Models;

// Serializer padrão da Lambda
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Consultas;

public class Function
{
    public async Task<string> FunctionHandler(QueryInputModel input, ILambdaContext context)
    {
        // Log básico para validar entrada
        context.Logger.LogLine("Lambda acionada com sucesso");

        context.Logger.LogLine($"Query recebida: {input.Query}");

        // Por enquanto, só devolvemos algo simples
        return $"Consulta solicitada: {input.Query}";
    }
}