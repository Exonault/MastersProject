using FamilyBudgetTracker.Backend.Domain.Entities.Familial;

namespace FamilyBudgetTracker.Backend.Domain.Invite;

public interface IFamilyInvitationLinkFactory
{
    public string Create(FamilyInvitationToken token);
}