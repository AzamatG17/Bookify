using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updatebranchservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
