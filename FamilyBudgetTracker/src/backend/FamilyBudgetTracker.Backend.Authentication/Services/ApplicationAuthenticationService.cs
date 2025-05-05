using FamilyBudgetTracker.Backend.Authentication.Constants;
using FamilyBudgetTracker.Backend.Authentication.Interfaces;
using FamilyBudgetTracker.Backend.Data.DTO.User;
using Microsoft.AspNetCore.Http;

namespace FamilyBudgetTracker.Backend.Authentication.Services;

public class ApplicationAuthenticationService : IApplicationAuthenticationService
{
    public ApplicationAuthenticationService()
    {
    }

    public void SetTokensInsideCookie(TokenDto request, HttpContext httpContext)
    {
        httpContext.Response.Cookies.Append(AuthenticationConstants.CookiesName.AccessToken, request.AccessToken, new CookieOptions()
        {
            // Expires = DateTimeOffset.UtcNow.AddMinutes(5),
            Expires = DateTimeOffset.UtcNow.AddHours(8),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        httpContext.Response.Cookies.Append(AuthenticationConstants.CookiesName.RefreshToken, request.RefreshToken, new CookieOptions()
        {
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
    }

    public void RemoveTokensInsideCookie(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete(AuthenticationConstants.CookiesName.AccessToken);
        httpContext.Response.Cookies.Delete(AuthenticationConstants.CookiesName.RefreshToken);
    }
}