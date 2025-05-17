using FamilyBudgetTracker.Backend.API.Constants;
using FamilyBudgetTracker.Backend.Authentication.Util;
using FamilyBudgetTracker.Backend.Domain.Services.Statistics;
using FamilyBudgetTracker.Shared.Contracts.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.Endpoints.Statistics;

public static class StatisticsEndpoints
{
    public static void MapPersonalStatisticsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("yearlyStatistics/{year:int}", CreatePersonalStatistics)
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK, typeof(List<YearlyStatisticsResponse>), "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyMemberPolicyName)
            .WithSummary("Generates a statistic about the user for a given year")
            .WithOpenApi();
    }

    public static void MapFamilialStatisticsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("yearlyStatistics/{year:int}", CreateFamilialStatistics)
            .Produces(StatusCodes.Status200OK, typeof(List<FamilyYearlyStatisticsResponse>), "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyMemberPolicyName)
            .WithSummary("Generates a statistic about the family for a given year")
            .WithOpenApi();
    }


    private static async Task<IResult> CreatePersonalStatistics([FromRoute] int year,
        IStatisticsService service, HttpContext httpContext)
    {
        string userId = httpContext.GetUserIdFromAuth();

        var response = await service.GetYearlyStatistics(year, userId);

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateFamilialStatistics([FromRoute] int year,
        IStatisticsService service, HttpContext httpContext)
    {
        string familyId = httpContext.GetFamilyIdFromAuth();
        string userId = httpContext.GetUserIdFromAuth();

        var response = await service.GetFamilyYearlyStatistics(year, familyId, userId);

        return Results.Ok(response);
    }
}