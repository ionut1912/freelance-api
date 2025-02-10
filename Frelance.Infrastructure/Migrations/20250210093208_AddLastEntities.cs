using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLastEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProfiles_Addresses_AddressId",
                table: "ClientProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfiles_Addresses_AddressId",
                table: "FreelancerProfiles");

            migrationBuilder.DropTable(
                name: "FreelancerProfileSkiills");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "FreelancerProfiles",
                newName: "AddressesId");

            migrationBuilder.RenameIndex(
                name: "IX_FreelancerProfiles_AddressId",
                table: "FreelancerProfiles",
                newName: "IX_FreelancerProfiles_AddressesId");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "ClientProfiles",
                newName: "AddressesId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientProfiles_AddressId",
                table: "ClientProfiles",
                newName: "IX_ClientProfiles_AddressesId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "FreelancerProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortofolioUrl",
                table: "FreelancerProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "FreelancerProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "FreelancerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "ClientProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "ClientProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_ClientProfiles_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ClientProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_FreelancerProfiles_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "FreelancerProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contracts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_ClientProfiles_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ClientProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_FreelancerProfiles_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "FreelancerProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Proposals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ProposerId = table.Column<int>(type: "int", nullable: false),
                    ProposedBudget = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proposals_AspNetUsers_ProposerId",
                        column: x => x.ProposerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Proposals_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewerId = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_FreelancerId",
                table: "Contracts",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProjectId",
                table: "Contracts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerProfilesSkiills_SkillsId",
                table: "FreelancerProfilesSkiills",
                column: "SkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_FreelancerId",
                table: "Invoices",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ProjectId",
                table: "Invoices",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_ProjectId",
                table: "Proposals",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_ProposerId",
                table: "Proposals",
                column: "ProposerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Reviews",
                column: "ReviewerId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProfiles_Addresses_AddressesId",
                table: "ClientProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerProfiles_Addresses_AddressesId",
                table: "FreelancerProfiles");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "FreelancerProfilesSkiills");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Proposals");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "FreelancerProfiles");

            migrationBuilder.DropColumn(
                name: "PortofolioUrl",
                table: "FreelancerProfiles");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "FreelancerProfiles");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "FreelancerProfiles");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "ClientProfiles");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "ClientProfiles");

            migrationBuilder.RenameColumn(
                name: "AddressesId",
                table: "FreelancerProfiles",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_FreelancerProfiles_AddressesId",
                table: "FreelancerProfiles",
                newName: "IX_FreelancerProfiles_AddressId");

            migrationBuilder.RenameColumn(
                name: "AddressesId",
                table: "ClientProfiles",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientProfiles_AddressesId",
                table: "ClientProfiles",
                newName: "IX_ClientProfiles_AddressId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FreelancerProfileSkiills",
                columns: table => new
                {
                    FreelancerProfilesId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerProfileSkiills", x => new { x.FreelancerProfilesId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_FreelancerProfileSkiills_FreelancerProfiles_FreelancerProfilesId",
                        column: x => x.FreelancerProfilesId,
                        principalTable: "FreelancerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FreelancerProfileSkiills_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerProfileSkiills_SkillsId",
                table: "FreelancerProfileSkiills",
                column: "SkillsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProfiles_Addresses_AddressId",
                table: "ClientProfiles",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerProfiles_Addresses_AddressId",
                table: "FreelancerProfiles",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
