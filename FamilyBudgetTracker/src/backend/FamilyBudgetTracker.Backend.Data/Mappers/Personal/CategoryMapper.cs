using FamilyBudgetTracker.Backend.Domain.Entities.Common;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.Category;

namespace FamilyBudgetTracker.Backend.Data.Mappers.Personal;

public static class CategoryMapper
{
    public static Category ToCategory(this CategoryRequest request)
    {
        return new Category
        {
            Name = request.Name,
            Icon = request.Icon,
            Type = Enum.Parse<CategoryType>(request.Type),
            Limit = request.Limit,
        };
    }

    public static Category ToCategory(this CategoryRequest request, Category origin)
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