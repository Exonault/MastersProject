namespace FamilyBudgetTracker.Backend.Domain.Invite;

public interface IFamilyInvitationService
{
    Task<bool> AddUserToFamily(string tokenId, string familyId);
}