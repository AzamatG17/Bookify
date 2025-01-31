using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixStartTimeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("fa472cfa-5d4e-4149-8dc7-bb86cf5157b2"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("0ee63ebc-c821-4dfa-903f-bb4915452ac7"), new Guid("ecb1b6d9-bcac-46ac-b6e2-ad210f0b5713") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("0ee63ebc-c821-4dfa-903f-bb4915452ac7"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("ecb1b6d9-bcac-46ac-b6e2-ad210f0b5713"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Booking",
                type: "datetime2",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4b0238f8-ceeb-4896-9366-4f054d136cff"), null, "admin", "ADMIN" },
                    { new Guid("b17395de-ad6c-43ba-9059-70bdc80be587"), null, "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("59b3f8de-f59b-4f62-b4cf-fd45034ee506"), 0, 0L, "2ce09139-4249-4464-9427-2e2c546c92e9", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEHz7NcGdnGuFGbNE6+51o+7vritld2HPs97zr9qfH1wWU+vO6YG6Oajash9DJG1rSA==", null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("4b0238f8-ceeb-4896-9366-4f054d136cff"), new Guid("59b3f8de-f59b-4f62-b4cf-fd45034ee506") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b17395de-ad6c-43ba-9059-70bdc80be587"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("4b0238f8-ceeb-4896-9366-4f054d136cff"), new Guid("59b3f8de-f59b-4f62-b4cf-fd45034ee506") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("4b0238f8-ceeb-4896-9366-4f054d136cff"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("59b3f8de-f59b-4f62-b4cf-fd45034ee506"));

            migrationBuilder.AlterColumn<string>(
                name: "StartDate",
                table: "Booking",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0ee63ebc-c821-4dfa-903f-bb4915452ac7"), null, "admin", "ADMIN" },
                    { new Guid("fa472cfa-5d4e-4149-8dc7-bb86cf5157b2"), null, "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("ecb1b6d9-bcac-46ac-b6e2-ad210f0b5713"), 0, 0L, "77471469-dad9-4519-abd4-9d7e1dfca6ea", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEBnxlU4xi8P2YNlw5JnExW1m8j6jEZz1GPv2rCJ0EpmKOlRpDbKnlb2G76pHPtEdXw==", null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("0ee63ebc-c821-4dfa-903f-bb4915452ac7"), new Guid("ecb1b6d9-bcac-46ac-b6e2-ad210f0b5713") });
        }
    }
}
