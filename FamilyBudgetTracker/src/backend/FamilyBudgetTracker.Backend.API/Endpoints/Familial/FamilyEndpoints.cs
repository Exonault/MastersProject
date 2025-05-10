using FamilyBudgetTracker.Backend.API.Constants;
using FamilyBudgetTracker.Backend.Authentication.Interfaces;
using FamilyBudgetTracker.Backend.Authentication.Token;
using FamilyBudgetTracker.Backend.Authentication.Util;
using FamilyBudgetTracker.Backend.Domain.Invite;
using FamilyBudgetTracker.Backend.Domain.Services.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;
using FamilyBudgetTracker.Shared.Contracts.Familial.Invite;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.API.Endpoints.Familial;

public static class FamilyEndpoints
{
    public static void MapFamilyEndpoints(this RouteGroupBuilder group)
    {
        var familyGroup = group.MapGroup("family");

        familyGroup.MapPost("/", CreateFamilyBearer) // Temporary because FE
            .RequireAuthorization(ApplicationConstants.PolicyNames.UserRolePolicyName)
            .AddFluentValidationAutoValidation()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a family")
            .WithOpenApi();

        familyGroup.MapDelete("/{id:guid}", DeleteFamily)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyAdminPolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a family")
            .WithOpenApi();

        familyGroup.MapGet("{id:guid}", GetFamilyById)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyMemberPolicyName)
            .Produces(StatusCodes.Status200OK, typeof(FamilyDetailedResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Retrieve a family by id")
            .WithOpenApi();

        familyGroup.MapGet("/all", GetAllFamilies)
            .RequireAuthorization(ApplicationConstants.PolicyNames.AdminRolePolicyName)
            .Produces(StatusCodes.Status200OK, typeof(List<FamilyDetailedResponse>), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all families")
            .WithOpenApi();

        familyGroup.MapPost("/invite", InviteToFamily)
            .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyAdminPolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Invites a list of users to a family")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateFamily([FromBody] FamilyRequest request,
        IFamilyService service, HttpContext httpContext, IUserService userService)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.CreateFamily(request, userId);
        await userService.UpdateUserAccessToken(httpContext);

        return Results.Ok();
    }
    
    private static async Task<IResult> CreateFamilyBearer([FromBody] FamilyRequest request,
        IFamilyService service, HttpContext httpContext, IUserService userService, IGenerateTokenService tokenService)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.CreateFamily(request, userId);
        string newAccessToken = await tokenService.GenerateAccessToken(userId);
        // await userService.UpdateUserAccessToken(httpContext);

        return Results.Ok(new 
        {
            NewAccessToken = newAccessToken
        });
    }

    private static async Task<IResult> DeleteFamily([FromRoute] Guid id,
        IFamilyService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        await service.DeleteFamily(id.ToString(), userId);

        return Results.Ok();
    }

    private static async Task<IResult> GetFamilyById([FromRoute] Guid id,
        IFamilyService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        FamilyDetailedResponse detailedResponse = await service.GetFamilyById(id.ToString(), userId);

        return Results.Ok(detailedResponse);
    }

    private static async Task<IResult> GetAllFamilies(IFamilyService service)
    {
        List<FamilyDetailedResponse> response = await service.GetAllFamilies();

        return Results.Ok(response);
    }

    private static async Task<IResult> InviteToFamily([FromBody] InviteFamilyMembersRequest request,
        IFamilyInvitationService service, HttpContext httpContext)
    {
        string familyId = httpContext.GetFamilyIdFromAuth();

        await service.InviteMembersToFamily(request, familyId);

        return Results.Ok();
    }
}