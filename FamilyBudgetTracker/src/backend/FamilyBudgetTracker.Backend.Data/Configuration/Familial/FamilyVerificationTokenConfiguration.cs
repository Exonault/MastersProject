using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetTracker.Backend.Data.Configuration.Familial;

public class FamilyVerificationTokenConfiguration : IEntityTypeConfiguration<FamilyInvitations>
{
    public void Configure(EntityTypeBuilder<FamilyInvitations> builder)
    {
        builder.HasKey(x => x.Id);
    }
}