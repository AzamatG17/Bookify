using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdddataCompnay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7dec89d2-4798-4033-bd70-94cdb35ad074"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("8a13e052-d5da-42d6-beb9-cf08d77dc163"), new Guid("1d4843dc-a3c7-43ab-8c71-df3c008fbd3b") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("8a13e052-d5da-42d6-beb9-cf08d77dc163"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("1d4843dc-a3c7-43ab-8c71-df3c008fbd3b"));

            migrationBuilder.AddColumn<string>(
                name: "LogoBase64",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0e89e70c-3930-461e-aafe-2720390242d6"), null, "user", "USER" },
                    { new Guid("2120dfd2-bb70-4119-927e-d19de1ff81d3"), null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("f1313997-0334-4701-b5fe-e983cc990705"), 0, 0L, "c0a86236-aec2-4abd-b03f-5af69ac7d339", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEK2fceNf3+B4L68TqCkfmDqbyqeIrK8c8rnmz2YR5lzdX6hfvNabzL7aunxaf0jIHw==", null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("2120dfd2-bb70-4119-927e-d19de1ff81d3"), new Guid("f1313997-0334-4701-b5fe-e983cc990705") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("0e89e70c-3930-461e-aafe-2720390242d6"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("2120dfd2-bb70-4119-927e-d19de1ff81d3"), new Guid("f1313997-0334-4701-b5fe-e983cc990705") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2120dfd2-bb70-4119-927e-d19de1ff81d3"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("f1313997-0334-4701-b5fe-e983cc990705"));

            migrationBuilder.DropColumn(
                name: "LogoBase64",
                table: "Companies");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7dec89d2-4798-4033-bd70-94cdb35ad074"), null, "user", "USER" },
                    { new Guid("8a13e052-d5da-42d6-beb9-cf08d77dc163"), null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("1d4843dc-a3c7-43ab-8c71-df3c008fbd3b"), 0, 0L, "786279a7-8ab9-4779-aa70-c05bdc44f2e2", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAENHuUQw2TMnGLx+XzThm/vBJ+D2EjUImX0fn3f2BDtW3+ya6mSayPh9Dk0eRkUKwLQ==", null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("8a13e052-d5da-42d6-beb9-cf08d77dc163"), new Guid("1d4843dc-a3c7-43ab-8c71-df3c008fbd3b") });
        }
    }
}
