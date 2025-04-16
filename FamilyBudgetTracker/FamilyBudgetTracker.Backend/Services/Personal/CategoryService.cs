using FamilyBudgetTracker.Backend.Mappers.Personal;
using FamilyBudgetTracker.Entities.Contracts.Personal.Category;
using FamilyBudgetTracker.Entities.Entities.Personal;
using FamilyBudgetTracker.Entities.Repositories.Personal;
using FamilyBudgetTracker.Entities.Services.Personal;

namespace FamilyBudgetTracker.Backend.Services.Personal;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateCategory(CreateCategoryRequest request)
    {
        Category category = request.ToCategory();

        await _repository.CreateCategory(category);
    }
}