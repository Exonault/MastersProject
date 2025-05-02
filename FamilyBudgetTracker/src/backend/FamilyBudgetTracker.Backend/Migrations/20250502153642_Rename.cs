using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyBudgetTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyVerificationTokens");

            migrationBuilder.CreateTable(
                name: "FamilyInvitationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    UserInApplication = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyInvitationTokens", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyInvitationTokens");

            migrationBuilder.CreateTable(
                name: "FamilyVerificationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserInApplication = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyVerificationTokens", x => x.Id);
                });
        }
    }
}
