using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using BooksAPI.FE.Contracts.Personal.Transaction;
using BooksAPI.FE.Interfaces;

namespace BooksAPI.FE.Services;

public class PersonalTransactionService : IPersonalTransactionService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IRefreshTokenService _refreshTokenService;

    private readonly string _baseUrl;

    public PersonalTransactionService(IConfiguration configuration, IHttpClientFactory clientFactory,
        IRefreshTokenService refreshTokenService)
    {
        _clientFactory = clientFactory;
        _refreshTokenService = refreshTokenService;
        _baseUrl = configuration["Backend:PersonalTransactions"]!;
    }


    public async Task<PersonalTransactionPeriodSummaryResponse> GetSummary(DateTime startOfMonth, string token,
        string refreshToken, string userId)
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
                PersonalTransactionPeriodSummaryResponse? response =
                    await JsonSerializer.DeserializeAsync<PersonalTransactionPeriodSummaryResponse>(responseStream);

                if (response is null)
                {
                    throw new Exception();
                }

                return response;
            }
        }
        else
        {
            return new PersonalTransactionPeriodSummaryResponse();
        }
    }

    public async Task<List<PersonalTransactionResponse>> GetTransactionsForPeriod(DateTime startOfMonth, string token,
        string refreshToken, string userId)
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
                List<PersonalTransactionResponse>? response =
                    await JsonSerializer.DeserializeAsync<List<PersonalTransactionResponse>>(responseStream);

                if (response is null)
                {
                    throw new Exception();
                }

                return response;
            }
        }
        else
        {
            return new List<PersonalTransactionResponse>();
        }
    }

    private static (DateOnly Start, DateOnly End) GetMonthRange(DateTime date)
    {
        var start = new DateOnly(date.Year, date.Month, 1);
        var end = new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        return (start, end);
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