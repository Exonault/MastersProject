using FamilyBudgetTracker.Entities.Entities.Common;

namespace FamilyBudgetTracker.Entities.Entities.Familial;

public class Family
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<FamilyCategory> Categories { get; set; }

    public List<User> FamilyMembers { get; set; }

    public List<FamilyTransaction> Transactions { get; set; }
}