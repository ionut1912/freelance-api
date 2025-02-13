using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFrelancerFromProjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_FreelancerProfiles_FreelancerProfileId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FreelancerProfileId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FreelancerProfileId",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FreelancerProfileId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FreelancerProfileId",
                table: "Projects",
                column: "FreelancerProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_FreelancerProfiles_FreelancerProfileId",
                table: "Projects",
                column: "FreelancerProfileId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id");
        }
    }
}
