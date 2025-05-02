using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Util;
using FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Exceptions;
using FamilyBudgetTracker.BE.Commons.Mappers.Familial;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Familial;
using FamilyBudgetTracker.BE.Commons.Services.Familial;
using FamilyBudgetTracker.BE.Commons.Validation;

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

    public async Task CreateFamilyCategory(CreateFamilyCategoryRequest request, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyCategory familyCategory = request.ToFamilyCategory();
        familyCategory.Family = family;

        await _familyCategoryRepository.CreateFamilyCategory(familyCategory);
    }

    public async Task UpdateFamilyCategory(int id, UpdateFamilyCategoryRequest request, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetCategoryById(id);

        familyCategory = familyCategory.ValidateFamilyCategory(family.Id.ToString());

        FamilyCategory updatedCategory = request.ToFamilyCategory(familyCategory);

        await _familyCategoryRepository.UpdateFamilyCategory(updatedCategory);
    }

    public async Task DeleteFamilyCategory(int id, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetCategoryById(id);

        familyCategory = familyCategory.ValidateFamilyCategory(family.Id.ToString());

        await _familyCategoryRepository.DeleteFamilyCategory(familyCategory);
    }

    public async Task<FamilyCategoryResponse> GetFamilyCategoryById(int id, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetCategoryById(id);

        familyCategory = familyCategory.ValidateFamilyCategory(family.Id.ToString());

        FamilyCategoryResponse response = familyCategory.ToFamilyCategoryResponse();

        return response;
    }

    public async Task<List<FamilyCategoryResponse>> GetFamilyCategoriesByFamilyId(string familyId, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        List<FamilyCategory> categories = await _familyCategoryRepository.GetCategoriesByFamilyId(familyId);

        List<FamilyCategoryResponse> response = categories.Select(x => x.ToFamilyCategoryResponse())
            .ToList();

        return response;
    }
}