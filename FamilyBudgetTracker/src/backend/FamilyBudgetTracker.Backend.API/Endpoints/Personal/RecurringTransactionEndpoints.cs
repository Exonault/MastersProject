using FamilyBudgetTracker.Backend.Authentication.Util;
using FamilyBudgetTracker.Backend.Domain.Services.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.RecurringTransaction;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.API.Endpoints.Personal;

public static class RecurringTransactionEndpoints
{
    public static void MapRecurringTransactionEndpoints(this RouteGroupBuilder group)
    {
        RouteGroupBuilder transactionGroup = group.MapGroup("recurringTransaction");

        transactionGroup.MapPost("/", CreateTransaction)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a recurring transaction")
            .WithOpenApi();

        transactionGroup.MapDelete("/{id:int}", DeleteTransaction)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a recurring transaction")
            .WithOpenApi();

        transactionGroup.MapGet("/{id:int}", GetTransactionById)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK, typeof(RecurringTransactionResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get a recurring transaction by id")
            .WithOpenApi();

        transactionGroup.MapGet("/user", GetTransactionsByUserId)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK, typeof(List<RecurringTransactionResponse>), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all recurring transactions for a user")
            .WithOpenApi();
        
        
        // transactionGroup.MapPut("/{id:int}", UpdateTransaction)
        //     // .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
        //     .AddFluentValidationAutoValidation()
        //     .Produces(StatusCodes.Status200OK)
        //     .Produces(StatusCodes.Status400BadRequest)
        //     .Produces(StatusCodes.Status401Unauthorized)
        //     .Produces(StatusCodes.Status403Forbidden)
        //     .Produces(StatusCodes.Status404NotFound)
        //     .Produces(StatusCodes.Status500InternalServerError)
        //     .WithSummary("Update a recurring transaction")
        //     .WithOpenApi();
    }

    private static async Task<IResult> CreateTransaction([FromBody] RecurringTransactionRequest request,
        IRecurringTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.CreateRecurringTransaction(request, userId);
        return Results.Ok();
    }

    //TODO manage execution date when updated
    private static async Task<IResult> UpdateTransaction([FromRoute] int id,
        [FromBody] RecurringTransactionRequest request,
        IRecurringTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.UpdateRecurringTransaction(id, request, userId);

        return Results.Ok();
    }

    private static async Task<IResult> DeleteTransaction([FromRoute] int id,
        IRecurringTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.DeleteRecurringTransaction(id, userId);
        return Results.Ok();
    }

    private static async Task<IResult> GetTransactionById([FromRoute] int id,
        IRecurringTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        RecurringTransactionResponse transaction = await service.GetRecurringTransactionById(id, userId);

        return Results.Ok(transaction);
    }

    private static async Task<IResult> GetTransactionsByUserId(
        IRecurringTransactionService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        List<RecurringTransactionResponse> transactions = await service.GetRecurringTransactionsForUser(userId);
        return Results.Ok(transactions);
    }
}