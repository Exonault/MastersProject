using System.Text.Json.Serialization;

namespace FamilyBudgetTracker.BE.Commons.Contracts.Personal.Transaction;

public class TransactionForPeriodSummaryResponse
{
    [JsonPropertyName("incomeAmount")]
    public decimal IncomeAmount { get; set; }
    
    [JsonPropertyName("expenseAmount")]
    public decimal ExpenseAmount { get; set; }
    
    [JsonPropertyName("expenseCategoryAmounts")]
    public Dictionary<string, decimal> ExpenseCategoryAmounts { get; set; }
}