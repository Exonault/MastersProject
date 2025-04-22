using FamilyBudgetTracker.BE.Commons.Entities.Common;
using FamilyBudgetTracker.BE.Commons.Entities.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetTracker.Backend.Data.Configuration.Personal;

public class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Amount).IsRequired();

        builder.Property(x => x.Description).IsRequired();

        builder.Property(x => x.StartDate).IsRequired();

        builder.Property(x => x.NextExecutionDate).IsRequired();

        builder.Property(x => x.EndDate).IsRequired();

        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Type).HasConversion(
            rt => rt.ToString(),
            t => (RecurringType)Enum.Parse(typeof(RecurringType), t)
        );
    }
}