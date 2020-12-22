using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class editBouns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionType_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionType");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TrueOrFalse",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "Serail",
                table: "GroupInstances");

            migrationBuilder.AddColumn<bool>(
                name: "AnswerIsTrueOrFalse",
                table: "SingleQuestions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "LessonInstances",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BonusPointsStatus",
                table: "Homeworks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Homeworks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "GroupInstances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_TeacherId",
                table: "Homeworks",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_ApplicationUsers_TeacherId",
                table: "Homeworks",
                column: "TeacherId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_ApplicationUsers_TeacherId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_TeacherId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "AnswerIsTrueOrFalse",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "BonusPointsStatus",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "GroupInstances");

            migrationBuilder.AddColumn<short>(
                name: "TrueOrFalse",
                table: "SingleQuestions",
                type: "bit",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Homeworks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Serail",
                table: "GroupInstances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionType_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId",
                principalTable: "QuestionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
