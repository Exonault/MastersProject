using System.ComponentModel.DataAnnotations;
using BooksAPI.FE.Messages;

namespace BooksAPI.FE.Model;

public class RegisterModel //TODO add errorMessages
{
    [Required]
    [StringLength(50, ErrorMessage = UserMessages.UsernameErrorMessage , MinimumLength = 8)]
    public string Username { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
        ErrorMessage = UserMessages.PasswordErrorMessage)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Compare(nameof(this.Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}