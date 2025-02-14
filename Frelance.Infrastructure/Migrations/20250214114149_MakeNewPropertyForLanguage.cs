using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeNewPropertyForLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignLanguages",
                table: "FreelancerProfiles");

            migrationBuilder.CreateTable(
                name: "FreelancerForeignLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreelancerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerForeignLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FreelancerForeignLanguage_FreelancerProfiles_FreelancerProfileId",
                        column: x => x.FreelancerProfileId,
                        principalTable: "FreelancerProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerForeignLanguage_FreelancerProfileId",
                table: "FreelancerForeignLanguage",
                column: "FreelancerProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreelancerForeignLanguage");

            migrationBuilder.AddColumn<string>(
                name: "ForeignLanguages",
                table: "FreelancerProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
