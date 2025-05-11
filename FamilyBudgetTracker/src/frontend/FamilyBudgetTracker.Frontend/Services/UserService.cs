using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using FamilyBudgetTracker.Frontend.Contracts.User;
using FamilyBudgetTracker.Frontend.Interfaces;
using FamilyBudgetTracker.Frontend.Models.User;

namespace FamilyBudgetTracker.Frontend.Services;

public class UserService : IUserService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;
    private readonly string _backendUrl;

    public UserService(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _configuration = configuration;
        _backendUrl = configuration["Backend:User"]!;
    }

    public async Task<RegisterResponse?> Register(RegisterModel model, bool isAdmin)
    {
        RegisterRequest requestContent = new RegisterRequest
        {
            UserName = model.Username,
            Email = model.Email,
            Password = model.Password,
            Admin = isAdmin
        };

        string uri = string.Format(_backendUrl, "register");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Content = JsonContent.Create(requestContent);

        HttpClient httpClient = _clientFactory.CreateClient();

        HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
        if (!responseMessage.IsSuccessStatusCode)
        {
            return null;
        }

        await using (Stream responseStream = await responseMessage.Content.ReadAsStreamAsync())
        {
            RegisterResponse? registerResponse =
                await JsonSerializer.DeserializeAsync<RegisterResponse>(responseStream);

            return registerResponse;
        }
    }

    public async Task<LoginResponse?> Login(LoginModel model)
    {
        LoginRequest loginRequest = new LoginRequest
        {
            Email = model.Email,
            Password = model.Password
        };


        string uri = string.Format(_backendUrl, "login");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
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

    public async Task Logout(string token)
    {
        HttpClient httpClient = _clientFactory.CreateClient();

        string uri = string.Format(_backendUrl, "revoke");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, uri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
        }
        catch (Exception e)
        {
            // return null;
        }
    }
}