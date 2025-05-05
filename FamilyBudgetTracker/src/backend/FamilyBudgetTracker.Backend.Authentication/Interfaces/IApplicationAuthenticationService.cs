using FamilyBudgetTracker.Backend.Data.DTO.User;
using Microsoft.AspNetCore.Http;

namespace FamilyBudgetTracker.Backend.Authentication.Interfaces;

public interface IApplicationAuthenticationService
{
    void SetTokensInsideCookie(TokenDto request, HttpContext httpContext);

    void RemoveTokensInsideCookie(HttpContext httpContext);
}