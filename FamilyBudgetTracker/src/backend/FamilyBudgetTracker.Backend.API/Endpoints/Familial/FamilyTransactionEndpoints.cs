using FamilyBudgetTracker.Backend.API.Constants;
using FamilyBudgetTracker.Backend.Authentication.Util;
using FamilyBudgetTracker.Backend.Domain.Services.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyTransaction;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.API.Endpoints.Familial;

public static class FamilyTransactionEndpoints
{
    public static void MapFamilyTransactionEndpoints(this RouteGroupBuilder group)
    {
        var familyTransactionGroup = group.MapGroup("familyTransaction");

        familyTransactionGroup.MapPost("/", CreateFamilyTransaction)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyMemberPolicyName)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a family transaction")
            .WithOpenApi();

        familyTransactionGroup.MapPut("/{id:int}", UpdateFamilyTransaction)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyMemberPolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a family transaction")
            .WithOpenApi();

        familyTransactionGroup.MapDelete("/{id:int}", DeleteFamilyTransaction)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyMemberPolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a family transaction")
            .WithOpenApi();

        familyTransactionGroup.MapGet("/period", GetFamilyTransactionsForPeriod)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyMemberPolicyName)
            .Produces(StatusCodes.Status200OK, typeof(List<FamilyTransactionRequest>), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all transactions for given period")
            .WithOpenApi();

        familyTransactionGroup.MapGet("/period/summary", GetFamilyTransactionsForPeriod)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyMemberPolicyName)
            .Produces(StatusCodes.Status200OK, typeof(FamilyTransactionsForPeriodSummaryResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get a summary for all transactions for given period")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateFamilyTransaction([FromBody] FamilyTransactionRequest request,
        IFamilyTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        await service.CreateFamilyTransaction(request, userId, familyId);

        return Results.Ok();
    }

    private static async Task<IResult> UpdateFamilyTransaction([FromRoute] int id, [FromBody] FamilyTransactionRequest request,
        IFamilyTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        await service.UpdateFamilyTransaction(id, request, userId, familyId);

        return Results.Ok();
    }

    private static async Task<IResult> DeleteFamilyTransaction([FromRoute] int id,
        IFamilyTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        await service.DeleteFamilyTransaction(id, userId, familyId);

        return Results.Ok();
    }

    private static async Task<IResult> GetFamilyTransactionById([FromRoute] int id,
        IFamilyTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        FamilyTransactionResponse response = await service.GetFamilyTransactionById(id, userId, familyId);

        return Results.Ok(response);
    }

    private static async Task<IResult> GetFamilyTransactionsForPeriod([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate,
        IFamilyTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        List<FamilyTransactionResponse> response = await service.GetFamilyTransactionsForPeriod(startDate, endDate, userId, familyId);

        return Results.Ok(response);
    }

    private static async Task<IResult> GetFamilyTransactionsForPeriodSummary([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate,
        IFamilyTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();
        var familyId = httpContext.GetFamilyIdFromAuth();

        FamilyTransactionsForPeriodSummaryResponse response =
            await service.GetFamilyTransactionsForPeriodSummary(startDate, endDate, userId, familyId);

        return Results.Ok(response);
    }
}