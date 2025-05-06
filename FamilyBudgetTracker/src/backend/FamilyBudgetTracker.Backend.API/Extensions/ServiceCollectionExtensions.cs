using System.Text;
using FamilyBudgetTracker.Backend.API.Constants;
using FamilyBudgetTracker.Backend.API.ExceptionHandlers;
using FamilyBudgetTracker.Backend.API.OpenApi;
using FamilyBudgetTracker.Backend.API.Policy;
using FamilyBudgetTracker.Backend.Authentication.Constants;
using FamilyBudgetTracker.Backend.Data;
using FamilyBudgetTracker.Backend.Domain.Email;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Email.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        // services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
    }

    public static void AddDatabase(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(configuration.GetConnectionString("ApplicationDb")); });
    }

    public static void AddCorsServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(ApplicationConstants.Cors.CorsPolicy,
                policy => { policy.WithOrigins(configuration.GetSection("FrontEndUrl").Value!); });
        });
    }

    public static void AddApplicationExceptionHandlers(this IServiceCollection services)
    {
        services.AddProblemDetails();

        services.AddExceptionHandler<InvalidEmailExceptionHandler>();
        services.AddExceptionHandler<OperationNotAllowedExceptionHandler>();
        services.AddExceptionHandler<UserAlreadyRegisteredExceptionHandler>();
        services.AddExceptionHandler<UserNotFoundExceptionHandler>();
        services.AddExceptionHandler<MappingExceptionHandler>();
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<BadHttpRequestExceptionHandler>();
        services.AddExceptionHandler<ResourceNotFoundExceptionHandler>();
        //Global exception handler should be last
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public static void AddFluentValidationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddFluentValidationAutoValidation();
    }

    public static void AddEmailServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
            .AddSmtpSender(configuration["Email:Host"], configuration.GetValue<int>("Email:Port"));

        services.AddScoped<ISendEmailService, SendEmailService>();
    }

    public static void AddHangfireServices(this IServiceCollection services, ConfigurationManager configuration)
    {
    }


    public static void AddApplicationCachingServices(this IServiceCollection service, ConfigurationManager configuration)
    {
        service.AddOutputCache(options =>
            {
                options.AddPolicy(nameof(AuthenticatedUserCachePolicy), AuthenticatedUserCachePolicy.Instance);
            })
            .AddStackExchangeRedisOutputCache(options =>
            {
                options.InstanceName = configuration["Redis:Name"]!;
                options.Configuration = configuration["Redis:Url"];
            });
    }

    public static void AddApplicationAuthenticationServices(this IServiceCollection service, ConfigurationManager configuration)
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

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = ctx =>
                    {
                        ctx.Request.Cookies.TryGetValue(AuthenticationConstants.CookiesName.AccessToken, out var accessToken);

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            ctx.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void AddApplicationAuthorizationServices(this IServiceCollection service)
    {
        service.AddAuthorizationBuilder()
            .AddPolicy(ApplicationConstants.PolicyNames.AdminRolePolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimValues.AdminRoleClaimName);

                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);
            })
            .AddPolicy(ApplicationConstants.PolicyNames.UserRolePolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimValues.UserRoleClaimName);

                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);
            })
            .AddPolicy(ApplicationConstants.PolicyNames.FamilyAdminPolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimValues.UserRoleClaimName);
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimValues.FamilyAdminClaimName);

                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimFamilyIdType);
            })
            .AddPolicy(ApplicationConstants.PolicyNames.FamilyMemberPolicyName, p =>
            {
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimValues.UserRoleClaimName);
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType,
                    ApplicationConstants.ClaimValues.FamilyMemberClaimName);

                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimUserIdType);
                p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimFamilyIdType);
            });
    }
}