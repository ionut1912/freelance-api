using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freelance.UserProfiles.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyFreelancerConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FreelancerForeignLanguages",
                table: "FreelancerForeignLanguages");

            migrationBuilder.DropIndex(
                name: "IX_FreelancerForeignLanguages_FreelancerProfileId",
                table: "FreelancerForeignLanguages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FreelancerForeignLanguages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FreelancerForeignLanguages",
                table: "FreelancerForeignLanguages",
                column: "FreelancerProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FreelancerForeignLanguages",
                table: "FreelancerForeignLanguages");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "FreelancerForeignLanguages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FreelancerForeignLanguages",
                table: "FreelancerForeignLanguages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerForeignLanguages_FreelancerProfileId",
                table: "FreelancerForeignLanguages",
                column: "FreelancerProfileId");
        }
    }
}
