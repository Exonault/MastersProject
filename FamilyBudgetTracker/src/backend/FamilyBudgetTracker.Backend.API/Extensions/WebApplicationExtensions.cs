using FamilyBudgetTracker.Backend.Authentication.Constants;
using Scalar.AspNetCore;

namespace FamilyBudgetTracker.Backend.API.Extensions;

public static class WebApplicationExtensions
{
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