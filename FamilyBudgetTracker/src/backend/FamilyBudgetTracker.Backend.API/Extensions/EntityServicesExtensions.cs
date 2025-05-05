using FamilyBudgetTracker.Backend.Authentication.Interfaces;
using FamilyBudgetTracker.Backend.Authentication.Services;
using FamilyBudgetTracker.Backend.Authentication.Token;
using FamilyBudgetTracker.Backend.Data.Repositories;
using FamilyBudgetTracker.Backend.Data.Repositories.Familial;
using FamilyBudgetTracker.Backend.Data.Repositories.Personal;
using FamilyBudgetTracker.Backend.Domain.Invite;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using FamilyBudgetTracker.Backend.Domain.Repositories.Personal;
using FamilyBudgetTracker.Backend.Domain.Services;
using FamilyBudgetTracker.Backend.Domain.Services.Familial;
using FamilyBudgetTracker.Backend.Domain.Services.Personal;
using FamilyBudgetTracker.Backend.DomainServices.DomainServices.Familial;
using FamilyBudgetTracker.Backend.DomainServices.DomainServices.Personal;
using FamilyBudgetTracker.Backend.DomainServices.FamilyInvitation;

namespace FamilyBudgetTracker.Backend.API.Extensions;

public static class EntityServicesExtensions
{
    public static void AddEntityServices(this IServiceCollection services)
    {
        //User services
        services.AddUserServices();

        //Personal services
        services.AddCategoryServices();
        services.AddPersonalTransactionServices();
        services.AddRecurringTransactionServices();

        //Family services
        services.AddFamilyServices();
        services.AddFamilyCategoryServices();
        services.AddFamilyTransactionServices();
    }

    private static void AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IGenerateTokenService, GenerateTokenService>();
        services.AddScoped<IApplicationAuthenticationService, ApplicationAuthenticationService>();
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

    private static void AddRecurringTransactionServices(this IServiceCollection services)
    {
        services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
        services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();
    }

    private static void AddFamilyServices(this IServiceCollection services)
    {
        services.AddScoped<IFamilyRepository, FamilyRepository>();
        services.AddScoped<IFamilyService, FamilyService>();

        services.AddScoped<IFamilyInvitationTokenRepository, FamilyInvitationTokenRepository>();
        services.AddScoped<IFamilyInvitationLinkFactory, FamilyInvitationLinkFactory>();
        services.AddScoped<IFamilyInvitationService, FamilyInvitationService>();
    }

    private static void AddFamilyCategoryServices(this IServiceCollection services)
    {
        services.AddScoped<IFamilyCategoryRepository, FamilyCategoryRepository>();
        services.AddScoped<IFamilyCategoryService, FamilyCategoryService>();
    }

    private static void AddFamilyTransactionServices(this IServiceCollection services)
    {
        services.AddScoped<IFamilyTransactionRepository, FamilyTransactionRepository>();
        services.AddScoped<IFamilyTransactionService, FamilyTransactionService>();
    }
}