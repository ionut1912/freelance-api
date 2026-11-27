using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freelance.ProjectManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTechnologyCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTechnologies",
                table: "ProjectTechnologies");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTechnologies_ProjectId",
                table: "ProjectTechnologies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProjectTechnologies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTechnologies",
                table: "ProjectTechnologies",
                columns: new[] { "ProjectId", "Technology" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTechnologies",
                table: "ProjectTechnologies");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProjectTechnologies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTechnologies",
                table: "ProjectTechnologies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechnologies_ProjectId",
                table: "ProjectTechnologies",
                column: "ProjectId");
        }
    }
}
