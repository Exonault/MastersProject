using System.Text;
using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Backend.Data;
using FamilyBudgetTracker.Backend.ExceptionHandlers;
using FamilyBudgetTracker.Backend.Repositories;
using FamilyBudgetTracker.Backend.Repositories.Personal;
using FamilyBudgetTracker.Backend.Services;
using FamilyBudgetTracker.Backend.Services.Personal;
using FamilyBudgetTracker.Backend.Validation.Personal.Category;
using FamilyBudgetTracker.Backend.Validation.Personal.PersonalTransaction;
using FamilyBudgetTracker.Entities.Contracts.Personal.Category;
using FamilyBudgetTracker.Entities.Contracts.Personal.Transaction;
using FamilyBudgetTracker.Entities.Entities;
using FamilyBudgetTracker.Entities.Repositories;
using FamilyBudgetTracker.Entities.Repositories.Personal;
using FamilyBudgetTracker.Entities.Services;
using FamilyBudgetTracker.Entities.Services.Personal;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationExceptionHandlers(this IServiceCollection services)
    {
        //Exception handlers
        services.AddProblemDetails();

        services.AddExceptionHandler<InvalidEmailExceptionHandler>();
        services.AddExceptionHandler<InvalidOperationExceptionHandler>();
        services.AddExceptionHandler<UserAlreadyRegisteredExceptionHandler>();
        services.AddExceptionHandler<UserNotFoundExceptionHandler>();
        services.AddExceptionHandler<MappingExceptionHandler>();
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<BadHttpRequestExceptionHandler>();
//Global exception handler should be last
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddUserServices();

        services.AddCategoryServices();
        services.AddPersonalTransactionServices();

        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddFluentValidationAutoValidation();
    }

    private static void AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
    }


    private static void AddCategoryServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
    }


    private static void AddPersonalTransactionServices(this IServiceCollection services)
    {
        services.AddScoped<IPersonalTransactionRepository, PersonalTransactionRepository>();
        services.AddScoped<IPersonalTransactionService, PersonalTransactionService>();
    }

    public static void AddApplicationAuthenticationServices(this IServiceCollection service,
        ConfigurationManager configuration)
    {
        service.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddRoles<IdentityRole>()
            .AddSignInManager();

        service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    ClockSkew = TimeSpan.FromSeconds(5),
                };
            });

        service.AddAuthorization(options =>
        {
            options.AddPolicy(ApplicationConstants.PolicyNames.AdminRolePolicyName,
                p =>
                {
                    p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                        ApplicationConstants.ClaimNames.AdminRoleClaimName);

                    p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);
                });

            options.AddPolicy(ApplicationConstants.PolicyNames.UserRolePolicyName,
                p =>
                {
                    p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                        ApplicationConstants.ClaimNames.UserRoleClaimName);

                    p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);
                });
        });
    }

    public static void AddApplicationCachingServices(this IServiceCollection service,
        ConfigurationManager configuration)
    {
        service.AddOutputCache()
            .AddStackExchangeRedisOutputCache(options =>
            {
                options.InstanceName = configuration["Redis:Name"]!;
                options.Configuration = configuration["Redis:Url"];
            });
    }
}