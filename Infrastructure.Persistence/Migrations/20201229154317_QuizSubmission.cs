using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class QuizSubmission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SingleQuestionSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    AnswerText = table.Column<string>(nullable: true),
                    TrueOrFalseSubmission = table.Column<bool>(nullable: false),
                    StudentId = table.Column<string>(nullable: true),
                    SingleQuestionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleQuestionSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleQuestionSubmissions_SingleQuestions_SingleQuestionId",
                        column: x => x.SingleQuestionId,
                        principalTable: "SingleQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SingleQuestionSubmissions_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChoiceSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    ChoiceId = table.Column<int>(nullable: false),
                    SingleQuestionSubmissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoiceSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChoiceSubmissions_Choice_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "Choice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoiceSubmissions_SingleQuestionSubmissions_SingleQuestionSu~",
                        column: x => x.SingleQuestionSubmissionId,
                        principalTable: "SingleQuestionSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceSubmissions_ChoiceId",
                table: "ChoiceSubmissions",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceSubmissions_SingleQuestionSubmissionId",
                table: "ChoiceSubmissions",
                column: "SingleQuestionSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestionSubmissions_SingleQuestionId",
                table: "SingleQuestionSubmissions",
                column: "SingleQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestionSubmissions_StudentId",
                table: "SingleQuestionSubmissions",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChoiceSubmissions");

            migrationBuilder.DropTable(
                name: "SingleQuestionSubmissions");
        }
    }
}
