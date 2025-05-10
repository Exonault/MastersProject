using System.Net;
using System.Text.Json;
using FamilyBudgetTracker.Frontend.Interfaces;
using FamilyBudgetTracker.Frontend.Models.User;
using FamilyBudgetTracker.Shared.Contracts.User;

namespace FamilyBudgetTracker.Frontend.Services;

public class UserService : IUserService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor)
    {
        _clientFactory = clientFactory;
        _contextAccessor = contextAccessor;
    }

    public async Task<LoginResponse?> Login(LoginModel model)
    {
        LoginRequest loginRequest = new LoginRequest
        {
            Email = model.Email,
            Password = model.Password
        };

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:8081/user/login");
        request.Content = JsonContent.Create(loginRequest);
        
        HttpClient httpClient = _clientFactory.CreateClient();
        
        
        HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
        if (!responseMessage.IsSuccessStatusCode)
        {
            return null;
        }
        
        if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new ArgumentException();
        }

        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            throw new InvalidOperationException();
        }

        await using (Stream responseStream = await responseMessage.Content.ReadAsStreamAsync())
        {
            LoginResponse? loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(responseStream);
            return loginResponse;
        }
       
    }
}