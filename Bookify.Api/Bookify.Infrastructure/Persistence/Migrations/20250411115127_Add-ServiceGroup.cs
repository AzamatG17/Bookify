using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceGroupId",
                table: "Service",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServiceGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Service_ServiceGroupId",
                table: "Service",
                column: "ServiceGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ServiceGroup_ServiceGroupId",
                table: "Service",
                column: "ServiceGroupId",
                principalTable: "ServiceGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_ServiceGroup_ServiceGroupId",
                table: "Service");

            migrationBuilder.DropTable(
                name: "ServiceGroup");

            migrationBuilder.DropIndex(
                name: "IX_Service_ServiceGroupId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ServiceGroupId",
                table: "Service");
        }
    }
}
