namespace FamilyBudgetTracker.Backend.Domain.Messages.Personal;

public static class PersonalTransactionValdationMessages
{
    public const string AmountRequired = "Amount is required";

    public const string AmountMustBeMoreThanZero = "Amount must be more than 0";

    public const string AmountValueMessage = "Amount must be with 2 digits after the decimal separator";
    
    public const string DateRequired = "Date is required and must not be empty";

    public const string DateValueMessage = "Date must be before today";

    public const string DescriptionRequired = "Description is required";

    public const string NoTransactionFound = "No transaction found";

    public const string DeleteImpossible = "You cannot delete this transaction";

    public const string TransactionIsNotFromTheUser = "The transaction is not from the user";

}
    