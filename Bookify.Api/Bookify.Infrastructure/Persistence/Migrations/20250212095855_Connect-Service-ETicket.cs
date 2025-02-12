using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConnectServiceETicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ETicket_ServiceId",
                table: "ETicket",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ETicket_Service_ServiceId",
                table: "ETicket",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ETicket_Service_ServiceId",
                table: "ETicket");

            migrationBuilder.DropIndex(
                name: "IX_ETicket_ServiceId",
                table: "ETicket");
        }
    }
}
