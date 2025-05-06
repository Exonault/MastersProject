using FamilyBudgetTracker.Backend.API.Constants;
using FamilyBudgetTracker.Backend.Authentication.Util;
using FamilyBudgetTracker.Backend.Domain.Services.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.API.Endpoints.Personal;

public static class PersonalTransactionEndpoints
{
    public static void MapPersonalTransactionEndpoints(this RouteGroupBuilder group)
    {
        var transactionsGroup = group.MapGroup("transaction");

        transactionsGroup.MapPost("/", CreateTransaction)
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a transaction")
            .WithOpenApi();

        transactionsGroup.MapPut("/{id:int}", UpdateTransaction)
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a transaction")
            .WithOpenApi();

        transactionsGroup.MapDelete("/{id:int}", DeleteTransaction)
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a transaction")
            .WithOpenApi();

        transactionsGroup.MapGet("/{id:int}", GetTransactionById)
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK, typeof(PersonalTransactionResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get a transaction by id")
            .WithOpenApi();

        transactionsGroup.MapGet("/period", GetTransactionsForPeriod)
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .CacheOutput("PerUser")
            .Produces(StatusCodes.Status200OK, typeof(List<PersonalTransactionResponse>), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all transactions for given period")
            .WithOpenApi();

        transactionsGroup.MapGet("/period/summary", GetTransactionsForPeriodSummary)
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK, typeof(TransactionsForPeriodSummaryResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get a summary for all transactions for given period")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateTransaction([FromBody] PersonalTransactionRequest request,
        IPersonalTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.CreateTransaction(request, userId);
        return Results.Ok();
    }


    private static async Task<IResult> UpdateTransaction([FromRoute] int id, [FromBody] PersonalTransactionRequest request,
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


    private static async Task<IResult> GetTransactionsForPeriod([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate,
        IPersonalTransactionService service, HttpContext httpContext)
    {
        string userId = httpContext.GetUserIdFromAuth();

        List<PersonalTransactionResponse>
            transactions = await service.GetTransactionForPeriod(startDate, endDate, userId);

        return Results.Ok(transactions);
    }

    private static async Task<IResult> GetTransactionsForPeriodSummary([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate,
        IPersonalTransactionService service, HttpContext httpContext)
    {
        string userId = httpContext.GetUserIdFromAuth();

        var summary = await service.GetTransactionsForPeriodSummary(startDate, endDate, userId);
        
        return Results.Ok(summary);
    }
}