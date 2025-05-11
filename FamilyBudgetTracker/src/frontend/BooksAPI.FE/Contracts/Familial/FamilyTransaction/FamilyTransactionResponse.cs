using System.Text.Json.Serialization;
using BooksAPI.FE.Contracts.Familial.FamilyCategory;
using BooksAPI.FE.Contracts.User;

namespace BooksAPI.FE.Contracts.Familial.FamilyTransaction;

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

    // [JsonPropertyName("family")]
    // public FamilyResponse FamilyDetailed { get; set; }
}