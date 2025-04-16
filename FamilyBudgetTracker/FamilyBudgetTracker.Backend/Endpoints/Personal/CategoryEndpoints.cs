using FamilyBudgetTracker.Entities.Contracts.Personal.Category;
using FamilyBudgetTracker.Entities.Services.Personal;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.Endpoints.Personal;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this RouteGroupBuilder group)
    {
        var categoryGroup = group.MapGroup("category");

        categoryGroup.MapPost("/", CreateCategory);
    }

    private static async Task<IResult> CreateCategory([FromBody] CreateCategoryRequest request,
        ICategoryService service)
    {
        return Results.Ok(); // TODO
    }
}