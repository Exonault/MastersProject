using FamilyBudgetTracker.Backend.Domain.Entities.Common;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.Category;

namespace FamilyBudgetTracker.Backend.Data.Mappers.Personal;

public static class CategoryMapper
{
    public static PersonalCategory ToCategory(this CategoryRequest request)
    {
        return new PersonalCategory
        {
            Name = request.Name,
            Icon = request.Icon,
            Type = Enum.Parse<CategoryType>(request.Type),
            Limit = request.Limit,
        };
    }

    public static PersonalCategory ToCategory(this CategoryRequest request, PersonalCategory origin)
    {
        origin.Name = request.Name;
        origin.Icon = request.Icon;
        origin.Type = Enum.Parse<CategoryType>(request.Type);
        origin.Limit = request.Limit;

        return origin;
    }

    public static CategoryResponse ToCategoryResponse(this PersonalCategory personalCategory)
    {
        return new CategoryResponse
        {
            Id = personalCategory.Id,
            Name = personalCategory.Name,
            Icon = personalCategory.Icon,
            Limit = personalCategory.Limit,
            Type = personalCategory.Type.ToString(),
            // UserId = category.FamilyMember.Id
        };
    }
}