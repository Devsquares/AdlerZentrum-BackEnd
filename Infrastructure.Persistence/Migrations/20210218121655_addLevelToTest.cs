using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addLevelToTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_LessonDefinition_LessonDefinitionId",
                table: "Tests");

            migrationBuilder.AlterColumn<int>(
                name: "LessonDefinitionId",
                table: "Tests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SublevelId",
                table: "Tests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_LevelId",
                table: "Tests",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_SublevelId",
                table: "Tests",
                column: "SublevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_LessonDefinition_LessonDefinitionId",
                table: "Tests",
                column: "LessonDefinitionId",
                principalTable: "LessonDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Levels_LevelId",
                table: "Tests",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_SubLevels_SublevelId",
                table: "Tests",
                column: "SublevelId",
                principalTable: "SubLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_LessonDefinition_LessonDefinitionId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Levels_LevelId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_SubLevels_SublevelId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_LevelId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_SublevelId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "SublevelId",
                table: "Tests");

            migrationBuilder.AlterColumn<int>(
                name: "LessonDefinitionId",
                table: "Tests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_LessonDefinition_LessonDefinitionId",
                table: "Tests",
                column: "LessonDefinitionId",
                principalTable: "LessonDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
