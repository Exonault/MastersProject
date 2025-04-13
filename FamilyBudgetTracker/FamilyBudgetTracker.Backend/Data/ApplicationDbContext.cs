using FamilyBudgetTracker.Entities.Entities;
using FamilyBudgetTracker.Entities.Entities.Familial;
using FamilyBudgetTracker.Entities.Entities.Personal;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }


    public DbSet<Family> Family { get; set; }
    public DbSet<FamilyCategory> FamilyCategories { get; set; }
    public DbSet<FamilyTransaction> FamilyTransactions { get; set; }
    
    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasMany(u => u.Transactions)
            .WithOne(t => t.User);
        
        builder.Entity<User>()
            .HasMany(u => u.RecurringTransactions)
            .WithOne(rt => rt.User);
        
        builder.Entity<User>()
            .HasMany(u => u.Categories)
            .WithOne(c => c.User);
        
        builder.Entity<Family>()
            .HasMany(f => f.Categories)
            .WithOne(fc => fc.Family);
        
        builder.Entity<Family>()
            .HasMany(fc => fc.Transactions)
            .WithOne(ft => ft.Family);
    }
}