using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServiceRatingConncetionToBookingAndEticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRating_Booking_BookingId",
                table: "ServiceRating");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRating_ETicket_ETicketId",
                table: "ServiceRating");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRating_BookingId",
                table: "ServiceRating");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRating_ETicketId",
                table: "ServiceRating");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRating_BookingId",
                table: "ServiceRating",
                column: "BookingId",
                unique: true,
                filter: "[BookingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRating_ETicketId",
                table: "ServiceRating",
                column: "ETicketId",
                unique: true,
                filter: "[ETicketId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRating_Booking_BookingId",
                table: "ServiceRating",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRating_ETicket_ETicketId",
                table: "ServiceRating",
                column: "ETicketId",
                principalTable: "ETicket",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRating_Booking_BookingId",
                table: "ServiceRating");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRating_ETicket_ETicketId",
                table: "ServiceRating");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRating_BookingId",
                table: "ServiceRating");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRating_ETicketId",
                table: "ServiceRating");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRating_BookingId",
                table: "ServiceRating",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRating_ETicketId",
                table: "ServiceRating",
                column: "ETicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRating_Booking_BookingId",
                table: "ServiceRating",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRating_ETicket_ETicketId",
                table: "ServiceRating",
                column: "ETicketId",
                principalTable: "ETicket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
