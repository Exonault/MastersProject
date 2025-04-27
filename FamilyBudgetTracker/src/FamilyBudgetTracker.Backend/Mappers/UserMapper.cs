using FamilyBudgetTracker.BE.Commons.Contracts.User;
using FamilyBudgetTracker.BE.Commons.Entities;

namespace FamilyBudgetTracker.Backend.Mappers;

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

    public static UserResponse ToUserResponse(this User user, string familyRole)
    {
        return new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            FamilyRole = familyRole,
        };
    }
}