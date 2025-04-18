using FamilyBudgetTracker.Backend.Endpoints.Personal;
using FamilyBudgetTracker.Backend.Endpoints.User;

namespace FamilyBudgetTracker.Backend.Extensions;

public static class WebApplicationExtensions
{
    public static void MapApplicationEndpoints(this WebApplication app)
    {
        app.MapUserEndpoints();
        app.MapPersonalEndpoints();
    }

    private static void MapPersonalEndpoints(this WebApplication app)
    {
        var personalGroup = app.MapGroup("personal");

        personalGroup.MapCategoryEndpoints();
    }

    private static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("user");

        userGroup.MapUserEndpoints();
    }
}