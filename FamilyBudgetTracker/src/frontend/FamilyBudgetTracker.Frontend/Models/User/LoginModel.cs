using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetTracker.Frontend.Models.User;

public class LoginModel
{
    [Required]
    // [StringLength(50, ErrorMessage = UserMessages.UsernameErrorMessage , MinimumLength = 8)]
    public string Email { get; set; } = string.Empty;

    [Required]
    // [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}