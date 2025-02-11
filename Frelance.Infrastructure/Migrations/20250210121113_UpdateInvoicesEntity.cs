using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoicesEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceFileUrl",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceFileUrl",
                table: "Invoices");
        }
    }
}
