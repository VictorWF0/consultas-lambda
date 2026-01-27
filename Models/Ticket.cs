namespace testeConsultas.Models;

// Modelo que representa um ticket do DynamoDB de forma simples e legível
public class Ticket
{
    // Número do ticket (chave primária)
    public string TicketNumber { get; set; } = string.Empty;

    // Data e hora do registro
    public string Timestamp { get; set; } = string.Empty;

    // Grupo de atribuição
    public string AssignmentGroup { get; set; } = string.Empty;

    // Descrição completa
    public string Description { get; set; } = string.Empty;

    // Nome do responsável
    public string Name { get; set; } = string.Empty;

    // Número de telefone
    public string PhoneNumber { get; set; } = string.Empty;

    // Descrição curta
    public string ShortDescription { get; set; } = string.Empty;
}