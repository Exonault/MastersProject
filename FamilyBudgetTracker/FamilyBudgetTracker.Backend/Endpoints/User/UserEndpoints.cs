using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Entities.Contracts.User;
using FamilyBudgetTracker.Entities.Services;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.Endpoints.User;

public static class UserEndpoint
{
    public static void MapUserEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("register/", Register)
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK, typeof(RegisterResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Register a user")
            .WithOpenApi();

        group.MapPost("login/", Login)
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK, typeof(LoginResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Log a user in the system")
            .WithOpenApi();

        group.MapPost("refresh/", Refresh)
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK, typeof(LoginResponse), "application/json")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Refresh user tokens")
            .WithOpenApi();

        group.MapDelete("revoke/", Revoke)
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Revoke all tokens")
            .WithOpenApi();
    }

    private static async Task<IResult> Register([FromBody] RegisterRequest request, IUserService service)
    {
        RegisterResponse response = await service.RegisterAccount(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> Login([FromBody] LoginRequest request, IUserService service)
    {
        LoginResponse response = await service.LoginAccount(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> Refresh([FromBody] RefreshRequest request, IUserService service)
    {
        LoginResponse response = await service.Refresh(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> Revoke(IUserService service)
    {
        await service.Revoke();
        return Results.Ok();
    }
}