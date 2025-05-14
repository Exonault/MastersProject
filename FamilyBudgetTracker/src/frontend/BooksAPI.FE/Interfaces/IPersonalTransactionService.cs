using BooksAPI.FE.Contracts.Personal.Transaction;

namespace BooksAPI.FE.Interfaces;

public interface IPersonalTransactionService
{
    Task<PersonalTransactionPeriodSummaryResponse> GetSummary(DateTime startOfMonth, string token, string refreshToken, string userId);

    Task<List<PersonalTransactionResponse>> GetTransactionsForPeriod(DateTime startOfMonth, string token, string refreshToken, string userId);
}