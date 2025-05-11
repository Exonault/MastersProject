using System.ComponentModel.DataAnnotations;
using BooksAPI.FE.Messages;

namespace BooksAPI.FE.Model;

public class LoginModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}