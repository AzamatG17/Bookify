using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2ff9790a-9113-455e-8a89-48526cc08622"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a5932dc8-e83b-4abb-92fa-dc2c57f6bbba"), new Guid("0ff2ef44-28d8-42ae-a7bf-6ed3b3d0af1e") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("a5932dc8-e83b-4abb-92fa-dc2c57f6bbba"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("0ff2ef44-28d8-42ae-a7bf-6ed3b3d0af1e"));

            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "User",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("b7b31f6e-5abc-4190-a868-75efe4a7c584"), null, "user", "USER" },
                    { new Guid("dd0024da-e9d8-49c7-b563-87090e5fdc8a"), null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("c0ffad1a-a088-4e4c-a1b4-d76232949775"), 0, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, "d52c81ad-aefb-4570-b3b6-cd3b8b59a319", "admin@example.com", true, "admin", "Male", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", null, null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("dd0024da-e9d8-49c7-b563-87090e5fdc8a"), new Guid("c0ffad1a-a088-4e4c-a1b4-d76232949775") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b7b31f6e-5abc-4190-a868-75efe4a7c584"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("dd0024da-e9d8-49c7-b563-87090e5fdc8a"), new Guid("c0ffad1a-a088-4e4c-a1b4-d76232949775") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("dd0024da-e9d8-49c7-b563-87090e5fdc8a"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("c0ffad1a-a088-4e4c-a1b4-d76232949775"));

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "User");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2ff9790a-9113-455e-8a89-48526cc08622"), null, "user", "USER" },
                    { new Guid("a5932dc8-e83b-4abb-92fa-dc2c57f6bbba"), null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("0ff2ef44-28d8-42ae-a7bf-6ed3b3d0af1e"), 0, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "da41291e-2cff-46f9-9e4b-abb9be1697a3", "admin@example.com", true, "admin", "Male", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", null, null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("a5932dc8-e83b-4abb-92fa-dc2c57f6bbba"), new Guid("0ff2ef44-28d8-42ae-a7bf-6ed3b3d0af1e") });
        }
    }
}
