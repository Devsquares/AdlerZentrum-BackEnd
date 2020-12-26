using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class LessonDefinitionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "TestInstance",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TestInstance_StudentId",
                table: "TestInstance",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestInstance_ApplicationUsers_StudentId",
                table: "TestInstance",
                column: "StudentId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestInstance_ApplicationUsers_StudentId",
                table: "TestInstance");

            migrationBuilder.DropIndex(
                name: "IX_TestInstance_StudentId",
                table: "TestInstance");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "TestInstance",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
