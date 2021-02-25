using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AdlerCardLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "AdlerCards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCards_LevelId",
                table: "AdlerCards",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdlerCards_Levels_LevelId",
                table: "AdlerCards",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdlerCards_Levels_LevelId",
                table: "AdlerCards");

            migrationBuilder.DropIndex(
                name: "IX_AdlerCards_LevelId",
                table: "AdlerCards");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "AdlerCards");
        }
    }
}
