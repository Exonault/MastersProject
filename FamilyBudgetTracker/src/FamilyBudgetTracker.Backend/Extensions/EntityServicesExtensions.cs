using FamilyBudgetTracker.Backend.Repositories;
using FamilyBudgetTracker.Backend.Repositories.Familial;
using FamilyBudgetTracker.Backend.Repositories.Personal;
using FamilyBudgetTracker.Backend.Services;
using FamilyBudgetTracker.Backend.Services.Familial;
using FamilyBudgetTracker.Backend.Services.Personal;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Familial;
using FamilyBudgetTracker.BE.Commons.Repositories.Personal;
using FamilyBudgetTracker.BE.Commons.Services;
using FamilyBudgetTracker.BE.Commons.Services.Familial;
using FamilyBudgetTracker.BE.Commons.Services.Personal;

namespace FamilyBudgetTracker.Backend.Extensions;

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
        //TODO
        services.AddFamilyServices();
        services.AddFamilyCategoryServices();
        services.AddFamilyTransactionServices();
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

    private static void AddRecurringTransactionServices(this IServiceCollection services)
    {
        services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
        services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();
    }

    private static void AddFamilyServices(this IServiceCollection services)
    {
        services.AddScoped<IFamilyRepository, FamilyRepository>();
        services.AddScoped<IFamilyService, FamilyService>();
    }

    private static void AddFamilyCategoryServices(this IServiceCollection services)
    {
        services.AddScoped<IFamilyCategoryRepository, FamilyCategoryRepository>();
        services.AddScoped<IFamilyCategoryService, FamilyCategoryService>();
    }

    private static void AddFamilyTransactionServices(this IServiceCollection services)
    {
    }
}