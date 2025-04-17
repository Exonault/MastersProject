using FamilyBudgetTracker.Backend.Repositories;
using FamilyBudgetTracker.Backend.Repositories.Personal;
using FamilyBudgetTracker.Backend.Services;
using FamilyBudgetTracker.Backend.Services.Personal;
using FamilyBudgetTracker.Backend.Validation.Personal;
using FamilyBudgetTracker.Entities.Entities.Personal;
using FamilyBudgetTracker.Entities.Repositories;
using FamilyBudgetTracker.Entities.Repositories.Personal;
using FamilyBudgetTracker.Entities.Services;
using FamilyBudgetTracker.Entities.Services.Personal;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddCategoryServices();
        services.AddUserServices();
    }

    private static void AddCategoryServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IValidator<Category>, CategoryValidator>();
    }

    private static void AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
    }
}