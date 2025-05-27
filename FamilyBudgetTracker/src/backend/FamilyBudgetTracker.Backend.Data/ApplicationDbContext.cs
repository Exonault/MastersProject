using FamilyBudgetTracker.Backend.Domain.Constants.User;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<PersonalCategory> PersonalCategories { get; set; }
    public DbSet<PersonalTransaction> PersonalTransactions { get; set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }


    public DbSet<Family> Family { get; set; }
    public DbSet<FamilyCategory> FamilyCategories { get; set; }
    public DbSet<FamilyTransaction> FamilyTransactions { get; set; }
    public DbSet<FamilyInvitations> FamilyInvitations { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        SeedAspNetRolesTable(builder);

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
            .HasData(
                new IdentityRole(UserConstants.RoleTypes.AdminRoleType)
                {
                    Id = UserConstants.RoleId.AdminRoleId,
                    NormalizedName = UserConstants.RoleNormalizedNames.Admin
                },
                new IdentityRole(UserConstants.RoleTypes.UserRoleType)
                {
                    Id = UserConstants.RoleId.UserRoleId,
                    NormalizedName = UserConstants.RoleNormalizedNames.User
                },
                new IdentityRole(UserConstants.RoleTypes.FamilyAdminRoleType)
                {
                    Id = UserConstants.RoleId.FamilyAdminRoleId,
                    NormalizedName = UserConstants.RoleNormalizedNames.FamilyAdmin
                },
                new IdentityRole(UserConstants.RoleTypes.FamilyMemberRoleType)
                {
                    Id = UserConstants.RoleId.FamilyMemberRoleId,
                    NormalizedName = UserConstants.RoleNormalizedNames.FamilyMember
                }
            );
    }
}