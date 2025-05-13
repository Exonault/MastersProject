using BooksAPI.FE.Authentication;
using BooksAPI.FE.Constants;
using BooksAPI.FE.Interfaces;
using BooksAPI.FE.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BooksAPI.FE.Extensions;

public static class ServicesExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPersonalTransactionService, PersonalTransactionService>();
    }

    public static void AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(ApplicationConstants.PolicyNames.AdminRolePolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimNames.AdminRoleClaimName);
            })
            .AddPolicy(ApplicationConstants.PolicyNames.UserRolePolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimNames.UserRoleClaimName);
            });
    }
}