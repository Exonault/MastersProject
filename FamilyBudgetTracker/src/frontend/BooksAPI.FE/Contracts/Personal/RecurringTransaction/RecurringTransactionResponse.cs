using System.Text.Json.Serialization;
using BooksAPI.FE.Contracts.Personal.Category;

namespace BooksAPI.FE.Contracts.Personal.RecurringTransaction;

public class RecurringTransactionResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("startDate")]
    public DateOnly StartDate { get; set; }

    [JsonPropertyName("nextExecutionDate")]
    public DateOnly NextExecutionDate { get; set; } // Calculated on execution in cronjob?

    [JsonPropertyName("endDate")]
    public DateOnly EndDate { get; set; }

    [JsonPropertyName("category")]
    public CategoryResponse Category { get; set; }
}