using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class CompleteQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropColumn(
            //     name: "ReportDateTime",
            //     table: "LessonInstanceStudents");

            migrationBuilder.AddColumn<string>(
                name: "CompleteWords",
                table: "SingleQuestionSubmissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompleteWordsCorrections",
                table: "SingleQuestionSubmissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrectionText",
                table: "SingleQuestionSubmissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteWords",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "CompleteWordsCorrections",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "CorrectionText",
                table: "SingleQuestionSubmissions");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportDateTime",
                table: "LessonInstanceStudents",
                type: "datetime",
                nullable: true);
        }
    }
}
