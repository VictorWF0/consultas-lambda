using System.Text.Json.Serialization;

namespace testeConsultas.Models;

// Define os tipos de consulta possíveis
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TipoConsulta
{
    MaisRecente,
    MaisAntigo,
    Todos
}

// Modelo que representa o input do usuário
public class UserInput
{
    // Tipo de consulta que o usuário deseja fazer
    public TipoConsulta TipoConsulta { get; set; }
}