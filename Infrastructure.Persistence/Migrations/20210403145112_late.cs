using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class late : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CorrectionDate",
                table: "TestInstances",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CorrectionDueDate",
                table: "TestInstances",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "LessonInstances",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmittedReportTeacherId",
                table: "LessonInstances",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CorrectionDueDate",
                table: "HomeWorkSubmitions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstances_SubmittedReportTeacherId",
                table: "LessonInstances",
                column: "SubmittedReportTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonInstances_ApplicationUsers_SubmittedReportTeacherId",
                table: "LessonInstances",
                column: "SubmittedReportTeacherId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonInstances_ApplicationUsers_SubmittedReportTeacherId",
                table: "LessonInstances");

            migrationBuilder.DropIndex(
                name: "IX_LessonInstances_SubmittedReportTeacherId",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "CorrectionDate",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "CorrectionDueDate",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "SubmittedReportTeacherId",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "CorrectionDueDate",
                table: "HomeWorkSubmitions");
        }
    }
}
