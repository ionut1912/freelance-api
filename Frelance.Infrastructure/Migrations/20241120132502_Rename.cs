using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Client", "CLIENT" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Cleint", "CLEINT" });
        }
    }
}
