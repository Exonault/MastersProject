namespace FamilyBudgetTracker.Backend.Domain.Constants.Personal;

public class RecurringTransactionConstants
{
    public const string WeeklyValue = "Weekly";
    public const string BiWeeklyValue = "Biweekly";
    public const string MonthlyValue = "Monthly";

    public static IReadOnlyList<string> Types =
    [
        WeeklyValue,
        BiWeeklyValue,
        MonthlyValue,
    ];
}