namespace FamilyBudgetTracker.Backend.Constants.Personal;

public static class CategoryConstants
{
    public const string IncomeValue = "Income";
    public const string ExpenseValue = "Expense";
    
    public static readonly IReadOnlyList<string> Types =
    [
        IncomeValue,
        ExpenseValue,
    ];
}