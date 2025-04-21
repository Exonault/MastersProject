using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetTracker.Entities.Contracts.User;

public class LoginRequest
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
  
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}