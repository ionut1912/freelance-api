using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContractsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractFileUrl",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractFileUrl",
                table: "Contracts");
        }
    }
}
