using System.ComponentModel.DataAnnotations;
using FamilyBudgetTracker.FE.Messages;

namespace FamilyBudgetTracker.FE.Model;

public class LoginModel
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}