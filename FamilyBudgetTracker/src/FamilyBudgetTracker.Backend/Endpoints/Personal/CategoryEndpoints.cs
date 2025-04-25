using FamilyBudgetTracker.Backend.Util;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Category;
using FamilyBudgetTracker.BE.Commons.Services.Personal;
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
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a category")
            .WithOpenApi();


        categoryGroup.MapPut("/{id:int}", UpdateCategory)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Updates a category")
            .WithOpenApi();


        categoryGroup.MapDelete("/{id:int}", DeleteCategory)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Deletes a category")
            .WithOpenApi();

        categoryGroup.MapGet("/{id:int}", GetCategoryById)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK, typeof(CategoryResponse), "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Retreives a category by id")
            .WithOpenApi();

        categoryGroup.MapGet("/user", GetCategoriesByUserId)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName
            // .CacheOutput(x => x.Expire(TimeSpan.FromMinutes(5)))
            .Produces(StatusCodes.Status200OK, typeof(List<CategoryResponse>), "application/json")
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Retrieves all categories for the user")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateCategory([FromBody] CreateCategoryRequest request,
        ICategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.CreateCategory(request, userId);
        return Results.Ok();
    }

    private static async Task<IResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryRequest request,
        ICategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.UpdateCategory(id, request, userId);
        return Results.Ok();
    }

    private static async Task<IResult> DeleteCategory([FromRoute] int id,
        ICategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.DeleteCategory(id, userId);
        return Results.Ok();
    }

    private static async Task<IResult> GetCategoryById([FromRoute] int id,
        ICategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        CategoryResponse categoryResponse = await service.GetCategory(id, userId);

        return Results.Ok(categoryResponse);
    }

    private static async Task<IResult> GetCategoriesByUserId(ICategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        List<CategoryResponse> categoriesForUser = await service.GetCategoriesForUser(userId);

        return Results.Ok(categoriesForUser);
    }
}