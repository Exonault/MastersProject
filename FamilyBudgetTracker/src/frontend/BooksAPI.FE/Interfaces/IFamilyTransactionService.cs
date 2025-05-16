using BooksAPI.FE.Contracts.Familial.FamilyTransaction;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Interfaces;

public interface IFamilyTransactionService
{
    Task<FamilyTransactionPeriodSummaryResponse> GetSummary(DateTime startOfMonth, string token, string refreshToken,
        string userId, string familyId);

    Task<List<FamilyTransactionResponse>> GetTransactionsForPeriod(DateTime startOfMonth, string token,
        string refreshToken, string userId, string familyId);

    Task<FamilyTransactionModel> GetTransactionModel(int id, string token, string refreshToken, string userId,
        string familyId);

    Task<FamilyTransactionResponse> GetTransactionById(int id, string token, string refreshToken, string userId,
        string familyId);

    Task<bool> CreateTransaction(FamilyTransactionModel model, int categoryId, string token, string refreshToken,
        string userId, string familyId);

    Task<bool> UpdateTransaction(int id, FamilyTransactionModel model, int categoryId, string token,
        string refreshToken, string userId, string familyId);

    Task<bool> DeleteTransaction(int id, string token, string refreshToken, string userId, string familyId);
}