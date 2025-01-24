using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServiceTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTranslation_Service_ServiceId",
                table: "ServiceTranslation");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTranslation_Service_ServiceId",
                table: "ServiceTranslation",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTranslation_Service_ServiceId",
                table: "ServiceTranslation");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTranslation_Service_ServiceId",
                table: "ServiceTranslation",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
