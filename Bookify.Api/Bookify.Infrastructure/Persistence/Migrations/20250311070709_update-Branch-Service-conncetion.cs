using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateBranchServiceconncetion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
