using FamilyBudgetTracker.Backend.Mappers.Personal;
using FamilyBudgetTracker.Entities.Contracts.Personal.Category;
using FamilyBudgetTracker.Entities.Entities;
using FamilyBudgetTracker.Entities.Entities.Personal;
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
    private readonly IValidator<Category> _validator;

    public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository,
        IValidator<Category> validator)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task CreateCategory(CreateCategoryRequest request)
    {
        //TODO check user
        User? user = await _userRepository.GetById(request.UserId);

        if (user is null)
        {
            throw new Exception("No user"); // Fix
        }

        //TODO check for possible entry

        //TODO map
        Category category = request.ToCategory();

        category.User = user;

        ValidationResult validationResult = await _validator.ValidateAsync(category);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _categoryRepository.CreateCategory(category);
    }
}