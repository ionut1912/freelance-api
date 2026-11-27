using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freelance.UserProfiles.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FreelancerForeignLanguagesCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FreelancerForeignLanguages",
                table: "FreelancerForeignLanguages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FreelancerForeignLanguages",
                table: "FreelancerForeignLanguages",
                columns: new[] { "FreelancerProfileId", "Language" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FreelancerForeignLanguages",
                table: "FreelancerForeignLanguages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FreelancerForeignLanguages",
                table: "FreelancerForeignLanguages",
                column: "FreelancerProfileId");
        }
    }
}
