using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class Grading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextSublevelId",
                table: "SubLevels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEligible",
                table: "GroupInstanceStudents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Succeeded",
                table: "GroupInstanceStudents",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsEligible",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "Succeeded",
                table: "GroupInstanceStudents");
        }
    }
}
