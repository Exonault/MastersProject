using FamilyBudgetTracker.Backend.Data.Validation;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using FamilyBudgetTracker.Backend.Domain.Invite;
using FamilyBudgetTracker.Backend.Domain.Messages.User;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;

namespace FamilyBudgetTracker.Backend.DomainServices.FamilyInvitation;

public class FamilyInvitationService : IFamilyInvitationService
{
    private readonly IFamilyInvitationTokenRepository _familyInvitationTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFamilyRepository _familyRepository;


    public FamilyInvitationService(IUserRepository userRepository, IFamilyInvitationTokenRepository familyInvitationTokenRepository,
        IFamilyRepository familyRepository)
    {
        _userRepository = userRepository;
        _familyInvitationTokenRepository = familyInvitationTokenRepository;
        _familyRepository = familyRepository;
    }

    public async Task<bool> AddUserToFamily(string tokenId, string familyId)
    {
        var token = await _familyInvitationTokenRepository.GetInvitationToken(tokenId);

        if (token is null || token.ExpiresOnUtc < DateTime.UtcNow)
        {
            return false;
        }

        if (!token.UserInApplication)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        User? user = await _userRepository.GetByEmail(token.Email);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(token.FamilyId);

        family = family.ValidateFamily();

        user.Family = family;

        // await _userRepository.AddToRole(user, AuthenticationConstants.RoleTypes.FamilyMemberRoleType);
        await _userRepository.AddToRole(user, "FamilyMember");

        await _userRepository.UpdateUser(user);

        await _familyInvitationTokenRepository.DeleteInvitationToken(token);

        throw new NotImplementedException();
    }
}