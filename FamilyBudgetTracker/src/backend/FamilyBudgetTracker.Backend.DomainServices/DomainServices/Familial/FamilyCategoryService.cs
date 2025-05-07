using FamilyBudgetTracker.Backend.Data.Mappers.Familial;
using FamilyBudgetTracker.Backend.Data.Validation;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using FamilyBudgetTracker.Backend.Domain.Services.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyCategory;

namespace FamilyBudgetTracker.Backend.DomainServices.DomainServices.Familial;

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

    public async Task CreateFamilyCategory(FamilyCategoryRequest request, string userId, string familyId)
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

    public async Task UpdateFamilyCategory(int id, FamilyCategoryRequest request, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetFamilyCategoryById(id);

        familyCategory = familyCategory.ValidateFamilyCategory(family.Id);

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

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetFamilyCategoryById(id);

        familyCategory = familyCategory.ValidateFamilyCategory(family.Id);

        await _familyCategoryRepository.DeleteFamilyCategory(familyCategory);
    }

    public async Task<FamilyCategorySingleResponse> GetFamilyCategoryById(int id, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetFamilyCategoryById(id);

        familyCategory = familyCategory.ValidateFamilyCategory(family.Id);

        FamilyCategorySingleResponse response = familyCategory.ToFamilyCategorySingleResponse();

        return response;
    }

    public async Task<List<FamilyCategoryResponse>> GetFamilyCategoriesByFamilyId(string familyId, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        List<FamilyCategory> categories = await _familyCategoryRepository.GetFamilyCategoriesByFamilyId(familyId);

        List<FamilyCategoryResponse> response = categories.Select(x => x.ToFamilyCategoryResponse())
            .ToList();

        return response;
    }
}