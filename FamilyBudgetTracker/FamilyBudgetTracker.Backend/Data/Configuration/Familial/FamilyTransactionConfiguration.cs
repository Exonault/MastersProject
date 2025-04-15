using FamilyBudgetTracker.Entities.Entities.Familial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetTracker.Backend.Data.Configuration.Familial;

public class FamilyTransactionConfiguration : IEntityTypeConfiguration<FamilyTransaction>
{
    public void Configure(EntityTypeBuilder<FamilyTransaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Amount).IsRequired();

        builder.Property(x => x.Description).IsRequired();

        builder.Property(x => x.TransactionDate).IsRequired();
    }
}