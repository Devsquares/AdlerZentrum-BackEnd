using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class jobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleQuestionSubmissions_Questions_QuestionId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_SingleQuestionSubmissions_QuestionId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<bool>(
                name: "ManualCorrection",
                table: "TestInstances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TestInstanceId",
                table: "SingleQuestionSubmissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    TestInstanceId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestionSubmissions_TestInstanceId",
                table: "SingleQuestionSubmissions",
                column: "TestInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestionSubmissions_TestInstances_TestInstanceId",
                table: "SingleQuestionSubmissions",
                column: "TestInstanceId",
                principalTable: "TestInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleQuestionSubmissions_TestInstances_TestInstanceId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_SingleQuestionSubmissions_TestInstanceId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "ManualCorrection",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "TestInstanceId",
                table: "SingleQuestionSubmissions");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "SingleQuestionSubmissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "ApplicationUsers",
                type: "text",
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
        }
    }
}
