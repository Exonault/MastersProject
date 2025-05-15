using BooksAPI.FE.Contracts.Personal.Transaction;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Interfaces;

public interface IPersonalTransactionService
{
    Task<PersonalTransactionPeriodSummaryResponse> GetSummary(DateTime startOfMonth, string token, string refreshToken,
        string userId);

    Task<List<PersonalTransactionResponse>> GetTransactionsForPeriod(DateTime startOfMonth, string token,
        string refreshToken, string userId);

    Task<PersonalTransactionModel> GetTransactionModel(int id, string token, string refreshToken, string userId);

    Task<PersonalTransactionResponse> GetTransactionById(int id, string token, string refreshToken, string userId);

    Task<bool> CreateTransaction(PersonalTransactionModel model, int categoryId, string token, string refreshToken, string userId);
    Task<bool> UpdateTransaction(int id, PersonalTransactionModel model, int categoryId, string token, string refreshToken, string userId);
    Task<bool> DeleteTransaction(int id, string token, string refreshToken, string userId);
}