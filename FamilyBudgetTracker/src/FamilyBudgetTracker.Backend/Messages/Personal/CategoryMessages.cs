using FamilyBudgetTracker.Backend.Constants.Personal;

namespace FamilyBudgetTracker.Backend.Messages.Personal;

public static class CategoryMessages
{
    public const string NameRequired = "Name is required";
    public const string TypeRequired = "Type is required";

    public static readonly string TypeMustBe = $"Type must be one of the following: " +
                                               $"{string.Join(",", CategoryConstants.Types)}";
    
    public const string DeleteImpossible = "You can not delete this category";


    public const string NoCategoryFound = "Category with id does not exist";

    public const string CategoryIsNotFromTheUser = "Category is not one from the user";
}