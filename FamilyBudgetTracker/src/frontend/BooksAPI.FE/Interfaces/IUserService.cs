using BooksAPI.FE.Contracts.User;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Interfaces;

public interface IUserService
{
    Task<RegisterResponse?> Register(RegisterModel model, bool isAdmin);

    Task<LoginResponse?> Login(LoginModel model);

    Task Logout(string token);
}