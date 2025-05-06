using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;
using FamilyBudgetTracker.Shared.Contracts.User;

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
    
    public static FamilyResponse ToFamilyResponse(this Family family, List<FamilyMemberResponse> familyMembers)
    {
        return new FamilyResponse
        {
            Id = family.Id,
            Name = family.Name,
            FamilyMembers = familyMembers
        };
    }
}