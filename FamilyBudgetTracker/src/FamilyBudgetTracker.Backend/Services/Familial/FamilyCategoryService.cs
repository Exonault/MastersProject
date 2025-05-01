using FamilyBudgetTracker.Backend.Mappers.Familial;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Messages.Familial;
using FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Exceptions;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Familial;
using FamilyBudgetTracker.BE.Commons.Services.Familial;

namespace FamilyBudgetTracker.Backend.Services.Familial;

public class FamilyCategoryService : IFamilyCategoryService
{
    private readonly IFamilyCategoryRepository _familyCategoryRepository;
    private readonly IFamilyRepository _familyRepository;
    private readonly IUserRepository _userRepository;


    public FamilyCategoryService(IFamilyCategoryRepository familyCategoryRepository, IUserRepository userRepository,
        IFamilyRepository familyRepository)
    {
        _familyCategoryRepository = familyCategoryRepository;
        _userRepository = userRepository;
        _familyRepository = familyRepository;
    }

    public async Task CreateFamilyCategory(CreateFamilyCategoryRequest request, string userId)
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

        Family? family = await _familyRepository.GetFamilyById(request.FamilyId);

        if (family is null)
        {
            throw new ResourceNotFoundException(FamilyMessages.FamilyNotFound);
        }

        if (user.Family.Id != family.Id)
        {
            throw new InvalidOperationException(UserMessages.ValidationMessages.UserIsNotFromFamily);
        }

        FamilyCategory familyCategory = request.ToFamilyCategory();
        familyCategory.Family = family;

        await _familyCategoryRepository.CreateFamilyCategory(familyCategory);
    }

    public async Task UpdateFamilyCategory(int id, UpdateFamilyCategoryRequest request, string userId)
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

        Family? family = await _familyRepository.GetFamilyById(request.FamilyId);

        if (family is null)
        {
            throw new ResourceNotFoundException(FamilyMessages.FamilyNotFound);
        }

        if (user.Family.Id != family.Id)
        {
            throw new InvalidOperationException(UserMessages.ValidationMessages.UserIsNotFromFamily);
        }

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetCategoryById(id);

        if (familyCategory is null)
        {
            throw new ResourceNotFoundException(FamilyCategoryMessages.FamilyCategoryNotFound);
        }

        if (familyCategory.Family.Id != family.Id)
        {
            throw new InvalidOperationException(FamilyCategoryMessages.FamilyCategoryIsNotFromFamily);
        }

        FamilyCategory updatedCategory = request.ToFamilyCategory(familyCategory);
        updatedCategory.Id = familyCategory.Id;
        updatedCategory.Family = family;

        await _familyCategoryRepository.UpdateFamilyCategory(updatedCategory);
    }

    public async Task DeleteFamilyCategory(int id, string userId)
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

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetCategoryById(id);

        if (familyCategory is null)
        {
            throw new ResourceNotFoundException(FamilyCategoryMessages.FamilyCategoryNotFound);
        }

        if (user.Family.Id != familyCategory.Family.Id)
        {
            throw new InvalidOperationException(FamilyCategoryMessages.FamilyCategoryIsNotFromFamily);
        }

        await _familyCategoryRepository.DeleteFamilyCategory(familyCategory);
    }

    public async Task<FamilyCategoryResponse> GetFamilyCategoryById(int id, string userId)
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

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetCategoryById(id);

        if (familyCategory is null)
        {
            throw new ResourceNotFoundException(FamilyCategoryMessages.FamilyCategoryNotFound);
        }

        if (user.Family.Id != familyCategory.Family.Id)
        {
            throw new InvalidOperationException(FamilyCategoryMessages.FamilyCategoryIsNotFromFamily);
        }

        FamilyCategoryResponse response = familyCategory.ToFamilyCategoryResponse();

        return response;
    }

    public async Task<List<FamilyCategoryResponse>> GetFamilyCategoriesByFamilyId(int familyId, string userId)
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

        if (user.Family.Id != familyId)
        {
            throw new InvalidOperationException(UserMessages.ValidationMessages.UserIsNotFromFamily);
        }

        List<FamilyCategory> categories = await _familyCategoryRepository.GetCategoriesByFamilyId(familyId);

        List<FamilyCategoryResponse> response = categories.Select(x => x.ToFamilyCategoryResponse())
            .ToList();

        return response;
    }
}