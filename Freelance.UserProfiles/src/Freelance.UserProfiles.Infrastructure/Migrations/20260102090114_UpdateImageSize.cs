using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freelance.UserProfiles.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "FreelancerProfiles",
                type: "character varying(120000)",
                maxLength: 120000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10000)",
                oldMaxLength: 10000);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "ClientProfiles",
                type: "character varying(120000)",
                maxLength: 120000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10000)",
                oldMaxLength: 10000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "FreelancerProfiles",
                type: "character varying(10000)",
                maxLength: 10000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120000)",
                oldMaxLength: 120000);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "ClientProfiles",
                type: "character varying(10000)",
                maxLength: 10000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120000)",
                oldMaxLength: 120000);
        }
    }
}
