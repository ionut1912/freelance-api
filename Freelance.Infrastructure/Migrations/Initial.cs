#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Freelance.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Addresses",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Country = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                City = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Street = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                StreetNumber = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                ZipCode = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Addresses", x => x.Id); });

        migrationBuilder.CreateTable(
            "AspNetRoles",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

        migrationBuilder.CreateTable(
            "AspNetUsers",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UserName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>("bit", nullable: false),
                PasswordHash = table.Column<string>("nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>("nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>("bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>("bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>("bit", nullable: false),
                AccessFailedCount = table.Column<int>("int", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });

        migrationBuilder.CreateTable(
            "Skills",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProgrammingLanguage = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Area = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Skills", x => x.Id); });

        migrationBuilder.CreateTable(
            "AspNetRoleClaims",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<int>("int", nullable: false),
                ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    x => x.RoleId,
                    "AspNetRoles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserClaims",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>("int", nullable: false),
                ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    "FK_AspNetUserClaims_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserLogins",
            table => new
            {
                LoginProvider = table.Column<string>("nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>("nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>("nvarchar(max)", nullable: true),
                UserId = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    "FK_AspNetUserLogins_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserRoles",
            table => new
            {
                UserId = table.Column<int>("int", nullable: false),
                RoleId = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    x => x.RoleId,
                    "AspNetRoles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_AspNetUserRoles_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserTokens",
            table => new
            {
                UserId = table.Column<int>("int", nullable: false),
                LoginProvider = table.Column<string>("nvarchar(450)", nullable: false),
                Name = table.Column<string>("nvarchar(450)", nullable: false),
                Value = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    "FK_AspNetUserTokens_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "ClientProfiles",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true),
                UserId = table.Column<int>("int", nullable: false),
                AddressId = table.Column<int>("int", nullable: false),
                Bio = table.Column<string>("nvarchar(max)", nullable: false),
                Image = table.Column<string>("nvarchar(max)", nullable: false),
                IsVerified = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientProfiles", x => x.Id);
                table.ForeignKey(
                    "FK_ClientProfiles_Addresses_AddressId",
                    x => x.AddressId,
                    "Addresses",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_ClientProfiles_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "FreelancerProfiles",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Experience = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Rate = table.Column<int>("int", nullable: false),
                Currency = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Rating = table.Column<int>("int", nullable: true),
                PortfolioUrl = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true),
                UserId = table.Column<int>("int", nullable: false),
                AddressId = table.Column<int>("int", nullable: false),
                Bio = table.Column<string>("nvarchar(max)", nullable: false),
                Image = table.Column<string>("nvarchar(max)", nullable: false),
                IsVerified = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FreelancerProfiles", x => x.Id);
                table.ForeignKey(
                    "FK_FreelancerProfiles_Addresses_AddressId",
                    x => x.AddressId,
                    "Addresses",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_FreelancerProfiles_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "Reviews",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ReviewerId = table.Column<int>("int", nullable: false),
                ReviewText = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reviews", x => x.Id);
                table.ForeignKey(
                    "FK_Reviews_AspNetUsers_ReviewerId",
                    x => x.ReviewerId,
                    "AspNetUsers",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "FreelancerForeignLanguage",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Language = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                FreelancerProfileId = table.Column<int>("int", nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FreelancerForeignLanguage", x => x.Id);
                table.ForeignKey(
                    "FK_FreelancerForeignLanguage_FreelancerProfiles_FreelancerProfileId",
                    x => x.FreelancerProfileId,
                    "FreelancerProfiles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "FreelancerProfileSkills",
            table => new
            {
                FreelancerProfileId = table.Column<int>("int", nullable: false),
                SkillId = table.Column<int>("int", nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FreelancerProfileSkills", x => new { x.FreelancerProfileId, x.SkillId });
                table.ForeignKey(
                    "FK_FreelancerProfileSkills_FreelancerProfiles_FreelancerProfileId",
                    x => x.FreelancerProfileId,
                    "FreelancerProfiles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_FreelancerProfileSkills_Skills_SkillId",
                    x => x.SkillId,
                    "Skills",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Projects",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Deadline = table.Column<DateTime>("datetime2", nullable: false),
                ClientId = table.Column<int>("int", nullable: false),
                FreelancerId = table.Column<int>("int", nullable: false),
                Budget = table.Column<decimal>("decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Projects", x => x.Id);
                table.ForeignKey(
                    "FK_Projects_ClientProfiles_ClientId",
                    x => x.ClientId,
                    "ClientProfiles",
                    "Id");
                table.ForeignKey(
                    "FK_Projects_FreelancerProfiles_FreelancerId",
                    x => x.FreelancerId,
                    "FreelancerProfiles",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "Contracts",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjectId = table.Column<int>("int", nullable: false),
                ClientId = table.Column<int>("int", nullable: false),
                FreelancerId = table.Column<int>("int", nullable: false),
                StartDate = table.Column<DateTime>("datetime2", nullable: false),
                EndDate = table.Column<DateTime>("datetime2", nullable: false),
                Amount = table.Column<decimal>("decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Status = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                ContractFile = table.Column<string>("nvarchar(max)", maxLength: 205000, nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contracts", x => x.Id);
                table.ForeignKey(
                    "FK_Contracts_ClientProfiles_ClientId",
                    x => x.ClientId,
                    "ClientProfiles",
                    "Id");
                table.ForeignKey(
                    "FK_Contracts_FreelancerProfiles_FreelancerId",
                    x => x.FreelancerId,
                    "FreelancerProfiles",
                    "Id");
                table.ForeignKey(
                    "FK_Contracts_Projects_ProjectId",
                    x => x.ProjectId,
                    "Projects",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "Invoices",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjectId = table.Column<int>("int", nullable: false),
                ClientId = table.Column<int>("int", nullable: false),
                FreelancerId = table.Column<int>("int", nullable: false),
                Amount = table.Column<decimal>("decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Status = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                InvoiceFile = table.Column<string>("nvarchar(max)", maxLength: 205000, nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Invoices", x => x.Id);
                table.ForeignKey(
                    "FK_Invoices_ClientProfiles_ClientId",
                    x => x.ClientId,
                    "ClientProfiles",
                    "Id");
                table.ForeignKey(
                    "FK_Invoices_FreelancerProfiles_FreelancerId",
                    x => x.FreelancerId,
                    "FreelancerProfiles",
                    "Id");
                table.ForeignKey(
                    "FK_Invoices_Projects_ProjectId",
                    x => x.ProjectId,
                    "Projects",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "ProjectTechnologies",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Technology = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                ProjectId = table.Column<int>("int", nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectTechnologies", x => x.Id);
                table.ForeignKey(
                    "FK_ProjectTechnologies_Projects_ProjectId",
                    x => x.ProjectId,
                    "Projects",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Proposals",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjectId = table.Column<int>("int", nullable: false),
                ProposerId = table.Column<int>("int", nullable: false),
                ProposedBudget = table.Column<decimal>("decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Status = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Proposals", x => x.Id);
                table.ForeignKey(
                    "FK_Proposals_AspNetUsers_ProposerId",
                    x => x.ProposerId,
                    "AspNetUsers",
                    "Id");
                table.ForeignKey(
                    "FK_Proposals_Projects_ProjectId",
                    x => x.ProjectId,
                    "Projects",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "Tasks",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjectId = table.Column<int>("int", nullable: false),
                Title = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                FreelancerProfileId = table.Column<int>("int", nullable: false),
                Status = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                Priority = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tasks", x => x.Id);
                table.ForeignKey(
                    "FK_Tasks_FreelancerProfiles_FreelancerProfileId",
                    x => x.FreelancerProfileId,
                    "FreelancerProfiles",
                    "Id");
                table.ForeignKey(
                    "FK_Tasks_Projects_ProjectId",
                    x => x.ProjectId,
                    "Projects",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "TimeLogs",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                TaskId = table.Column<int>("int", nullable: false),
                StartTime = table.Column<DateTime>("datetime2", nullable: false),
                EndTime = table.Column<DateTime>("datetime2", nullable: false),
                FreelancerProfileId = table.Column<int>("int", nullable: false),
                TotalHours = table.Column<int>("int", nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TimeLogs", x => x.Id);
                table.ForeignKey(
                    "FK_TimeLogs_Tasks_TaskId",
                    x => x.TaskId,
                    "Tasks",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            "AspNetRoles",
            new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            new object[,]
            {
                { 1, null, "Freelancer", "FREELANCER" },
                { 2, null, "Client", "CLIENT" }
            });

        migrationBuilder.InsertData(
            "Skills",
            new[] { "Id", "Area", "CreatedAt", "ProgrammingLanguage", "UpdatedAt" },
            new object[,]
            {
                {
                    1, "Backend", new DateTime(2025, 9, 30, 14, 51, 51, 189, DateTimeKind.Utc).AddTicks(1357), ".NET",
                    null
                },
                {
                    2, "Frontend", new DateTime(2025, 9, 30, 14, 51, 51, 189, DateTimeKind.Utc).AddTicks(1361),
                    "Angular", null
                },
                {
                    3, "Frontend", new DateTime(2025, 9, 30, 14, 51, 51, 189, DateTimeKind.Utc).AddTicks(1362),
                    "JavaScript", null
                },
                {
                    4, "Frontend", new DateTime(2025, 9, 30, 14, 51, 51, 189, DateTimeKind.Utc).AddTicks(1363), "React",
                    null
                },
                {
                    5, "Backend", new DateTime(2025, 9, 30, 14, 51, 51, 189, DateTimeKind.Utc).AddTicks(1365), "Python",
                    null
                },
                {
                    6, "Backend", new DateTime(2025, 9, 30, 14, 51, 51, 189, DateTimeKind.Utc).AddTicks(1366), "Java",
                    null
                }
            });

        migrationBuilder.CreateIndex(
            "IX_AspNetRoleClaims_RoleId",
            "AspNetRoleClaims",
            "RoleId");

        migrationBuilder.CreateIndex(
            "RoleNameIndex",
            "AspNetRoles",
            "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserClaims_UserId",
            "AspNetUserClaims",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserLogins_UserId",
            "AspNetUserLogins",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserRoles_RoleId",
            "AspNetUserRoles",
            "RoleId");

        migrationBuilder.CreateIndex(
            "EmailIndex",
            "AspNetUsers",
            "NormalizedEmail");

        migrationBuilder.CreateIndex(
            "UserNameIndex",
            "AspNetUsers",
            "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_ClientProfiles_AddressId",
            "ClientProfiles",
            "AddressId",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientProfiles_UserId",
            "ClientProfiles",
            "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Contracts_ClientId",
            "Contracts",
            "ClientId");

        migrationBuilder.CreateIndex(
            "IX_Contracts_FreelancerId",
            "Contracts",
            "FreelancerId");

        migrationBuilder.CreateIndex(
            "IX_Contracts_ProjectId",
            "Contracts",
            "ProjectId");

        migrationBuilder.CreateIndex(
            "IX_FreelancerForeignLanguage_FreelancerProfileId",
            "FreelancerForeignLanguage",
            "FreelancerProfileId");

        migrationBuilder.CreateIndex(
            "IX_FreelancerProfiles_AddressId",
            "FreelancerProfiles",
            "AddressId",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_FreelancerProfiles_UserId",
            "FreelancerProfiles",
            "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_FreelancerProfileSkills_SkillId",
            "FreelancerProfileSkills",
            "SkillId");

        migrationBuilder.CreateIndex(
            "IX_Invoices_ClientId",
            "Invoices",
            "ClientId");

        migrationBuilder.CreateIndex(
            "IX_Invoices_FreelancerId",
            "Invoices",
            "FreelancerId");

        migrationBuilder.CreateIndex(
            "IX_Invoices_ProjectId",
            "Invoices",
            "ProjectId");

        migrationBuilder.CreateIndex(
            "IX_Projects_ClientId",
            "Projects",
            "ClientId");

        migrationBuilder.CreateIndex(
            "IX_Projects_FreelancerId",
            "Projects",
            "FreelancerId");

        migrationBuilder.CreateIndex(
            "IX_ProjectTechnologies_ProjectId",
            "ProjectTechnologies",
            "ProjectId");

        migrationBuilder.CreateIndex(
            "IX_Proposals_ProjectId",
            "Proposals",
            "ProjectId");

        migrationBuilder.CreateIndex(
            "IX_Proposals_ProposerId",
            "Proposals",
            "ProposerId");

        migrationBuilder.CreateIndex(
            "IX_Reviews_ReviewerId",
            "Reviews",
            "ReviewerId");

        migrationBuilder.CreateIndex(
            "IX_Tasks_FreelancerProfileId",
            "Tasks",
            "FreelancerProfileId");

        migrationBuilder.CreateIndex(
            "IX_Tasks_ProjectId",
            "Tasks",
            "ProjectId");

        migrationBuilder.CreateIndex(
            "IX_TimeLogs_TaskId",
            "TimeLogs",
            "TaskId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "AspNetRoleClaims");

        migrationBuilder.DropTable(
            "AspNetUserClaims");

        migrationBuilder.DropTable(
            "AspNetUserLogins");

        migrationBuilder.DropTable(
            "AspNetUserRoles");

        migrationBuilder.DropTable(
            "AspNetUserTokens");

        migrationBuilder.DropTable(
            "Contracts");

        migrationBuilder.DropTable(
            "FreelancerForeignLanguage");

        migrationBuilder.DropTable(
            "FreelancerProfileSkills");

        migrationBuilder.DropTable(
            "Invoices");

        migrationBuilder.DropTable(
            "ProjectTechnologies");

        migrationBuilder.DropTable(
            "Proposals");

        migrationBuilder.DropTable(
            "Reviews");

        migrationBuilder.DropTable(
            "TimeLogs");

        migrationBuilder.DropTable(
            "AspNetRoles");

        migrationBuilder.DropTable(
            "Skills");

        migrationBuilder.DropTable(
            "Tasks");

        migrationBuilder.DropTable(
            "Projects");

        migrationBuilder.DropTable(
            "ClientProfiles");

        migrationBuilder.DropTable(
            "FreelancerProfiles");

        migrationBuilder.DropTable(
            "Addresses");

        migrationBuilder.DropTable(
            "AspNetUsers");
    }
}