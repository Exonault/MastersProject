using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Shared.Contracts.User;

namespace FamilyBudgetTracker.Backend.Data.Mappers;

public static class UserMapper
{
    public static User ToUser(this RegisterRequest request)
    {
        return new User
        {
            UserName = request.UserName,
            PasswordHash = request.Password,
            Email = request.Email
        };
    }

    public static FamilyMemberResponse ToUserResponse(this User user, string familyRole)
    {
        return new FamilyMemberResponse
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            FamilyRole = familyRole,
        };
    }
}