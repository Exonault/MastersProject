using FamilyBudgetTracker.Backend.API.Constants;
using FamilyBudgetTracker.Backend.Authentication.Interfaces;
using FamilyBudgetTracker.Shared.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.API.Endpoints.User;

public static class BearerUserEndpoints
{
    public static void MapBearerUserGroups(this RouteGroupBuilder group)
    {
        group.MapPost("register/", Register)
            .AllowAnonymous()
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK, typeof(RegisterResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Register a user")
            .WithOpenApi();

        group.MapPost("login/", Login)
            .AllowAnonymous()
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK, typeof(LoginResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Log in for a user")
            .WithOpenApi();
        
        group.MapPost("refresh/", Refresh)
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK, typeof(LoginResponse), "application/json")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Refresh user access token")
            .WithOpenApi();

        group.MapDelete("revoke/", Revoke)
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Revoke all tokens")
            .WithOpenApi();
    }

    private static async Task<IResult> Register([FromBody] RegisterRequest request,
        IBearerUserService service)
    {
        RegisterResponse response = await service.RegisterAccount(request);

        if (!response.Successful)
        {
            Results.BadRequest(response.Errors);
        }

        return Results.Ok(response);
    }

    private static async Task<IResult> Login([FromBody] LoginRequest request,
        IBearerUserService service)
    {
        LoginResponse response = await service.LoginAccount(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> Refresh([FromBody] RefreshRequest request, IBearerUserService service)
    {
        LoginResponse response = await service.Refresh(request);
        return Results.Ok(response);
    }

    private static async Task<IResult> Revoke(IBearerUserService service)
    {
        await service.Revoke();
        return Results.Ok();
    }
}