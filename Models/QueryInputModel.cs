using System.Text.Json.Serialization;

namespace Consultas.Models
{
    public enum QueryType
    {
        MostRecent,
        Oldest,
        All
    }

    public class QueryInputModel
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QueryType Query { get; set; }
    }
}
