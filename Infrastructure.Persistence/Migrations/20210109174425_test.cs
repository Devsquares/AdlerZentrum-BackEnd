using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_ChoiceSubmissions_Choice_ChoiceId",
            //     table: "ChoiceSubmissions");

            // migrationBuilder.DropIndex(
            //     name: "IX_ChoiceSubmissions_ChoiceId",
            //     table: "ChoiceSubmissions");

            // migrationBuilder.DropColumn(
            //     name: "ChoiceId",
            //     table: "ChoiceSubmissions");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "TestInstances",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            // migrationBuilder.AddColumn<bool>(
            //     name: "Corrected",
            //     table: "SingleQuestionSubmissions",
            //     nullable: false,
                // defaultValue: false);

            // migrationBuilder.AddColumn<bool>(
            //     name: "RightAnswer",
            //     table: "SingleQuestionSubmissions",
            //     nullable: false,
            //     defaultValue: false);

            // migrationBuilder.AddColumn<int>(
            //     name: "Points",
            //     table: "SingleQuestions",
            //     nullable: false,
            //     defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChoiceSubmissionId",
                table: "ChoiceSubmissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TestInstances_StudentId",
                table: "TestInstances",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestInstances_ApplicationUsers_StudentId",
                table: "TestInstances",
                column: "StudentId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestInstances_ApplicationUsers_StudentId",
                table: "TestInstances");

            migrationBuilder.DropIndex(
                name: "IX_TestInstances_StudentId",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "Corrected",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "RightAnswer",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "ChoiceSubmissionId",
                table: "ChoiceSubmissions");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "TestInstances",
                type: "text",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "ChoiceId",
                table: "ChoiceSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceSubmissions_ChoiceId",
                table: "ChoiceSubmissions",
                column: "ChoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChoiceSubmissions_Choice_ChoiceId",
                table: "ChoiceSubmissions",
                column: "ChoiceId",
                principalTable: "Choice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
