namespace FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;

public class CreateFamilyRequest
{
    public string Name { get; set; }

    public List<string> InviteList { get; set; }
}