using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using BooksAPI.FE.Contracts.Familial.FamilyCategory;
using BooksAPI.FE.Interfaces;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Services;

public class FamilyCategoryService : IFamilyCategoryService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IRefreshTokenService _refreshTokenService;

    private readonly string _baseUrl;

    public FamilyCategoryService(IConfiguration configuration, IHttpClientFactory clientFactory,
        IRefreshTokenService refreshTokenService)
    {
        _clientFactory = clientFactory;
        _refreshTokenService = refreshTokenService;
        _baseUrl = configuration["Backend:FamilyCategory"]!;
    }

    public async Task<List<FamilyCategoryResponse>> GetFamilyCategories(string token, string refreshToken,
        string userId,
        string familyId)
    {
        string url = $"{_baseUrl}/family";

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
                List<FamilyCategoryResponse>? response =
                    await JsonSerializer.DeserializeAsync<List<FamilyCategoryResponse>>(responseStream);

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

    public async Task<FamilyCategoryModel> GetCategoryModel(int id, string token, string refreshToken, string userId,
        string familyId)
    {
        FamilyCategoryResponse response = await GetCategory(id, token, refreshToken, userId, familyId);

        FamilyCategoryModel model = new FamilyCategoryModel();

        return model;
    }

    public async Task<FamilyCategoryResponse> GetCategory(int id, string token, string refreshToken, string userId,
        string familyId)
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
                FamilyCategoryResponse? response =
                    await JsonSerializer.DeserializeAsync<FamilyCategoryResponse>(responseStream);

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

    public async Task<bool> CreateCategory(FamilyCategoryModel model, string token, string refreshToken, string userId,
        string familyId)
    {
        FamilyCategoryRequest requestContent = new FamilyCategoryRequest();

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

    public async Task<bool> UpdateCategory(int id, FamilyCategoryModel model, string token, string refreshToken,
        string userId,
        string familyId)
    {
        FamilyCategoryRequest requestContent = new FamilyCategoryRequest();

        string url = $"{_baseUrl}/{id}";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);

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

    public async Task<bool> DeleteCategory(int id, string token, string refreshToken, string userId, string familyId)
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