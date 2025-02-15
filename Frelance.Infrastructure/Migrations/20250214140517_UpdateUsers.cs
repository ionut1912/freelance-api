using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreelancerProfilesSkiills");

            migrationBuilder.DropColumn(
                name: "Technologies",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "ClientProfilesId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FreelancerProfilesId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FreelancerProfilesSkills",
                columns: table => new
                {
                    FreelancerProfilesId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerProfilesSkills", x => new { x.FreelancerProfilesId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_FreelancerProfilesSkills_FreelancerProfiles_FreelancerProfilesId",
                        column: x => x.FreelancerProfilesId,
                        principalTable: "FreelancerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FreelancerProfilesSkills_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTechnologies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Technology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTechnologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTechnologies_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientProfilesId",
                table: "Projects",
                column: "ClientProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FreelancerProfilesId",
                table: "Projects",
                column: "FreelancerProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerProfilesSkills_SkillsId",
                table: "FreelancerProfilesSkills",
                column: "SkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechnologies_ProjectId",
                table: "ProjectTechnologies",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ClientProfiles_ClientProfilesId",
                table: "Projects",
                column: "ClientProfilesId",
                principalTable: "ClientProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_FreelancerProfiles_FreelancerProfilesId",
                table: "Projects",
                column: "FreelancerProfilesId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ClientProfiles_ClientProfilesId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_FreelancerProfiles_FreelancerProfilesId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "FreelancerProfilesSkills");

            migrationBuilder.DropTable(
                name: "ProjectTechnologies");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ClientProfilesId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FreelancerProfilesId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ClientProfilesId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FreelancerProfilesId",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Technologies",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FreelancerProfilesSkiills",
                columns: table => new
                {
                    FreelancerProfilesId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerProfilesSkiills", x => new { x.FreelancerProfilesId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_FreelancerProfilesSkiills_FreelancerProfiles_FreelancerProfilesId",
                        column: x => x.FreelancerProfilesId,
                        principalTable: "FreelancerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FreelancerProfilesSkiills_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerProfilesSkiills_SkillsId",
                table: "FreelancerProfilesSkiills",
                column: "SkillsId");
        }
    }
}
