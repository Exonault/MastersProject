using BooksAPI.FE.Contracts.Personal.RecurringTransaction;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Interfaces;

public interface IRecurringTransactionService
{
    Task<List<RecurringTransactionResponse>> GetRecurringTransactions(string token, string refreshToken, string userId);

    Task<RecurringTransactionModel> GetRecurringTransactionModel(int id, string token, string refreshToken, string userId);

    Task<RecurringTransactionResponse> GetTransactionById(int id, string token, string refreshToken, string userId);

    Task<bool> CreateTransaction(RecurringTransactionModel model, int categoryId, string token, string refreshToken,
        string userId);

    [Obsolete("Not implemented. not needed")]
    Task<bool> UpdateTransaction(int id, RecurringTransactionModel model, int categoryId, string token,
        string refreshToken, string userId);

    Task<bool> DeleteTransaction(int id, string token, string refreshToken, string userId);
}