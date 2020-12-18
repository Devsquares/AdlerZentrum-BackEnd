using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class fixSingleQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Choice_ChoiceAnswer_AnswerId",
            //     table: "Choice");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_ChoiceAnswer_SingleQuestions_SingleQuestionId",
            //     table: "ChoiceAnswer");

            migrationBuilder.DropIndex(
                name: "IX_ChoiceAnswer_SingleQuestionId",
                table: "ChoiceAnswer");

            migrationBuilder.DropIndex(
                name: "IX_Choice_AnswerId",
                table: "Choice");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Choice");

            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "SingleQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnswerId1",
                table: "SingleQuestions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestions_AnswerId1",
                table: "SingleQuestions",
                column: "AnswerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestions_ChoiceAnswer_AnswerId1",
                table: "SingleQuestions",
                column: "AnswerId1",
                principalTable: "ChoiceAnswer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleQuestions_ChoiceAnswer_AnswerId1",
                table: "SingleQuestions");

            migrationBuilder.DropIndex(
                name: "IX_SingleQuestions_AnswerId1",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "AnswerId1",
                table: "SingleQuestions");

            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "Choice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceAnswer_SingleQuestionId",
                table: "ChoiceAnswer",
                column: "SingleQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_AnswerId",
                table: "Choice",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Choice_ChoiceAnswer_AnswerId",
                table: "Choice",
                column: "AnswerId",
                principalTable: "ChoiceAnswer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChoiceAnswer_SingleQuestions_SingleQuestionId",
                table: "ChoiceAnswer",
                column: "SingleQuestionId",
                principalTable: "SingleQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
