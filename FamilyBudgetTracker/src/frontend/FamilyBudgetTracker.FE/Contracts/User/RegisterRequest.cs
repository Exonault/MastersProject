using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetTracker.FE.Contracts.User;

public class RegisterRequest
{
    public string UserName { get; set; } = string.Empty;
    
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool Admin { get; set; }
}