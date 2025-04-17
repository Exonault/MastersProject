using FamilyBudgetTracker.Entities.Contracts.Personal.Category;
using FamilyBudgetTracker.Entities.Entities.Common;
using FamilyBudgetTracker.Entities.Entities.Personal;
using FamilyBudgetTracker.Entities.Exceptions;

namespace FamilyBudgetTracker.Backend.Mappers.Personal;

public static class CategoryMapper
{
    public static Category ToCategory(this CreateCategoryRequest request)
    {
        CategoryType type;
        try
        {
            type = (CategoryType)Enum.Parse(typeof(CategoryType), request.Type);
        }
        catch (Exception e)
        {
            throw new MappingException("Type is not 'Income' or 'Expense'.");
        }


        return new Category()
        {
            Name = request.Name,
            Icon = request.Icon,
            Type = type,
            Limit = request.Limit,
        };
    }

    public static CategoryResponse ToCategoryResponse(this Category category)
    {
        return new CategoryResponse()
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
            Limit = category.Limit,
            Type = category.Type.ToString(),
            UserId = category.User.Id
        };
    }
}