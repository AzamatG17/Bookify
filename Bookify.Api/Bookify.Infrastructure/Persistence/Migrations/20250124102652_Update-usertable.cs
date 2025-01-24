using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updateusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("28ae89fb-d017-4558-950e-875b19bd43b4"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("9fdfe09f-a98e-49b1-8fe9-b21b56f01897"), new Guid("7ffd3a1e-f45e-4601-841b-2baa866bed89") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("9fdfe09f-a98e-49b1-8fe9-b21b56f01897"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("7ffd3a1e-f45e-4601-841b-2baa866bed89"));

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5c2be04c-85b6-4c52-af14-79fd8c685eab"), null, "admin", "ADMIN" },
                    { new Guid("b649d03a-0a82-4aa3-b694-5f4f90ad3421"), null, "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("b127f5cf-4a7a-403b-a03b-645952dbad5e"), 0, 0L, "cfa22e3f-6a6d-462c-9df8-2dd81d13fd18", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAENgsUFby33ij4kGoRFF99ft+MJEA55RTZI4+9DC0c4qaO2Dib4frhnO8rYsIifoPqA==", null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("5c2be04c-85b6-4c52-af14-79fd8c685eab"), new Guid("b127f5cf-4a7a-403b-a03b-645952dbad5e") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b649d03a-0a82-4aa3-b694-5f4f90ad3421"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("5c2be04c-85b6-4c52-af14-79fd8c685eab"), new Guid("b127f5cf-4a7a-403b-a03b-645952dbad5e") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("5c2be04c-85b6-4c52-af14-79fd8c685eab"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("b127f5cf-4a7a-403b-a03b-645952dbad5e"));

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("28ae89fb-d017-4558-950e-875b19bd43b4"), null, "user", "USER" },
                    { new Guid("9fdfe09f-a98e-49b1-8fe9-b21b56f01897"), null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("7ffd3a1e-f45e-4601-841b-2baa866bed89"), 0, 0L, "116f5c7a-93c3-4b96-9461-bbc5c105d22c", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", null, null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("9fdfe09f-a98e-49b1-8fe9-b21b56f01897"), new Guid("7ffd3a1e-f45e-4601-841b-2baa866bed89") });
        }
    }
}
