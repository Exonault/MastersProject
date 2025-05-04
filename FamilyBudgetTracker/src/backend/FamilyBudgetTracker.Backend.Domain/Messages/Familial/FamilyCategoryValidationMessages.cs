using FamilyBudgetTracker.Backend.Domain.Constants.Personal;

namespace FamilyBudgetTracker.Backend.Domain.Messages.Familial;

public class FamilyCategoryValidationMessages
{
    public const string NameRequired = "Name is required";
    public const string TypeRequired = "Type is required";

    public static readonly string TypeMustBe = $"Type must be one of the following: " +
                                               $"{string.Join(", ", CategoryConstants.Types)}";

    public const string FamilyCategoryNotFound = "Family category with id does not exist";

    public const string FamilyCategoryIsNotFromFamily = "Family category is not from family";

}