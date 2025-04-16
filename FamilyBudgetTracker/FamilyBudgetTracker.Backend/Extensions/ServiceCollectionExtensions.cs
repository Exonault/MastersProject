using FamilyBudgetTracker.Backend.Repositories.Personal;
using FamilyBudgetTracker.Backend.Services.Personal;
using FamilyBudgetTracker.Entities.Repositories.Personal;
using FamilyBudgetTracker.Entities.Services.Personal;

namespace FamilyBudgetTracker.Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCategoryServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
    }
}