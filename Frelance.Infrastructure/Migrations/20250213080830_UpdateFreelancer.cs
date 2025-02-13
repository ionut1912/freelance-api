using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFreelancer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeLogs_FreelancerProfiles_FreelancerProfileId",
                table: "TimeLogs");

            migrationBuilder.DropIndex(
                name: "IX_TimeLogs_FreelancerProfileId",
                table: "TimeLogs");

            migrationBuilder.AddColumn<int>(
                name: "FreelancerProfilesId",
                table: "TimeLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeLogs_FreelancerProfilesId",
                table: "TimeLogs",
                column: "FreelancerProfilesId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLogs_FreelancerProfiles_FreelancerProfilesId",
                table: "TimeLogs",
                column: "FreelancerProfilesId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeLogs_FreelancerProfiles_FreelancerProfilesId",
                table: "TimeLogs");

            migrationBuilder.DropIndex(
                name: "IX_TimeLogs_FreelancerProfilesId",
                table: "TimeLogs");

            migrationBuilder.DropColumn(
                name: "FreelancerProfilesId",
                table: "TimeLogs");

            migrationBuilder.CreateIndex(
                name: "IX_TimeLogs_FreelancerProfileId",
                table: "TimeLogs",
                column: "FreelancerProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLogs_FreelancerProfiles_FreelancerProfileId",
                table: "TimeLogs",
                column: "FreelancerProfileId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id");
        }
    }
}
