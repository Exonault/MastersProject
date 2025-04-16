using FamilyBudgetTracker.Entities.Contracts.Personal.Category;
using FamilyBudgetTracker.Entities.Entities.Personal;

namespace FamilyBudgetTracker.Backend.Mappers.Personal;

public static class CategoryMapper
{
    public static Category ToCategory(this CreateCategoryRequest request)
    {
        return new Category() //TODO
        {

        };
    }
}