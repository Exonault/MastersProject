namespace FamilyBudgetTracker.Backend.Domain.Entities.Familial;

public class FamilyInvitationToken
{
    public Guid Id { get; set; }

    // public FamilyMember FamilyMember { get; set; }

    public string Email { get; set; }

    public string FamilyId { get; set; }

    public bool UserInApplication { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime ExpiresOnUtc { get; set; }
}