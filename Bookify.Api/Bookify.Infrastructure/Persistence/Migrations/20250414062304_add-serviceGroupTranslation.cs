using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addserviceGroupTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_ServiceGroup_ServiceGroupId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_ServiceGroupId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ServiceGroupId",
                table: "Service");

            migrationBuilder.CreateTable(
                name: "ServiceGroupServices",
                columns: table => new
                {
                    ServiceGroupsId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGroupServices", x => new { x.ServiceGroupsId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_ServiceGroupServices_ServiceGroup_ServiceGroupsId",
                        column: x => x.ServiceGroupsId,
                        principalTable: "ServiceGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceGroupServices_Service_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceGroupTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ServiceGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGroupTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceGroupTranslation_ServiceGroup_ServiceGroupId",
                        column: x => x.ServiceGroupId,
                        principalTable: "ServiceGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceGroupServices_ServicesId",
                table: "ServiceGroupServices",
                column: "ServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceGroupTranslation_ServiceGroupId",
                table: "ServiceGroupTranslation",
                column: "ServiceGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceGroupServices");

            migrationBuilder.DropTable(
                name: "ServiceGroupTranslation");

            migrationBuilder.AddColumn<int>(
                name: "ServiceGroupId",
                table: "Service",
                type: "int",
                nullable: true);

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
    }
}
