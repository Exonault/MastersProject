using FamilyBudgetTracker.BE.Commons.Entities.Common;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetTracker.Backend.Data.Configuration.Familial;

public class FamilyCategoryConfiguration : IEntityTypeConfiguration<FamilyCategory>
{
    public void Configure(EntityTypeBuilder<FamilyCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).IsRequired();
        
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Type).HasConversion(
            ct => ct.ToString(),
            t => (CategoryType)Enum.Parse(typeof(CategoryType), t)
        );
        
        
    }
}