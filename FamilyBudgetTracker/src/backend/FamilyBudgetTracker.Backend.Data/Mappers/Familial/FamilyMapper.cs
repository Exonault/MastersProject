using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;

namespace FamilyBudgetTracker.Backend.Data.Mappers.Familial;

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