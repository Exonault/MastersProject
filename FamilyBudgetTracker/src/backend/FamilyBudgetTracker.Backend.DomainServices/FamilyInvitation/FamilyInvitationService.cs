using FamilyBudgetTracker.Backend.Data.Validation;
using FamilyBudgetTracker.Backend.Domain.Constants.User;
using FamilyBudgetTracker.Backend.Domain.Email;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using FamilyBudgetTracker.Backend.Domain.Invite;
using FamilyBudgetTracker.Backend.Domain.Messages.User;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using FamilyBudgetTracker.Backend.DomainServices.FamilyInvitation.Messages;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;

namespace FamilyBudgetTracker.Backend.DomainServices.FamilyInvitation;

public class FamilyInvitationService : IFamilyInvitationService
{
    private readonly IFamilyInvitationTokenRepository _familyInvitationTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFamilyRepository _familyRepository;
    private readonly ISendEmailService _sendEmailService;


    public FamilyInvitationService(IUserRepository userRepository, IFamilyInvitationTokenRepository familyInvitationTokenRepository,
        IFamilyRepository familyRepository, ISendEmailService sendEmailService)
    {
        _userRepository = userRepository;
        _familyInvitationTokenRepository = familyInvitationTokenRepository;
        _familyRepository = familyRepository;
        _sendEmailService = sendEmailService;
    }

    public async Task InviteMembersToFamily(InviteFamilyMembersRequest request, string familyId)
    {
        //TODO send emails to join with code. 
        // Similar to https://www.youtube.com/watch?v=KtCjH-1iCIk

        List<string> inviteList = request.EmailList;

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family.ValidateFamily();

        foreach (string email in inviteList)
        {
            User? user = await _userRepository.GetByEmail(email);

            bool userInApplicationFlag = user is null;

            var dateUtc = DateTime.UtcNow;
            var familyInvitationToken = new FamilyInvitationToken
            {
                Id = Guid.NewGuid(),
                Email = email,
                FamilyId = familyId,
                UserInApplication = userInApplicationFlag,
                CreatedOnUtc = dateUtc,
                ExpiresOnUtc = dateUtc.AddDays(1)
            };

            await _familyInvitationTokenRepository.CreateInvitationToken(familyInvitationToken);

            await _sendEmailService.SendFamilyInvitationEmail(familyInvitationToken);
        }
    }

    public async Task AddUserToFamily(string tokenId)
    {
        FamilyInvitationToken? token = await _familyInvitationTokenRepository.GetInvitationToken(tokenId);

        if (token is null || token.ExpiresOnUtc < DateTime.UtcNow)
        {
            throw new OperationNotAllowedException(FamilyInvitationMessages.InvitationTokenExpired);
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

        await _userRepository.AddToRole(user, UserConstants.RoleTypes.FamilyMemberRoleType);

        await _userRepository.UpdateUser(user);

        await _familyInvitationTokenRepository.DeleteInvitationToken(token);
    }
}