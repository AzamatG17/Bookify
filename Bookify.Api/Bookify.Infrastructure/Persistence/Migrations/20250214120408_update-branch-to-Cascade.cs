using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatebranchtoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningTimeBranch_Branch_BranchId",
                table: "OpeningTimeBranch");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningTimeBranch_Branch_BranchId",
                table: "OpeningTimeBranch",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningTimeBranch_Branch_BranchId",
                table: "OpeningTimeBranch");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningTimeBranch_Branch_BranchId",
                table: "OpeningTimeBranch",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id");
        }
    }
}
