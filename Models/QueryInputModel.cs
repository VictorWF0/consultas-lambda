namespace Consultas.Models
{
    // Define os tipos de consulta possíveis
    public enum QueryType
    {
        MostRecent,
        Oldest,
        All
    }

    // Modelo de entrada da Lambda
    public class QueryInputModel
    {
        // Indica qual tipo de consulta será feita
        public QueryType Query { get; set; }
    }
}
