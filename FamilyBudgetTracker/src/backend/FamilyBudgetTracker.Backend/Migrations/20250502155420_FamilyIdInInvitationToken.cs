using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyBudgetTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class FamilyIdInInvitationToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FamilyId",
                table: "FamilyInvitationTokens",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FamilyId",
                table: "FamilyInvitationTokens");
        }
    }
}
