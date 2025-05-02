using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Category;
using FamilyBudgetTracker.BE.Commons.Entities.Common;
using FamilyBudgetTracker.BE.Commons.Entities.Personal;

namespace FamilyBudgetTracker.BE.Commons.Mappers.Personal;

public static class CategoryMapper
{
    public static Category ToCategory(this CreateCategoryRequest request)
    {
        return new Category
        {
            Name = request.Name,
            Icon = request.Icon,
            Type = Enum.Parse<CategoryType>(request.Type),
            Limit = request.Limit,
        };
    }

    public static Category ToCategory(this UpdateCategoryRequest request, Category origin)
    {
        origin.Name = request.Name;
        origin.Icon = request.Icon;
        origin.Type = Enum.Parse<CategoryType>(request.Type);
        origin.Limit = request.Limit;

        return origin;
    }

    public static CategoryResponse ToCategoryResponse(this Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
            Limit = category.Limit,
            Type = category.Type.ToString(),
            // UserId = category.User.Id
        };
    }
}