using FamilyBudgetTracker.Backend.Domain.Entities.Familial;

namespace FamilyBudgetTracker.Backend.Domain.Repositories.Familial;

public interface IFamilyInvitationTokenRepository
{
    Task CreateInvitationToken(FamilyInvitationToken token);
    
    Task DeleteInvitationToken(FamilyInvitationToken token);

    Task<FamilyInvitationToken?> GetInvitationToken(string id);
}