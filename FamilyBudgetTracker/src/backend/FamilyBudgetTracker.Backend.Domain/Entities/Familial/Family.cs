namespace FamilyBudgetTracker.Backend.Domain.Entities.Familial;

public class Family
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<FamilyCategory> Categories { get; set; }

    public List<User> FamilyMembers { get; set; }

    public List<FamilyTransaction> Transactions { get; set; }
}