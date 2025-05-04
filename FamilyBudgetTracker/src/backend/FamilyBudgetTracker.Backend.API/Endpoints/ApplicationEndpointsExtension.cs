using FamilyBudgetTracker.Backend.API.Endpoints.Familial;
using FamilyBudgetTracker.Backend.API.Endpoints.Personal;
using FamilyBudgetTracker.Backend.API.Endpoints.User;

namespace FamilyBudgetTracker.Backend.API.Endpoints;

public static class ApplicationEndpointsExtension
{
    public static void MapApplicationEndpoints(this WebApplication app)
    {
        app.MapUserEndpoints();
        app.MapPersonalEndpoints();
        app.MapFamilialEndpoints();
    }

    private static void MapPersonalEndpoints(this WebApplication app)
    {
        var personalGroup = app.MapGroup("personal");

        personalGroup.MapCategoryEndpoints();
        personalGroup.MapPersonalTransactionEndpoints();
        personalGroup.MapRecurringTransactionEndpoints();
    }

    private static void MapFamilialEndpoints(this WebApplication app)
    {
        var familyGroup = app.MapGroup("familial");

        familyGroup.MapFamilyEndpoints();
        familyGroup.MapFamilyCategoryEndpoints();
    }
    
    private static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("user");

        userGroup.MapUserEndpoints();
    }
}