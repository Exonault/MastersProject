using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using BooksAPI.FE.Contracts.Familial.Family;
using BooksAPI.FE.Contracts.Familial.Invite;
using BooksAPI.FE.Interfaces;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Services;

public class FamilyService : IFamilyService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IRefreshTokenService _refreshTokenService;

    private readonly string _baseUrl;
    
    public FamilyService(IConfiguration configuration, IHttpClientFactory clientFactory,
        IRefreshTokenService refreshTokenService)
    {
        _clientFactory = clientFactory;
        _refreshTokenService = refreshTokenService;
        _baseUrl = configuration["Backend:Family"]!;

    }
    
    public async Task<FamilyResponse> GetFamily(string id, string token, string refreshToken, string userId)
    {
        string url = $"{_baseUrl}/{id}";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpClient httpClient = _clientFactory.CreateClient();

        HttpResponseMessage responseMessage = await httpClient.SendAsync(request);

        if (!responseMessage.IsSuccessStatusCode)
        {
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                responseMessage = await RefreshRequest(token, refreshToken, request, httpClient);
            }
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            await using (Stream responseStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                FamilyResponse? response =
                    await JsonSerializer.DeserializeAsync<FamilyResponse>(responseStream);

                if (response is null)
                {
                    throw new Exception();
                }

                return response;
            }
        }
        else
        {
            throw new Exception();
        }
    }

    public async Task<FamilyModel> GetFamilyModel(string id, string token, string refreshToken, string userId)
    {
        FamilyResponse response = await GetFamily(id, token, refreshToken, userId);

        FamilyModel model = new FamilyModel();

        return model;
    }

    public async Task<bool> CreateFamily(string token, string refreshToken, string userId)
    {
        FamilyRequest requestContent = new FamilyRequest();
        
        string url = $"{_baseUrl}/";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(requestContent);

        HttpClient httpClient = _clientFactory.CreateClient();

        HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
        if (!responseMessage.IsSuccessStatusCode)
        {
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                responseMessage = await RefreshRequest(token, refreshToken, request, httpClient);
            }
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteFamily(string id, string token, string refreshToken, string userId)
    {
        string url = $"{_baseUrl}/{id}";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpClient httpClient = _clientFactory.CreateClient();

        HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
        if (!responseMessage.IsSuccessStatusCode)
        {
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                responseMessage = await RefreshRequest(token, refreshToken, request, httpClient);
            }
            else throw new Exception();
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> InviteMembers(List<string> emails, string token, string refreshToken, string userId)
    {
        InviteFamilyMembersRequest requestContent = new InviteFamilyMembersRequest
        {
            EmailList = emails
        };

        string url = $"{_baseUrl}/invite";
        
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(requestContent);

        HttpClient httpClient = _clientFactory.CreateClient();

        HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
        if (!responseMessage.IsSuccessStatusCode)
        {
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                responseMessage = await RefreshRequest(token, refreshToken, request, httpClient);
            }
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }
    
    
    private async Task<HttpResponseMessage> RefreshRequest(string token, string refreshToken,
        HttpRequestMessage request, HttpClient httpClient)
    {
        HttpResponseMessage responseMessage;
        bool isRefreshSuccessful = await _refreshTokenService.RefreshToken(token, refreshToken);

        if (isRefreshSuccessful)
        {
            string[] tokens = await _refreshTokenService.GetTokens();

            token = tokens[0];

            HttpRequestMessage refreshedRequest = new HttpRequestMessage(request.Method, request.RequestUri);
            refreshedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            responseMessage = await httpClient.SendAsync(refreshedRequest);
        }
        else
        {
            throw new InvalidOperationException();
        }

        return responseMessage;
    }
}