using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class TestSub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestType_TestTypeId",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "QuestionDetails");

            migrationBuilder.DropTable(
                name: "TestType");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TestTypeId",
                table: "Tests");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "SingleQuestions",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NoOfRepeats",
                table: "Questions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MinCharacters",
                table: "Questions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "LessonInstances",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestStatus",
                table: "LessonInstances",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestInstance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LessonInstanceId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestInstance_LessonInstances_LessonInstanceId",
                        column: x => x.LessonInstanceId,
                        principalTable: "LessonInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestions_QuestionId",
                table: "SingleQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstances_TestId",
                table: "LessonInstances",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestInstance_LessonInstanceId",
                table: "TestInstance",
                column: "LessonInstanceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonInstances_Tests_TestId",
                table: "LessonInstances",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestions_Questions_QuestionId",
                table: "SingleQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonInstances_Tests_TestId",
                table: "LessonInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleQuestions_Questions_QuestionId",
                table: "SingleQuestions");

            migrationBuilder.DropTable(
                name: "TestInstance");

            migrationBuilder.DropIndex(
                name: "IX_SingleQuestions_QuestionId",
                table: "SingleQuestions");

            migrationBuilder.DropIndex(
                name: "IX_LessonInstances_TestId",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "TestStatus",
                table: "LessonInstances");

            migrationBuilder.AlterColumn<int>(
                name: "NoOfRepeats",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinCharacters",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "QuestionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SingleQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionDetails_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionDetails_SingleQuestions_SingleQuestionId",
                        column: x => x.SingleQuestionId,
                        principalTable: "SingleQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestType",
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
                    table.PrimaryKey("PK_TestType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestTypeId",
                table: "Tests",
                column: "TestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_QuestionId",
                table: "QuestionDetails",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_SingleQuestionId",
                table: "QuestionDetails",
                column: "SingleQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestType_TestTypeId",
                table: "Tests",
                column: "TestTypeId",
                principalTable: "TestType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
