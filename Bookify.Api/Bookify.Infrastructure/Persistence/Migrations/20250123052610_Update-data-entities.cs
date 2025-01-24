using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updatedataentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Companies_CompanyId",
                table: "Services");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("79bad706-46dd-475e-856c-fd588ca776f5"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("9b9da519-4ffe-4503-8826-2efb9fc476c6"), new Guid("7befed39-45f9-41a6-a533-5eaa046f03d0") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("9b9da519-4ffe-4503-8826-2efb9fc476c6"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("7befed39-45f9-41a6-a533-5eaa046f03d0"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Services",
                newName: "BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_CompanyId",
                table: "Services",
                newName: "IX_Services_BranchId");

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BranchAddres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoordinateLatitude = table.Column<double>(type: "float", nullable: true),
                    CoordinateLongitude = table.Column<double>(type: "float", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpeningTimeBranch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    OpenTime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningTimeBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeningTimeBranch_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("89c752d9-4146-4421-a439-ee42ff46b76e"), null, "user", "USER" },
                    { new Guid("b8b1f8e9-6b12-4fff-93cc-c775457c7187"), null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("abd7a292-7b09-4dbc-98e3-aa5d2e241c99"), 0, 0L, "55529548-107b-4437-9496-931451e02b38", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", null, null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("b8b1f8e9-6b12-4fff-93cc-c775457c7187"), new Guid("abd7a292-7b09-4dbc-98e3-aa5d2e241c99") });

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CompanyId",
                table: "Branch",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningTimeBranch_BranchId",
                table: "OpeningTimeBranch",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Branch_BranchId",
                table: "Services",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Branch_BranchId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "OpeningTimeBranch");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("89c752d9-4146-4421-a439-ee42ff46b76e"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b8b1f8e9-6b12-4fff-93cc-c775457c7187"), new Guid("abd7a292-7b09-4dbc-98e3-aa5d2e241c99") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b8b1f8e9-6b12-4fff-93cc-c775457c7187"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("abd7a292-7b09-4dbc-98e3-aa5d2e241c99"));

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "Services",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_BranchId",
                table: "Services",
                newName: "IX_Services_CompanyId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Services",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("79bad706-46dd-475e-856c-fd588ca776f5"), null, "user", "USER" },
                    { new Guid("9b9da519-4ffe-4503-8826-2efb9fc476c6"), null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("7befed39-45f9-41a6-a533-5eaa046f03d0"), 0, 0L, "abc9601c-7440-46be-a5c7-34d0cf4f67b2", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", null, null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("9b9da519-4ffe-4503-8826-2efb9fc476c6"), new Guid("7befed39-45f9-41a6-a533-5eaa046f03d0") });

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Companies_CompanyId",
                table: "Services",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
