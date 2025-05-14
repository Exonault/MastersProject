using System.Text.Json.Serialization;

namespace BooksAPI.FE.Contracts.Familial.FamilyTransaction;

public class FamilyTransactionPeriodSummaryResponse
{
    [JsonPropertyName("totalIncomeAmount")]
    public decimal TotalIncomeAmount { get; set; }

    [JsonPropertyName("expenseAmount")]
    public decimal TotalExpenseAmount { get; set; }

    [JsonPropertyName("expenseCategoryAmounts")]
    public Dictionary<string, decimal> ExpenseCategoryAmounts { get; set; }
}