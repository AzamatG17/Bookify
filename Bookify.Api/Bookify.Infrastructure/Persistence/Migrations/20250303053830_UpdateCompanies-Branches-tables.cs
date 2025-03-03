using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompaniesBranchestables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Projects",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "BaseUrl",
                table: "Companies",
                newName: "BaseUrlForOnlinet");

            migrationBuilder.AddColumn<string>(
                name: "BaseUrlForBookingService",
                table: "Companies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Projects",
                table: "Branch",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseUrlForBookingService",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Projects",
                table: "Branch");

            migrationBuilder.RenameColumn(
                name: "BaseUrlForOnlinet",
                table: "Companies",
                newName: "BaseUrl");

            migrationBuilder.AddColumn<int>(
                name: "Projects",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
