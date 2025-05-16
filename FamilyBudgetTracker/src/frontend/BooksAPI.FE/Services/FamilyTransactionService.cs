using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using BooksAPI.FE.Contracts.Familial.FamilyTransaction;
using BooksAPI.FE.Contracts.Personal.Transaction;
using BooksAPI.FE.Interfaces;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Services;

public class FamilyTransactionService : IFamilyTransactionService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IRefreshTokenService _refreshTokenService;

    private readonly string _baseUrl;

    public FamilyTransactionService(IConfiguration configuration, IHttpClientFactory clientFactory,
        IRefreshTokenService refreshTokenService)
    {
        _clientFactory = clientFactory;
        _refreshTokenService = refreshTokenService;
        _baseUrl = configuration["Backend:FamilyTransaction"]!;
    }

    public async Task<FamilyTransactionPeriodSummaryResponse> GetSummary(DateTime startOfMonth, string token,
        string refreshToken, string userId, string familyId)
    {
        var monthRange = GetMonthRange(startOfMonth);
        string startDate = monthRange.Start.ToString("yyyy-M-d", CultureInfo.InvariantCulture);
        string endDate = monthRange.End.ToString("yyyy-M-d", CultureInfo.InvariantCulture);


        string url = $"{_baseUrl}/period/summary?startDate={startDate}&endDate={endDate}";

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
                FamilyTransactionPeriodSummaryResponse? response =
                    await JsonSerializer.DeserializeAsync<FamilyTransactionPeriodSummaryResponse>(responseStream);

                if (response is null)
                {
                    throw new Exception();
                }

                return response;
            }
        }
        else
        {
            return new FamilyTransactionPeriodSummaryResponse();
        }
    }

    public async Task<List<FamilyTransactionResponse>> GetTransactionsForPeriod(DateTime startOfMonth, string token,
        string refreshToken, string userId, string familyId)
    {
        var monthRange = GetMonthRange(startOfMonth);
        string startDate = monthRange.Start.ToString("yyyy-M-d", CultureInfo.InvariantCulture);
        string endDate = monthRange.End.ToString("yyyy-M-d", CultureInfo.InvariantCulture);


        string url = $"{_baseUrl}/period/?startDate={startDate}&endDate={endDate}";

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
                List<FamilyTransactionResponse>? response =
                    await JsonSerializer.DeserializeAsync<List<FamilyTransactionResponse>>(responseStream);

                if (response is null)
                {
                    throw new Exception();
                }

                return response;
            }
        }
        else
        {
            return new List<FamilyTransactionResponse>();
        }
    }

    public async Task<FamilyTransactionModel> GetTransactionModel(int id, string token, string refreshToken,
        string userId,
        string familyId)
    {
        FamilyTransactionResponse response = await GetTransactionById(id, token, refreshToken, userId, familyId);

        FamilyTransactionModel model = new FamilyTransactionModel();

        return model;
    }

    public async Task<FamilyTransactionResponse> GetTransactionById(int id, string token, string refreshToken,
        string userId,
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
                FamilyTransactionResponse? response =
                    await JsonSerializer.DeserializeAsync<FamilyTransactionResponse>(responseStream);

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

    public async Task<bool> CreateTransaction(FamilyTransactionModel model, int categoryId, string token,
        string refreshToken,
        string userId,
        string familyId)
    {
        FamilyTransactionRequest requestContent = new FamilyTransactionRequest();

        string url = $"{_baseUrl}/";

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

    public async Task<bool> UpdateTransaction(int id, FamilyTransactionModel model, int categoryId, string token,
        string refreshToken,
        string userId, string familyId)
    {
        FamilyTransactionRequest requestContent = new FamilyTransactionRequest();

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

    public async Task<bool> DeleteTransaction(int id, string token, string refreshToken, string userId, string familyId)
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

    private static (DateOnly Start, DateOnly End) GetMonthRange(DateTime date)
    {
        var start = new DateOnly(date.Year, date.Month, 1);
        var end = new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        return (start, end);
    }
}