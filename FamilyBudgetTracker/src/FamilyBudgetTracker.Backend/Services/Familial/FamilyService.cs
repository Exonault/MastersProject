using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Backend.Mappers;
using FamilyBudgetTracker.Backend.Mappers.Familial;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Messages.Familial;
using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;
using FamilyBudgetTracker.BE.Commons.Contracts.User;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Exceptions;
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


    public FamilyService(IFamilyRepository familyRepository, IUserRepository userRepository, ISendEmailService sendEmailService)
    {
        _familyRepository = familyRepository;
        _userRepository = userRepository;
        _sendEmailService = sendEmailService;
    }

    public async Task CreateFamily(CreateFamilyRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        Family family = new Family
        {
            Name = request.Name,
        };

        await _familyRepository.CreateFamily(family);

        user.Family = family;

        await _userRepository.UpdateUser(user);

        await _userRepository.AddToRole(user, ApplicationConstants.RoleTypes.FamilyAdminRoleType);


        //TODO send emails to join with code. 
        // Similar to https://www.youtube.com/watch?v=KtCjH-1iCIk
    }

    public async Task DeleteFamily(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        if (user.Family is null)
        {
            throw new ResourceNotFoundException(UserMessages.ValidationMessages.NoFamilyForUser);
        }

        Family? family = await _familyRepository.GetFamilyById(id);

        if (family is null)
        {
            throw new ResourceNotFoundException(FamilyMessages.FamilyNotFound);
        }

        if (user.Family.Id != family.Id)
        {
            throw new InvalidOperationException(UserMessages.ValidationMessages.UserIsNotFromFamily);
        }

        List<string> roles = await _userRepository.GetAllRoles(user);

        if (!roles.Contains(ApplicationConstants.RoleTypes.FamilyAdminRoleType))
        {
            throw new InvalidOperationException(FamilyMessages.DeleteImpossible);
        }

        await _familyRepository.DeleteFamily(family);
    }

    public async Task<FamilyResponse> GetFamilyById(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        if (user.Family is null)
        {
            throw new ResourceNotFoundException(UserMessages.ValidationMessages.NoFamilyForUser);
        }

        Family? family = await _familyRepository.GetFamilyById(id);

        if (family is null)
        {
            throw new ResourceNotFoundException(FamilyMessages.FamilyNotFound);
        }

        if (user.Family.Id != family.Id)
        {
            throw new InvalidOperationException(UserMessages.ValidationMessages.UserIsNotFromFamily);
        }

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