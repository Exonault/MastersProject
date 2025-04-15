using FamilyBudgetTracker.Entities.Entities;
using FamilyBudgetTracker.Entities.Entities.Familial;
using FamilyBudgetTracker.Entities.Entities.Personal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<PersonalTransaction> Transactions { get; set; }
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
        
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

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
    
    private void SeedAspNetRolesTable(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>()
            .HasData(new IdentityRole("Admin") {Id = "3187bce0-f9a9-48fb-adb6-36cea86dfb16", NormalizedName = "ADMIN" },
                new IdentityRole("User") {Id = "284b3a5c-4235-4b01-ba23-09f2f6f9737c", NormalizedName = "USER" });
    }
}