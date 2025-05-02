using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetTracker.Backend.Data.Configuration.Familial;

public class FamilyVerificationTokenConfiguration : IEntityTypeConfiguration<FamilyVerificationToken>
{
    public void Configure(EntityTypeBuilder<FamilyVerificationToken> builder)
    {
        builder.HasKey(x => x.Id);
    }
}