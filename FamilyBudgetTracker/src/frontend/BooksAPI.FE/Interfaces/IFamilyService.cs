using BooksAPI.FE.Contracts.Familial.Family;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Interfaces;

public interface IFamilyService
{
    Task<FamilyResponse> GetFamily(string id, string token, string refreshToken, string userId);

    Task<FamilyModel> GetFamilyModel(string id, string token, string refreshToken, string userId);

    Task<bool> CreateFamily(string token, string refreshToken, string userId);

    Task<bool> DeleteFamily(string id, string token, string refreshToken, string userId);

    Task<bool> InviteMembers(List<string> emails, string token, string refreshToken, string userId);
}