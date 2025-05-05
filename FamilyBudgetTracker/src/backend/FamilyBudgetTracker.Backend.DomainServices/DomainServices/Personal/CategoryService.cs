using FamilyBudgetTracker.Backend.Data.Mappers.Personal;
using FamilyBudgetTracker.Backend.Data.Validation;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Personal;
using FamilyBudgetTracker.Backend.Domain.Services.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.Category;

namespace FamilyBudgetTracker.Backend.DomainServices.DomainServices.Personal;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;

    
    public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
    }

    public async Task CreateCategory(CategoryRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();
        
        Category category = request.ToCategory();

        category.User = user;

        await _categoryRepository.CreateCategory(category);
    }


    public async Task UpdateCategory(int id, CategoryRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Category? category = await _categoryRepository.GetCategoryById(id);
        
        category = category.ValidateCategory(user.Id);

        Category updatedCategory = request.ToCategory(category);

        await _categoryRepository.UpdateCategory(updatedCategory);
    }

    public async Task DeleteCategory(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Category? category = await _categoryRepository.GetCategoryById(id);

        category = category.ValidateCategory(user.Id);

        await _categoryRepository.DeleteCategory(category);
    }

    public async Task<CategoryResponse> GetCategory(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Category? category = await _categoryRepository.GetCategoryById(id);

        category = category.ValidateCategory(user.Id);

        CategoryResponse response = category.ToCategoryResponse();

        return response;
    }

    public async Task<List<CategoryResponse>> GetCategoriesForUser(string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        List<Category> categories = await _categoryRepository.GetAllCategoriesForUser(user.Id);

        List<CategoryResponse> response = categories.Select(x => x.ToCategoryResponse())
            .ToList();

        return response;
    }
    
}