namespace FamilyBudgetTracker.BE.Commons.Services.Auth;

public interface IAuthenticationService
{
    void SetTokensInsideCookie(string accessToken, string refreshToken);
}