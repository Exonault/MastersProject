using FamilyBudgetTracker.Backend.Domain.Entities.Common;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetTracker.Backend.Data.Configuration.Personal;

public class CategoryConfiguration : IEntityTypeConfiguration<PersonalCategory>
{
    public void Configure(EntityTypeBuilder<PersonalCategory> builder)
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