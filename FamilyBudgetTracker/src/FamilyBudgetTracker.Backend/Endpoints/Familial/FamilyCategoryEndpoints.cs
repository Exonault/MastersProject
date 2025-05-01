using FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;
using FamilyBudgetTracker.BE.Commons.Services.Familial;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.Endpoints.Familial;

public static class FamilyCategoryEndpoints
{
    public static void MapFamilyCategoryEndpoints(this RouteGroupBuilder group)
    {
        var familyCategoryGroup = group.MapGroup("familyCategory");
    }

    private static async Task<IResult> CreateFamilyCategory([FromBody] CreateFamilyCategoryRequest request,
        IFamilyCategoryService service, HttpContext httpContext)
    {
        return Results.Ok();
    }

    private static async Task<IResult> UpdateFamilyCategory([FromRoute] int id, [FromBody] UpdateFamilyCategoryRequest request,
        IFamilyCategoryService service, HttpContext httpContext)
    {
        return Results.Ok();
    }


    private static async Task<IResult> DeleteFamilyCategory([FromRoute] int id,
        IFamilyCategoryService service, HttpContext httpContext)
    {
        return Results.Ok();
    }

    private static async Task<IResult> GetFamilyCategoryById([FromRoute] int id,
        IFamilyCategoryService service, HttpContext httpContext)
    {
        return Results.Ok();
    }

    private static async Task<IResult> GetFamilyCategoriesByFamilyId(IFamilyCategoryService service, HttpContext httpContext)
    {
        return Results.Ok();
    }
}