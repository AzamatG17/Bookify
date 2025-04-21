using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingServiceNullableBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Service_ServiceId",
                table: "Booking");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Service_ServiceId",
                table: "Booking",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Service_ServiceId",
                table: "Booking");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Service_ServiceId",
                table: "Booking",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id");
        }
    }
}
