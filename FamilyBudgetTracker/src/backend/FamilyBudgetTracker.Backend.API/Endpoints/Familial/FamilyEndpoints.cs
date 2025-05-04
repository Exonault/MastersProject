using FamilyBudgetTracker.Backend.API.Constants;
using FamilyBudgetTracker.Backend.Authentication.Util;
using FamilyBudgetTracker.Backend.Domain.Constants.User;
using FamilyBudgetTracker.Backend.Domain.Invite;
using FamilyBudgetTracker.Backend.Domain.Services.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace FamilyBudgetTracker.Backend.API.Endpoints.Familial;

public static class FamilyEndpoints
{
    public static void MapFamilyEndpoints(this RouteGroupBuilder group)
    {
        var familyGroup = group.MapGroup("family");

        familyGroup.MapPost("/", CreateFamily)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyAdminPolicyName)
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
            // .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyAdminPolicyName)
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
            .Produces(StatusCodes.Status200OK, typeof(FamilyResponse), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Retrieve a family by id")
            .WithOpenApi();

        familyGroup.MapGet("/all", GetAllFamilies)
            // .RequireAuthorization(ApplicationConstants.PolicyNames.AdminRolePolicyName)
            .Produces(StatusCodes.Status200OK, typeof(List<FamilyResponse>), "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all families")
            .WithOpenApi();

        familyGroup.MapPost("/invite", InviteToFamily)
            // .RequireAuthorization()
            // .RequireAuthorization(ApplicationConstants.PolicyNames.FamilyAdminPolicyName)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Invites a list of users to a family")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateFamily([FromBody] CreateFamilyRequest request,
        IFamilyService service, HttpContext httpContext)
    {
        var userId = httpContext.GetUserIdFromAuth();

        string updatedToken = await service.CreateFamily(request, userId);

        return Results.Ok(updatedToken);
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

        FamilyResponse response = await service.GetFamilyById(id.ToString(), userId);

        return Results.Ok(response);
    }

    private static async Task<IResult> GetAllFamilies(IFamilyService service)
    {
        List<FamilyResponse> response = await service.GetAllFamilies();

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