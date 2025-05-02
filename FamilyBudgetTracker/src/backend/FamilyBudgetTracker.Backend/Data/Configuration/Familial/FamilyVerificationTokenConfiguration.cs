using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetTracker.Backend.Data.Configuration.Familial;

public class FamilyVerificationTokenConfiguration : IEntityTypeConfiguration<FamilyInvitationToken>
{
    public void Configure(EntityTypeBuilder<FamilyInvitationToken> builder)
    {
        builder.HasKey(x => x.Id);
    }
}