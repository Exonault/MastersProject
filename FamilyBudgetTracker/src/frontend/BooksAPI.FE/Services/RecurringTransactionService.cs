using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using BooksAPI.FE.Contracts.Personal.Category;
using BooksAPI.FE.Contracts.Personal.RecurringTransaction;
using BooksAPI.FE.Interfaces;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Services;

public class RecurringTransactionService : IRecurringTransactionService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IRefreshTokenService _refreshTokenService;

    private readonly string _baseUrl;

    public RecurringTransactionService(IConfiguration configuration, IHttpClientFactory clientFactory,
        IRefreshTokenService refreshTokenService)
    {
        _clientFactory = clientFactory;
        _refreshTokenService = refreshTokenService;
        _baseUrl = configuration["Backend:RecurringTransactions"]!;
    }


    public async Task<List<RecurringTransactionResponse>> GetRecurringTransactions(string token, string refreshToken,
        string userId)
    {
        string url = $"{_baseUrl}/user";

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
                List<RecurringTransactionResponse>? response =
                    await JsonSerializer.DeserializeAsync<List<RecurringTransactionResponse>>(responseStream);

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

    public async Task<RecurringTransactionModel> GetRecurringTransactionModel(int id, string token, string refreshToken,
        string userId)
    {
        RecurringTransactionResponse response = await GetTransactionById(id, token, refreshToken, userId);

        RecurringTransactionModel model = new RecurringTransactionModel();

        return model;
    }

    public async Task<RecurringTransactionResponse> GetTransactionById(int id, string token, string refreshToken,
        string userId)
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
                RecurringTransactionResponse? response =
                    await JsonSerializer.DeserializeAsync<RecurringTransactionResponse>(responseStream);

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

    public async Task<bool> CreateTransaction(RecurringTransactionModel model, int categoryId, string token,
        string refreshToken,
        string userId)
    {
        RecurringTransactionRequest requestContent = new RecurringTransactionRequest();

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

    [Obsolete("Not implemented. not needed")]
    public async Task<bool> UpdateTransaction(int id, RecurringTransactionModel model, int categoryId, string token,
        string refreshToken,
        string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteTransaction(int id, string token, string refreshToken, string userId)
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