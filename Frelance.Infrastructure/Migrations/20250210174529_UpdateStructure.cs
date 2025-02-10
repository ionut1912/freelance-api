using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProfiles_Addresses_AddressesId",
                table: "ClientProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfiles_Addresses_AddressesId",
                table: "FreelancerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_FreelancerProfiles_AddressesId",
                table: "FreelancerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ClientProfiles_AddressesId",
                table: "ClientProfiles");

            migrationBuilder.DropColumn(
                name: "AddressesId",
                table: "FreelancerProfiles");

            migrationBuilder.DropColumn(
                name: "AddressesId",
                table: "ClientProfiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "TimeLogs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProposedBudget",
                table: "Proposals",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Budget",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "FreelancerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Contracts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "ClientProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerProfiles_AddressId",
                table: "FreelancerProfiles",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfiles_AddressId",
                table: "ClientProfiles",
                column: "AddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProfiles_Addresses_AddressId",
                table: "ClientProfiles",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerProfiles_Addresses_AddressId",
                table: "FreelancerProfiles",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProfiles_Addresses_AddressId",
                table: "ClientProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfiles_Addresses_AddressId",
                table: "FreelancerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_FreelancerProfiles_AddressId",
                table: "FreelancerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ClientProfiles_AddressId",
                table: "ClientProfiles");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "FreelancerProfiles");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "ClientProfiles");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "TimeLogs",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ProposedBudget",
                table: "Proposals",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<float>(
                name: "Budget",
                table: "Projects",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Invoices",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Invoices",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "AddressesId",
                table: "FreelancerProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "Contracts",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "Contracts",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Contracts",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "AddressesId",
                table: "ClientProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerProfiles_AddressesId",
                table: "FreelancerProfiles",
                column: "AddressesId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfiles_AddressesId",
                table: "ClientProfiles",
                column: "AddressesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProfiles_Addresses_AddressesId",
                table: "ClientProfiles",
                column: "AddressesId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerProfiles_Addresses_AddressesId",
                table: "FreelancerProfiles",
                column: "AddressesId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
