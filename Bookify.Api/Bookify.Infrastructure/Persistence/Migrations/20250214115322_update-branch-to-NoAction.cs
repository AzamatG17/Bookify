using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatebranchtoNoAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningTimeBranch_Branch_BranchId",
                table: "OpeningTimeBranch");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningTimeBranch_Branch_BranchId",
                table: "OpeningTimeBranch",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningTimeBranch_Branch_BranchId",
                table: "OpeningTimeBranch");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningTimeBranch_Branch_BranchId",
                table: "OpeningTimeBranch",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Branch_BranchId",
                table: "Service",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
