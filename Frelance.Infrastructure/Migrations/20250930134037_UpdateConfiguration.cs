using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfileSkill_FreelancerProfiles_FreelancerProfileId",
                table: "FreelancerProfileSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfileSkill_Skills_SkillId",
                table: "FreelancerProfileSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ClientProfiles_ClientProfilesId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_FreelancerProfiles_FreelancerProfilesId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ClientProfilesId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FreelancerProfilesId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FreelancerProfileSkill",
                table: "FreelancerProfileSkill");

            migrationBuilder.DropColumn(
                name: "ClientProfilesId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FreelancerProfilesId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "FreelancerProfiles");

            migrationBuilder.RenameTable(
                name: "FreelancerProfileSkill",
                newName: "FreelancerProfileSkills");

            migrationBuilder.RenameIndex(
                name: "IX_FreelancerProfileSkill_SkillId",
                table: "FreelancerProfileSkills",
                newName: "IX_FreelancerProfileSkills_SkillId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Skills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Skills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Technology",
                table: "ProjectTechnologies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProjectTechnologies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProjectTechnologies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FrelancerId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FreelancerForeignLanguage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FreelancerForeignLanguage",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Addresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Addresses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FreelancerProfileSkills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FreelancerProfileSkills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FreelancerProfileSkills",
                table: "FreelancerProfileSkills",
                columns: new[] { "FreelancerProfileId", "SkillId" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 30, 13, 40, 36, 221, DateTimeKind.Utc).AddTicks(2969), null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 30, 13, 40, 36, 221, DateTimeKind.Utc).AddTicks(2973), null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 30, 13, 40, 36, 221, DateTimeKind.Utc).AddTicks(2975), null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 30, 13, 40, 36, 221, DateTimeKind.Utc).AddTicks(2976), null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 30, 13, 40, 36, 221, DateTimeKind.Utc).AddTicks(2979), null });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 30, 13, 40, 36, 221, DateTimeKind.Utc).AddTicks(2981), null });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientId",
                table: "Projects",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FrelancerId",
                table: "Projects",
                column: "FrelancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerProfileSkills_FreelancerProfiles_FreelancerProfileId",
                table: "FreelancerProfileSkills",
                column: "FreelancerProfileId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerProfileSkills_Skills_SkillId",
                table: "FreelancerProfileSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ClientProfiles_ClientId",
                table: "Projects",
                column: "ClientId",
                principalTable: "ClientProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_FreelancerProfiles_FrelancerId",
                table: "Projects",
                column: "FrelancerId",
                principalTable: "FreelancerProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfileSkills_FreelancerProfiles_FreelancerProfileId",
                table: "FreelancerProfileSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfileSkills_Skills_SkillId",
                table: "FreelancerProfileSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ClientProfiles_ClientId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_FreelancerProfiles_FrelancerId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ClientId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FrelancerId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FreelancerProfileSkills",
                table: "FreelancerProfileSkills");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProjectTechnologies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProjectTechnologies");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FrelancerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FreelancerForeignLanguage");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FreelancerForeignLanguage");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FreelancerProfileSkills");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FreelancerProfileSkills");

            migrationBuilder.RenameTable(
                name: "FreelancerProfileSkills",
                newName: "FreelancerProfileSkill");

            migrationBuilder.RenameIndex(
                name: "IX_FreelancerProfileSkills_SkillId",
                table: "FreelancerProfileSkill",
                newName: "IX_FreelancerProfileSkill_SkillId");

            migrationBuilder.AlterColumn<string>(
                name: "Technology",
                table: "ProjectTechnologies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

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

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "FreelancerProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FreelancerProfileSkill",
                table: "FreelancerProfileSkill",
                columns: new[] { "FreelancerProfileId", "SkillId" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientProfilesId",
                table: "Projects",
                column: "ClientProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FreelancerProfilesId",
                table: "Projects",
                column: "FreelancerProfilesId");

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
    }
}
