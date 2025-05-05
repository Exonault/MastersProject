using FamilyBudgetTracker.Backend.API.Constants;
using FamilyBudgetTracker.Backend.Authentication.Interfaces;
using FamilyBudgetTracker.Backend.Domain.Invite;
using FamilyBudgetTracker.Shared.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.API.Endpoints.User;

public static class UserEndpoint
{
    public static void MapUserEndpoints(this RouteGroupBuilder group)
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
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Log a user in the system")
            .WithOpenApi();

        group.MapPost("refresh/", Refresh)
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Refresh user tokens")
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
        
        group.MapGet("joinFamily/{token:guid}", JoinFamily)
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Adds user to a family")
            .WithOpenApi();
    }

    private static async Task<IResult> Register([FromBody] RegisterRequest request,
        IUserService service)
    {
        RegisterResponse response = await service.RegisterAccount(request);

        if (!response.Successful)
        {
            return TypedResults.BadRequest(response.Errors);
        }

        return Results.Ok(response);
    }

    private static async Task<IResult> Login([FromBody] LoginRequest request,
        IUserService service, HttpContext httpContext)
    {
        await service.LoginAccount(request, httpContext);
        return Results.Ok();
    }

    private static async Task<IResult> Refresh(IUserService service, HttpContext httpContext)
    {
        httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

        await service.Refresh(refreshToken, httpContext);
        return Results.Ok();
    }

    private static async Task<IResult> Revoke(IUserService service, HttpContext context)
    {
        await service.Revoke(context);
        return Results.Ok();
    }

    private static async Task<IResult> JoinFamily([FromRoute] Guid token,
        IFamilyInvitationService service, HttpContext httpContext)
    {

        await service.AddUserToFamily(token.ToString());
        
        return Results.Ok();
    }
}