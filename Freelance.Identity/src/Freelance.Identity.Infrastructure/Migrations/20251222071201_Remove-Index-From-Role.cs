using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freelance.Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIndexFromRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_Role",
                table: "Accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Role",
                table: "Accounts",
                column: "Role",
                unique: true);
        }
    }
}
