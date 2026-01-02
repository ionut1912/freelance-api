using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freelance.UserProfiles.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "FreelancerProfiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120000)",
                oldMaxLength: 120000);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "ClientProfiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120000)",
                oldMaxLength: 120000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "FreelancerProfiles",
                type: "character varying(120000)",
                maxLength: 120000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "ClientProfiles",
                type: "character varying(120000)",
                maxLength: 120000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
