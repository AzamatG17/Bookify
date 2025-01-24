using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ServiceTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTranslation_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("28a0d6dd-d29a-401f-a0f3-58cfaab5a9dc"), null, "admin", "ADMIN" },
                    { new Guid("b19e3e32-0b28-414c-a4b7-8b292b26937a"), null, "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ChatId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("b7d920b7-11f1-4682-a932-e1dc24a71e0a"), 0, 0L, "545d9efc-ac3a-4a4e-90cc-9a00acc3cded", "admin@example.com", true, "admin", "admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", null, null, false, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("28a0d6dd-d29a-401f-a0f3-58cfaab5a9dc"), new Guid("b7d920b7-11f1-4682-a932-e1dc24a71e0a") });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTranslation_ServiceId",
                table: "ServiceTranslation",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceTranslation");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b19e3e32-0b28-414c-a4b7-8b292b26937a"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("28a0d6dd-d29a-401f-a0f3-58cfaab5a9dc"), new Guid("b7d920b7-11f1-4682-a932-e1dc24a71e0a") });

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("28a0d6dd-d29a-401f-a0f3-58cfaab5a9dc"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("b7d920b7-11f1-4682-a932-e1dc24a71e0a"));

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
        }
    }
}
