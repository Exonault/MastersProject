namespace FamilyBudgetTracker.BE.Commons.Entities.Familial;

public class FamilyVerificationToken
{
    public Guid Id { get; set; }

    // public User User { get; set; }

    public string Email { get; set; }

    public bool UserInApplication { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime ExpiresOnUtc { get; set; }
}