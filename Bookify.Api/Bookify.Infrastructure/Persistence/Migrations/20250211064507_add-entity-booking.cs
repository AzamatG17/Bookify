using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addentitybooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Booking",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Booking");
        }
    }
}
