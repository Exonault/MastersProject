using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyBudgetTracker.Backend.Data.Configuration.Personal;

public class TransactionConfiguration : IEntityTypeConfiguration<PersonalTransaction>
{
    public void Configure(EntityTypeBuilder<PersonalTransaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Amount).IsRequired();

        builder.Property(x => x.TransactionDate).IsRequired();
    }
}