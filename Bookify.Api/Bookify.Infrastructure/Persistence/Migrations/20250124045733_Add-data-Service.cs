using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdddataService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("609a80b5-95b0-42af-86ba-67fe9295e896"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("e6cf504a-7e15-4a27-b032-c7b6051f23c1"), new Guid("a193d86e-fe4b-4a55-9308-be7ed9991bab") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("e6cf504a-7e15-4a27-b032-c7b6051f23c1"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("a193d86e-fe4b-4a55-9308-be7ed9991bab"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Service");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Service",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("35663efa-b5c8-445e-9507-4a03ea396025"), null, "user", "USER" },
                    { new Guid("45f7b1de-6a9f-4579-9410-1efbe7fe97ed"), null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("1a0bfec0-862e-4d5a-987d-cc5807fcefb5"), 0, 0L, "6fb91de5-eae4-45b2-8986-4507ad75cc61", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", null, null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("45f7b1de-6a9f-4579-9410-1efbe7fe97ed"), new Guid("1a0bfec0-862e-4d5a-987d-cc5807fcefb5") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("35663efa-b5c8-445e-9507-4a03ea396025"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("45f7b1de-6a9f-4579-9410-1efbe7fe97ed"), new Guid("1a0bfec0-862e-4d5a-987d-cc5807fcefb5") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("45f7b1de-6a9f-4579-9410-1efbe7fe97ed"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("1a0bfec0-862e-4d5a-987d-cc5807fcefb5"));

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Service");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Service",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("609a80b5-95b0-42af-86ba-67fe9295e896"), null, "user", "USER" },
                    { new Guid("e6cf504a-7e15-4a27-b032-c7b6051f23c1"), null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("a193d86e-fe4b-4a55-9308-be7ed9991bab"), 0, 0L, "4db0c384-e4ac-4c7a-863b-b96140716309", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", null, null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("e6cf504a-7e15-4a27-b032-c7b6051f23c1"), new Guid("a193d86e-fe4b-4a55-9308-be7ed9991bab") });
        }
    }
}
