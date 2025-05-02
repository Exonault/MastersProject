using FamilyBudgetTracker.Backend.Constants.Personal;

namespace FamilyBudgetTracker.Backend.Messages.Personal;

public static class RecurringTransactionMessages
{
    public const string AmountRequired = "Amount is required";

    public const string AmountMustBeMoreThanZero = "Amount must be more than 0";

    public const string AmountValueMessage = "Amount must be with 2 digits after the decimal separator";

    public const string TypeRequired = "Type is required";

    public static readonly string TypeMustBe = $"Type must be one of the following: " +
                                               $"{string.Join(",", RecurringTransactionConstants.Types)}";

    public const string StartDateRequired = "Start date is required and must not be empty";

    public const string StartDateValueMessage = "Start date must be after today";

    public const string EndDateRequired = "End date is required and must not be empty";

    public const string EndDateValueMessage = "End date must be after start date";

    public const string DescriptionRequired = "Description is required";

    public const string NoTransactionFound = "No transaction found";

    public const string DeleteImpossible = "You cannot delete this transaction";

    public const string TransactionIsNotFromTheUser = "The transaction is not from the user";
}