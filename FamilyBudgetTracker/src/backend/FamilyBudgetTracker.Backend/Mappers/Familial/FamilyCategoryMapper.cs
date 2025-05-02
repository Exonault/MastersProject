using FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;
using FamilyBudgetTracker.BE.Commons.Entities.Common;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;

namespace FamilyBudgetTracker.Backend.Mappers.Familial;

public static class FamilyCategoryMapper
{
    public static FamilyCategory ToFamilyCategory(this CreateFamilyCategoryRequest request)
    {
        return new FamilyCategory
        {
            Name = request.Name,
            Icon = request.Icon,
            Type = Enum.Parse<CategoryType>(request.Type),
            Limit = request.Limit,
        };
    }


    public static FamilyCategory ToFamilyCategory(this UpdateFamilyCategoryRequest request, FamilyCategory origin)
    {
        origin.Name = request.Name;
        origin.Icon = request.Icon;
        origin.Type = Enum.Parse<CategoryType>(request.Type);
        origin.Limit = request.Limit;

        return origin;
    }


    public static FamilyCategoryResponse ToFamilyCategoryResponse(this FamilyCategory category)
    {
        return new FamilyCategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
            Type = category.Type.ToString(),
            Limit = category.Limit,
            Family = category.Family.ToFamilyResponse()
        };
    }
}