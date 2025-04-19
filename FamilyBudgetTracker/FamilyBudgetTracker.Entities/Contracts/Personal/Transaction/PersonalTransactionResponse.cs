using System.Text.Json.Serialization;
using FamilyBudgetTracker.Entities.Contracts.Personal.Category;

namespace FamilyBudgetTracker.Entities.Contracts.Personal.Transaction;

public class PersonalTransactionResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("transactionDate")]
    public DateOnly TransactionDate { get; set; }

    [JsonPropertyName("category")]
    public CategoryResponse Category { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; }  
}