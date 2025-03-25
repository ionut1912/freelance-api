using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFrelancerSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfileSkill_FreelancerProfileId",
                table: "FreelancerProfileSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfileSkill_SkillId",
                table: "FreelancerProfileSkill");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerProfileSkill_FreelancerProfiles_FreelancerProfileId",
                table: "FreelancerProfileSkill",
                column: "FreelancerProfileId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerProfileSkill_Skills_SkillId",
                table: "FreelancerProfileSkill",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfileSkill_FreelancerProfiles_FreelancerProfileId",
                table: "FreelancerProfileSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfileSkill_Skills_SkillId",
                table: "FreelancerProfileSkill");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerProfileSkill_FreelancerProfileId",
                table: "FreelancerProfileSkill",
                column: "FreelancerProfileId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerProfileSkill_SkillId",
                table: "FreelancerProfileSkill",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
