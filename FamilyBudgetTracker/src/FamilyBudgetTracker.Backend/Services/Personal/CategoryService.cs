using FamilyBudgetTracker.Backend.Mappers.Personal;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Category;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Personal;
using FamilyBudgetTracker.BE.Commons.Exceptions;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Personal;
using FamilyBudgetTracker.BE.Commons.Services.Personal;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Services.Personal;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
  

    public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
    }

    public async Task CreateCategory(CreateCategoryRequest request, string userId)
    {
        //check user
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }
        
        //map
        Category category = request.ToCategory();

        category.User = user;

        await _categoryRepository.CreateCategory(category);
    }

    public async Task UpdateCategory(int id, UpdateCategoryRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        Category? category = await _categoryRepository.GetCategoryById(id);

        if (category is null)
        {
            throw new ResourceNotFoundException(CategoryMessages.NoCategoryFound);
        }

        Category updatedCategory = request.ToCategory(category);
        updatedCategory.Id = category.Id;
        updatedCategory.User = user;
        //TODO: Should the lists be added here
        // updatedCategory.RecurringTransactions = category.RecurringTransactions;
        // updatedCategory.Transactions = category.Transactions;

        await _categoryRepository.UpdateCategory(updatedCategory);
    }

    public async Task DeleteCategory(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        Category? category = await _categoryRepository.GetCategoryById(id);

        if (category is null)
        {
            throw new ResourceNotFoundException(CategoryMessages.NoCategoryFound);
        }

        if (category.User.Id != userId)
        {
            throw new InvalidOperationException(CategoryMessages.DeleteImpossible);
        }

        await _categoryRepository.DeleteCategory(category);
    }

    public async Task<CategoryResponse> GetCategory(int id, string userId)
    {
        Category? category = await _categoryRepository.GetCategoryById(id);

        if (category is null)
        {
            throw new ResourceNotFoundException(CategoryMessages.NoCategoryFound);
        }
        
        if (category.User.Id != userId)
        {
            throw new InvalidOperationException(CategoryMessages.CategoryIsNotFromTheUser);
        }


        CategoryResponse response = category.ToCategoryResponse();

        return response;
    }

    public async Task<List<CategoryResponse>> GetCategoriesForUser(string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        List<Category> categories = await _categoryRepository.GetAllCategoriesForUser(userId);

        List<CategoryResponse> response = categories.Select(x => x.ToCategoryResponse())
            .ToList();

        return response;
    }
}