using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AnswerQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_SingleQuestions_ChoiceAnswer_AnswerId1",
            //     table: "SingleQuestions");

            migrationBuilder.DropTable(
                name: "ChoiceAnswer");

            migrationBuilder.DropIndex(
                name: "IX_SingleQuestions_AnswerId1",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "AnswerId1",
                table: "SingleQuestions");

            migrationBuilder.AddColumn<bool>(
                name: "TrueOrFalse",
                table: "SingleQuestions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Choice",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrueOrFalse",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Choice");

            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "SingleQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnswerId1",
                table: "SingleQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChoiceAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    SingleQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoiceAnswer", x => x.Id);
                });

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
    }
}
