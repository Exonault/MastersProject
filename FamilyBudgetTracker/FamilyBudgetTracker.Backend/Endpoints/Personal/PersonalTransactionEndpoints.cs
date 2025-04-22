using FamilyBudgetTracker.Backend.Util;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Transaction;
using FamilyBudgetTracker.BE.Commons.Services.Personal;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.Endpoints.Personal;

public static class PersonalTransactionEndpoints
{
    public static void MapPersonalTransactionEndpoints(this RouteGroupBuilder group)
    {
        var transactionsGroup = group.MapGroup("transaction");

        transactionsGroup.MapPost("/", CreateTransaction)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a transaction")
            .WithOpenApi();

        transactionsGroup.MapPut("/{id:int}", UpdateTransaction)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a transaction")
            .WithOpenApi();

        transactionsGroup.MapDelete("/{id:int}", DeleteTransaction)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a transaction")
            .WithOpenApi();

        transactionsGroup.MapGet("/{id:int}", GetTransactionById)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK, typeof(PersonalTransactionResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get a transaction by id")
            .WithOpenApi();

        transactionsGroup.MapGet("/period", GetTransactionsForPeriod)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK, typeof(List<PersonalTransactionResponse>), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get a transactions for given period")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateTransaction([FromBody] CreatePersonalTransactionRequest request,
        IPersonalTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.CreateTransaction(request, userId);
        return Results.Ok();
    }


    private static async Task<IResult> UpdateTransaction([FromRoute] int id,
        [FromBody] UpdatePersonalTransactionRequest request,
        IPersonalTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.UpdateTransaction(id, request, userId);
        return Results.Ok();
    }

    private static async Task<IResult> DeleteTransaction([FromRoute] int id,
        IPersonalTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.DeleteTransaction(id, userId);
        return Results.Ok();
    }

    private static async Task<IResult> GetTransactionById([FromRoute] int id,
        IPersonalTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        PersonalTransactionResponse response = await service.GetTransactionById(id, userId);

        return Results.Ok(response);
    }


    private static async Task<IResult> GetTransactionsForPeriod([FromBody] PersonalTransactionsForPeriodRequest request,
        IPersonalTransactionService service, HttpContext httpContext)
    {
        string userId = httpContext.GetUserIdFromAuth();

        List<PersonalTransactionResponse>
            transactions = await service.GetTransactionForPeriod(request, userId);

        return Results.Ok(transactions);
    }
}