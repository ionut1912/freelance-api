using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeletActiom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerForeignLanguage_FreelancerProfiles_FreelancerProfileId",
                table: "FreelancerForeignLanguage");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerForeignLanguage_FreelancerProfiles_FreelancerProfileId",
                table: "FreelancerForeignLanguage",
                column: "FreelancerProfileId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerForeignLanguage_FreelancerProfiles_FreelancerProfileId",
                table: "FreelancerForeignLanguage");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerForeignLanguage_FreelancerProfiles_FreelancerProfileId",
                table: "FreelancerForeignLanguage",
                column: "FreelancerProfileId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id");
        }
    }
}
