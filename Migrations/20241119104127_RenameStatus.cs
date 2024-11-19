using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tasks",
                newName: "ProjectTaskStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectTaskStatus",
                table: "Tasks",
                newName: "Status");
        }
    }
}
