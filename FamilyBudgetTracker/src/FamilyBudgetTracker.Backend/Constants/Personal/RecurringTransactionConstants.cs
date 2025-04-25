namespace FamilyBudgetTracker.Backend.Constants.Personal;

public class RecurringTransactionConstants
{
    public const string WeeklyValue = "weekly";
    public const string BiWeeklyValue = "biweekly";
    public const string MonthlyValue = "monthly";

    public static IReadOnlyList<string> Types =
    [
        WeeklyValue,
        BiWeeklyValue,
        MonthlyValue,
    ];
}