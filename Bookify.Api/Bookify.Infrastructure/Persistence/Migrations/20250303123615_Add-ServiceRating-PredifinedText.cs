using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceRatingPredifinedText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PredefinedText",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedText", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    TicketNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeskNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeskName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PredefinedTextId = table.Column<int>(type: "int", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    ETicketId = table.Column<int>(type: "int", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRating_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRating_ETicket_ETicketId",
                        column: x => x.ETicketId,
                        principalTable: "ETicket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRating_PredefinedText_PredefinedTextId",
                        column: x => x.PredefinedTextId,
                        principalTable: "PredefinedText",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceRating_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceRating_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRating_BookingId",
                table: "ServiceRating",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRating_ETicketId",
                table: "ServiceRating",
                column: "ETicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRating_PredefinedTextId",
                table: "ServiceRating",
                column: "PredefinedTextId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRating_ServiceId",
                table: "ServiceRating",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRating_UserId",
                table: "ServiceRating",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRating");

            migrationBuilder.DropTable(
                name: "PredefinedText");
        }
    }
}
