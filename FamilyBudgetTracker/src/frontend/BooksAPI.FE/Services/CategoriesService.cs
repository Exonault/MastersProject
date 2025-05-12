using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using BooksAPI.FE.Contracts.Personal.Category;
using BooksAPI.FE.Interfaces;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Services;

public class CategoriesService : ICategoriesService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IRefreshTokenService _refreshTokenService;

    public CategoriesService(IConfiguration configuration, IHttpClientFactory clientFactory, IRefreshTokenService refreshTokenService)
    {
        _configuration = configuration;
        _clientFactory = clientFactory;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<List<CategoryResponse>> GetCategories(string token, string refreshToken, string userId)
    {
        string url = $"{_configuration["Backend:Categories"]!}/user";

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
                List<CategoryResponse>? response =
                    await JsonSerializer.DeserializeAsync<List<CategoryResponse>>(responseStream);

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

    public async Task<CategoryModel> GetCategoryModel(int id, string token, string refreshToken, string userId)
    {
        CategoryResponse response = await GetCategory(id, token, refreshToken, userId);

        CategoryModel model = new CategoryModel
        {
            Name = response.Name,
            Icon = response.Icon,
            Type = response.Type,
            Limit = response.Limit
        };

        return model;
    }
    
    public async Task<CategoryResponse> GetCategory(int id, string token, string refreshToken, string userId)
    {
        string url = $"{_configuration["Backend:Category"]}/{id}";
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
                CategoryResponse? response =
                    await JsonSerializer.DeserializeAsync<CategoryResponse>(responseStream);

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

    public async Task<bool> CreateCategory(CategoryModel model, string token, string refreshToken, string userId)
    {
        CategoryRequest requestContent = new CategoryRequest
        {
            Name = model.Name,
            Icon = model.Icon,
            Type = model.Type,
            Limit = model.Limit
        };
        
        string url = $"{_configuration["Backend:Category"]}/";
        
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

    public async Task<bool> UpdateCategory(int id, CategoryModel model, string token, string refreshToken, string userId)
    {
        CategoryRequest requestContent = new CategoryRequest
        {
            Name = model.Name,
            Icon = model.Icon,
            Type = model.Type,
            Limit = model.Limit
        };
        
        string url = $"{_configuration["Backend:Category"]}/{id}";
        
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