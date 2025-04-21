using FamilyBudgetTracker.Backend.Mappers.Personal;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.Entities.Contracts.Personal.Category;
using FamilyBudgetTracker.Entities.Entities;
using FamilyBudgetTracker.Entities.Entities.Personal;
using FamilyBudgetTracker.Entities.Exceptions;
using FamilyBudgetTracker.Entities.Repositories;
using FamilyBudgetTracker.Entities.Repositories.Personal;
using FamilyBudgetTracker.Entities.Services.Personal;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyBudgetTracker.Backend.Services.Personal;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    // private readonly IValidator<CreateCategoryRequest> _createCategoryValidator;
    // private readonly IValidator<UpdateCategoryRequest> _updateCategoryValidator;

    public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository,
        IValidator<CreateCategoryRequest> createCategoryValidator,
        IValidator<UpdateCategoryRequest> updateCategoryValidator)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        // _createCategoryValidator = createCategoryValidator;
        // _updateCategoryValidator = updateCategoryValidator;
    }

    public async Task CreateCategory(CreateCategoryRequest request, string userId)
    {
        //check user
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        //validate
        // ValidationResult validationResult = await _createCategoryValidator.ValidateAsync(request);
        //
        // if (!validationResult.IsValid)
        // {
        //     throw new ValidationException(validationResult.Errors);
        // }

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
        //
        // ValidationResult validationResult = await _updateCategoryValidator.ValidateAsync(request);
        //
        // if (!validationResult.IsValid)
        // {
        //     throw new ValidationException(validationResult.Errors);
        // }

        Category updatedCategory = request.ToCategory();
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