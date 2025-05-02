using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Backend.Mappers;
using FamilyBudgetTracker.Backend.Mappers.Familial;
using FamilyBudgetTracker.Backend.Util;
using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;
using FamilyBudgetTracker.BE.Commons.Contracts.User;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Familial;
using FamilyBudgetTracker.BE.Commons.Services;
using FamilyBudgetTracker.BE.Commons.Services.Familial;

namespace FamilyBudgetTracker.Backend.Services.Familial;

public class FamilyService : IFamilyService
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISendEmailService _sendEmailService;
    private readonly IFamilyVerificationTokenRepository _familyVerificationTokenRepository;


    public FamilyService(IFamilyRepository familyRepository, IUserRepository userRepository, ISendEmailService sendEmailService,
        IFamilyVerificationTokenRepository familyVerificationTokenRepository)
    {
        _familyRepository = familyRepository;
        _userRepository = userRepository;
        _sendEmailService = sendEmailService;
        _familyVerificationTokenRepository = familyVerificationTokenRepository;
    }

    public async Task CreateFamily(CreateFamilyRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family family = new Family
        {
            Name = request.Name,
        };

        await _familyRepository.CreateFamily(family);

        user.Family = family;

        await _userRepository.UpdateUser(user);

        await _userRepository.AddToRole(user, ApplicationConstants.RoleTypes.FamilyAdminRoleType);
    }

    public async Task AddFamilyMembersToFamily(AddFamilyMembersRequest request, string userId, int familyId)
    {
        //TODO send emails to join with code. 
        // Similar to https://www.youtube.com/watch?v=KtCjH-1iCIk

        List<string> inviteList = request.InviteList;

        foreach (string email in inviteList)
        {
            User? user = await _userRepository.GetByEmail(email);

            bool userInApplicationFlag = user is null;

            var familyVerificationToken = new FamilyVerificationToken
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserInApplication = userInApplicationFlag,
                CreatedOnUtc = DateTime.UtcNow,
                ExpiresOnUtc = DateTime.UtcNow.AddDays(1)
            };

            await _familyVerificationTokenRepository.CreateVerificationToken(familyVerificationToken);
            
            
        }
    }


    public async Task DeleteFamily(string id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(id);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        await _familyRepository.DeleteFamily(family);
    }

    public async Task<FamilyResponse> GetFamilyById(string id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(id);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        var familyMemberResponse = await GetFamilyMembersResponse(family.FamilyMembers);

        FamilyResponse familyResponse = family.ToFamilyResponse();

        familyResponse.FamilyMembers = familyMemberResponse;

        return familyResponse;
    }

    public async Task<List<FamilyResponse>> GetAllFamilies()
    {
        List<Family> allFamilies = await _familyRepository.GetAllFamilies();

        List<FamilyResponse> familyResponses = [];

        foreach (Family family in allFamilies)
        {
            FamilyResponse familyResponse = family.ToFamilyResponse();

            familyResponse.FamilyMembers = await GetFamilyMembersResponse(family.FamilyMembers);

            familyResponses.Add(familyResponse);
        }

        return familyResponses;
    }


    private async Task<List<UserResponse>> GetFamilyMembersResponse(List<User> familyMembers)
    {
        List<UserResponse> familyMemberResponse = [];

        foreach (User familyMember in familyMembers)
        {
            string mainFamilyRole = await _userRepository.GetMainFamilyRole(familyMember);

            UserResponse userResponse = familyMember.ToUserResponse(mainFamilyRole);

            familyMemberResponse.Add(userResponse);
        }

        return familyMemberResponse;
    }
}