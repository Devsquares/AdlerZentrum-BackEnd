using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class SubmittedReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SingleQuestions_Questions_QuestionId",
            //    table: "SingleQuestions");

            migrationBuilder.DropIndex(
                name: "IX_SingleQuestions_QuestionId",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "SingleQuestions");

            migrationBuilder.AddColumn<bool>(
                name: "SubmittedReport",
                table: "LessonInstances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_QuestionId",
                table: "QuestionDetails",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_SingleQuestionId",
                table: "QuestionDetails",
                column: "SingleQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionDetails_Questions_QuestionId",
                table: "QuestionDetails",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionDetails_SingleQuestions_SingleQuestionId",
                table: "QuestionDetails",
                column: "SingleQuestionId",
                principalTable: "SingleQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionDetails_Questions_QuestionId",
                table: "QuestionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionDetails_SingleQuestions_SingleQuestionId",
                table: "QuestionDetails");

            migrationBuilder.DropIndex(
                name: "IX_QuestionDetails_QuestionId",
                table: "QuestionDetails");

            migrationBuilder.DropIndex(
                name: "IX_QuestionDetails_SingleQuestionId",
                table: "QuestionDetails");

            migrationBuilder.DropColumn(
                name: "SubmittedReport",
                table: "LessonInstances");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "SingleQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestions_QuestionId",
                table: "SingleQuestions",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestions_Questions_QuestionId",
                table: "SingleQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
