using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addcomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SingleQuestionSubmissions_SingleQuestions_SingleQuestionId",
            //    table: "SingleQuestionSubmissions");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "TestInstances",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "SingleQuestionId",
                table: "SingleQuestionSubmissions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "SingleQuestionSubmissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "PromoCodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "HomeWorkSubmitions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestionSubmissions_QuestionId",
                table: "SingleQuestionSubmissions",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestionSubmissions_Questions_QuestionId",
                table: "SingleQuestionSubmissions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestionSubmissions_SingleQuestions_SingleQuestionId",
                table: "SingleQuestionSubmissions",
                column: "SingleQuestionId",
                principalTable: "SingleQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleQuestionSubmissions_Questions_QuestionId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleQuestionSubmissions_SingleQuestions_SingleQuestionId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_SingleQuestionSubmissions_QuestionId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "HomeWorkSubmitions");

            migrationBuilder.AlterColumn<int>(
                name: "SingleQuestionId",
                table: "SingleQuestionSubmissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestionSubmissions_SingleQuestions_SingleQuestionId",
                table: "SingleQuestionSubmissions",
                column: "SingleQuestionId",
                principalTable: "SingleQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
