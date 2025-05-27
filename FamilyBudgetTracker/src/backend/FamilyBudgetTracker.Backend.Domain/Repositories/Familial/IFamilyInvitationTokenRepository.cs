using FamilyBudgetTracker.Backend.Domain.Entities.Familial;

namespace FamilyBudgetTracker.Backend.Domain.Repositories.Familial;

public interface IFamilyInvitationTokenRepository
{
    Task CreateInvitationToken(FamilyInvitations token);
    
    Task DeleteInvitationToken(FamilyInvitations token);

    Task<FamilyInvitations?> GetInvitationToken(string id);
}