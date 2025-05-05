using FamilyBudgetTracker.Backend.Authentication.Util;
using FamilyBudgetTracker.Backend.Domain.Services.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyCategory;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.API.Endpoints.Familial;

public static class FamilyCategoryEndpoints
{
    public static void MapFamilyCategoryEndpoints(this RouteGroupBuilder group)
    {
        var familyCategoryGroup = group.MapGroup("familyCategory");

        familyCategoryGroup.MapPost("/", CreateFamilyCategory)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a family category")
            .WithOpenApi();


        familyCategoryGroup.MapPut("/{id:int}", UpdateFamilyCategory)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a family category")
            .WithOpenApi();

        familyCategoryGroup.MapDelete("/{id:int}", DeleteFamilyCategory)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a family category")
            .WithOpenApi();


        familyCategoryGroup.MapGet("/{id:int}", GetFamilyCategoryById)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK, typeof(FamilyCategoryResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Retrieves a family category")
            .WithOpenApi();

        familyCategoryGroup.MapGet("/family", GetFamilyCategoriesByFamilyId)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK, typeof(List<FamilyCategoryResponse>), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Retrieves all family categories for family")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateFamilyCategory([FromBody] FamilyCategoryRequest request,
        IFamilyCategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        await service.CreateFamilyCategory(request, userId, familyId);

        return Results.Ok();
    }

    private static async Task<IResult> UpdateFamilyCategory([FromRoute] int id, [FromBody] FamilyCategoryRequest request,
        IFamilyCategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        await service.UpdateFamilyCategory(id, request, userId, familyId);

        return Results.Ok();
    }


    private static async Task<IResult> DeleteFamilyCategory([FromRoute] int id,
        IFamilyCategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        await service.DeleteFamilyCategory(id, userId, familyId);

        return Results.Ok();
    }

    private static async Task<IResult> GetFamilyCategoryById([FromRoute] int id,
        IFamilyCategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        FamilyCategoryResponse response = await service.GetFamilyCategoryById(id, userId, familyId);

        return Results.Ok(response);
    }

    private static async Task<IResult> GetFamilyCategoriesByFamilyId(IFamilyCategoryService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        List<FamilyCategoryResponse> response = await service.GetFamilyCategoriesByFamilyId(familyId, userId);

        return Results.Ok(response);
    }
}