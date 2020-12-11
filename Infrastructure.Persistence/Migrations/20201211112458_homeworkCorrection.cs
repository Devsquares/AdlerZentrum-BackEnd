using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class homeworkCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CorrectionDate",
                table: "HomeWorkSubmitions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrectionTeacherId",
                table: "HomeWorkSubmitions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "HomeWorkSubmitions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Solution",
                table: "HomeWorkSubmitions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorkSubmitions_CorrectionTeacherId",
                table: "HomeWorkSubmitions",
                column: "CorrectionTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeWorkSubmitions_ApplicationUsers_CorrectionTeacherId",
                table: "HomeWorkSubmitions",
                column: "CorrectionTeacherId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeWorkSubmitions_ApplicationUsers_CorrectionTeacherId",
                table: "HomeWorkSubmitions");

            migrationBuilder.DropIndex(
                name: "IX_HomeWorkSubmitions_CorrectionTeacherId",
                table: "HomeWorkSubmitions");

            migrationBuilder.DropColumn(
                name: "CorrectionDate",
                table: "HomeWorkSubmitions");

            migrationBuilder.DropColumn(
                name: "CorrectionTeacherId",
                table: "HomeWorkSubmitions");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "HomeWorkSubmitions");

            migrationBuilder.DropColumn(
                name: "Solution",
                table: "HomeWorkSubmitions");
        }
    }
}
