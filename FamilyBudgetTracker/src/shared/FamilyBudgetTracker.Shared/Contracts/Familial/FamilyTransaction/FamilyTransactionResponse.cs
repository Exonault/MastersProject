using System.Text.Json.Serialization;
using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyCategory;
using FamilyBudgetTracker.Shared.Contracts.User;

namespace FamilyBudgetTracker.Shared.Contracts.Familial.FamilyTransaction;

public class FamilyTransactionResponse
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
    public FamilyCategoryResponse Category { get; set; }

    [JsonPropertyName("user")]
    public FamilyMemberResponse FamilyMember { get; set; }

    // public FamilyResponse Family { get; set; }
}