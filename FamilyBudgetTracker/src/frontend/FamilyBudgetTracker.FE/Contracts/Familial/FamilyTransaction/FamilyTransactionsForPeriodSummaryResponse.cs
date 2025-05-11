using System.Text.Json.Serialization;

namespace FamilyBudgetTracker.FE.Contracts.Familial.FamilyTransaction;

public class FamilyTransactionsForPeriodSummaryResponse
{
    [JsonPropertyName("totalIncomeAmount")]
    public decimal TotalIncomeAmount { get; set; }

    [JsonPropertyName("expenseAmount")]
    public decimal TotalExpenseAmount { get; set; }

    [JsonPropertyName("expenseCategoryAmounts")]
    public Dictionary<string, decimal> ExpenseCategoryAmounts { get; set; }
}