using FamilyBudgetTracker.Entities.Contracts.Personal.Category;

namespace FamilyBudgetTracker.Entities.Services.Personal;

public interface ICategoryService
{
    Task CreateCategory(CreateCategoryRequest request);
}