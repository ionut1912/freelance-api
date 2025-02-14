using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ClientProfiles_ClientProfileId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ClientProfileId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ClientProfileId",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientProfileId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientProfileId",
                table: "Projects",
                column: "ClientProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ClientProfiles_ClientProfileId",
                table: "Projects",
                column: "ClientProfileId",
                principalTable: "ClientProfiles",
                principalColumn: "Id");
        }
    }
}
