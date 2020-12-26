using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addtesttoTestInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "TestInstance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TestInstance_TestId",
                table: "TestInstance",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestInstance_Tests_TestId",
                table: "TestInstance",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestInstance_Tests_TestId",
                table: "TestInstance");

            migrationBuilder.DropIndex(
                name: "IX_TestInstance_TestId",
                table: "TestInstance");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "TestInstance");
        }
    }
}
