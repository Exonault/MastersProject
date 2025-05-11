namespace FamilyBudgetTracker.FE.Messages;

public static class OrderMessages
{
    public const string DateRequired = "Date is required";
    
    public const string NotInFuture = "Date cannot be in the future";
    
    public const string DescriptionRequired = "Description is required";

    public const string StatusRequired = "Status is required";

    public const string PlaceRequired = "Place is required";

    public const string AmountRequired = "Amount is required";

    public const string AmountMoreThanZero = "Amount must be more than zero";

    public const string NumberOfItemsRequired = "Number of items is required";

    public const string NumberOfItemsAtLeastOne = "Number of items must be greater than or equal to one";

   
}