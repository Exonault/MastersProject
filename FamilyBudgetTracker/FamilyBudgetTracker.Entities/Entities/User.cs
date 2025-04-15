using FamilyBudgetTracker.Entities.Entities.Familial;
using FamilyBudgetTracker.Entities.Entities.Personal;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetTracker.Entities.Entities;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiry { get; set; }
    
    public List<Category> Categories { get; set; }

    public List<PersonalTransaction> Transactions { get; set; }

    public List<RecurringTransaction> RecurringTransactions { get; set; }

    public Family? Family { get; set; }
}