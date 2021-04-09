using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class SingleQuestionTestNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleQuestionSubmissions_TestInstances_TestInstanceId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.AlterColumn<int>(
                name: "TestInstanceId",
                table: "SingleQuestionSubmissions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestionSubmissions_TestInstances_TestInstanceId",
                table: "SingleQuestionSubmissions",
                column: "TestInstanceId",
                principalTable: "TestInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleQuestionSubmissions_TestInstances_TestInstanceId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.AlterColumn<int>(
                name: "TestInstanceId",
                table: "SingleQuestionSubmissions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestionSubmissions_TestInstances_TestInstanceId",
                table: "SingleQuestionSubmissions",
                column: "TestInstanceId",
                principalTable: "TestInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
