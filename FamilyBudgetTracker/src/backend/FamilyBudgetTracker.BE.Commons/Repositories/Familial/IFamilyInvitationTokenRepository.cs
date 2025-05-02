using FamilyBudgetTracker.BE.Commons.Entities.Familial;

namespace FamilyBudgetTracker.BE.Commons.Repositories.Familial;

public interface IFamilyInvitationTokenRepository
{
    Task CreateInvitationToken(FamilyInvitationToken token);
    
    Task DeleteInvitationToken(FamilyInvitationToken token);

    Task<FamilyInvitationToken?> GetInvitationToken(string id);
}