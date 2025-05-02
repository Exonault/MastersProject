using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;

namespace FamilyBudgetTracker.BE.Commons.Mappers.Familial;

public static class FamilyMapper
{
    public static FamilyResponse ToFamilyResponse(this Family family)
    {
        return new FamilyResponse
        {
            Id = family.Id,
            Name = family.Name,
        };
    }
}