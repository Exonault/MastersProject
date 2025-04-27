using FamilyBudgetTracker.Backend.Endpoints.Familial;
using FamilyBudgetTracker.Backend.Endpoints.Personal;
using FamilyBudgetTracker.Backend.Endpoints.User;
using Scalar.AspNetCore;

namespace FamilyBudgetTracker.Backend.Extensions;

public static class WebApplicationExtensions
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
        var familyGroup = app.MapGroup("family");

        familyGroup.MapFamilyEndpoints();
    }


    private static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("user");

        userGroup.MapUserEndpoints();
    }

    public static void ConfigureScalar(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("FamilyBudgetTracker.Api")
                .WithLayout(ScalarLayout.Modern)
                .WithTheme(ScalarTheme.BluePlanet)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }
}