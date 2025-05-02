using FamilyBudgetTracker.BE.Commons.Entities.Familial;

namespace FamilyBudgetTracker.BE.Commons.Repositories.Familial;

public interface IFamilyVerificationTokenRepository
{
    Task CreateVerificationToken(FamilyVerificationToken token);
    
    Task DeleteVerificationToken(FamilyVerificationToken token);

    Task<FamilyVerificationToken?> GetVerificationToken(string id);
}