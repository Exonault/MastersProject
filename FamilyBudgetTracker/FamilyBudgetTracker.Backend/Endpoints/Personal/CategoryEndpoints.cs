using System.Security.Claims;
using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Entities.Contracts.Personal.Category;
using FamilyBudgetTracker.Entities.Services.Personal;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.Endpoints.Personal;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this RouteGroupBuilder group)
    {
        var categoryGroup = group.MapGroup("category");

        categoryGroup.MapPost("/", CreateCategory)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            // .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a category entry")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateCategory([FromBody] CreateCategoryRequest request,
        ICategoryService service, HttpContext httpContext)
    {
        string? userId = GetUserIdFromAuth(httpContext);

        await service.CreateCategory(request, userId);
        return Results.Ok();
    }


    private static string? GetUserIdFromAuth(HttpContext httpContext)
    {
        ClaimsPrincipal user = httpContext.User;
        string? userIdFromAuth = null;
        foreach (Claim userClaim in user.Claims)
        {
            if (userClaim.Type == ApplicationConstants.ClaimTypes.ClaimUserIdType)
            {
                userIdFromAuth = userClaim.Value;
                break;
            }
        }

        return userIdFromAuth;
    }
}