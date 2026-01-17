using Amazon.Lambda.Core;
using Consultas.Models;

// Serializer padrão da Lambda
[assembly: LambdaSerializer(
    typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer)
)]

namespace Consultas
{
    public class Function
    {
        public string FunctionHandler(QueryInputModel input, ILambdaContext context)
        {
            context.Logger.LogLine($"Query recebida: {input.Query}");

            // Por enquanto não faz mais nada
            return "ok";
        }
    }
}
