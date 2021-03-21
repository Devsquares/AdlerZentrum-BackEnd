using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class Grading2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubLevels_SubLevels_NextSublevelId",
                table: "SubLevels");

            migrationBuilder.DropIndex(
                name: "IX_SubLevels_NextSublevelId",
                table: "SubLevels");

            migrationBuilder.DropColumn(
                name: "NextSublevelId",
                table: "SubLevels");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "SubLevels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "SubLevels");

            migrationBuilder.AddColumn<int>(
                name: "NextSublevelId",
                table: "SubLevels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubLevels_NextSublevelId",
                table: "SubLevels",
                column: "NextSublevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubLevels_SubLevels_NextSublevelId",
                table: "SubLevels",
                column: "NextSublevelId",
                principalTable: "SubLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
