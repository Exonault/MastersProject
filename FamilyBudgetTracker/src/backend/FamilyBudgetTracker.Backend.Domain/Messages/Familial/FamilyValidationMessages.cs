namespace FamilyBudgetTracker.Backend.Domain.Messages.Familial;

public static class FamilyValidationMessages
{
    public const string NameRequired = "Name is required";

    public const string InvitesRequired = "Invite list is required";

    public const string ItemIsNotEmail = "Not all elements are email";

    public const string FamilyNotFound = "Family not found";

    public const string DeleteImpossible = "FamilyMember is not the family admin. Cannot delete family";
}