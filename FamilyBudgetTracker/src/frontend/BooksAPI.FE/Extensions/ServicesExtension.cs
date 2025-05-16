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
        services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();

        services.AddScoped<IFamilyService, FamilyService>();
        services.AddScoped<IFamilyCategoryService, FamilyCategoryService>();
        services.AddScoped<IFamilyTransactionService, FamilyTransactionService>();
    }

    public static void AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(ApplicationConstants.PolicyNames.AdminRolePolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimNames.AdminRoleClaimName);

                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);
            })
            .AddPolicy(ApplicationConstants.PolicyNames.UserRolePolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimNames.UserRoleClaimName);
                
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);

            })
            .AddPolicy(ApplicationConstants.PolicyNames.FamilyAdminPolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimNames.UserRoleClaimName, 
                    ApplicationConstants.ClaimNames.FamilyAdminClaimName);
                
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimFamilyIdType);

            })
            .AddPolicy(ApplicationConstants.PolicyNames.FamilyMemberPolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimNames.UserRoleClaimName, 
                    ApplicationConstants.ClaimNames.FamilyMemberClaimName);
                
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimFamilyIdType);

            });
    }
}