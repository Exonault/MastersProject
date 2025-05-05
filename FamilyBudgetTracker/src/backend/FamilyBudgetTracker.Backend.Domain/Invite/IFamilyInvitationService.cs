using FamilyBudgetTracker.Shared.Contracts.Familial.Family;
using FamilyBudgetTracker.Shared.Contracts.Familial.Invite;

namespace FamilyBudgetTracker.Backend.Domain.Invite;

public interface IFamilyInvitationService
{
    Task InviteMembersToFamily(InviteFamilyMembersRequest request,  string familyId);

    Task AddUserToFamily(string tokenId);
}