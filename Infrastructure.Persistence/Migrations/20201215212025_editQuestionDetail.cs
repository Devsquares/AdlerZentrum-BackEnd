using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class editQuestionDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleQuestions_SingleQuestionType_SingleQuestionTypeId",
                table: "SingleQuestions");

            migrationBuilder.DropTable(
                name: "SingleQuestionType");

            migrationBuilder.DropIndex(
                name: "IX_SingleQuestions_SingleQuestionTypeId",
                table: "SingleQuestions");

            migrationBuilder.DropColumn(
                name: "SingleQuestionTypeId",
                table: "SingleQuestions");

            migrationBuilder.AddColumn<int>(
                name: "SingleQuestionType",
                table: "SingleQuestions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SingleQuestionType",
                table: "SingleQuestions");

            migrationBuilder.AddColumn<int>(
                name: "SingleQuestionTypeId",
                table: "SingleQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SingleQuestionType",
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
                    table.PrimaryKey("PK_SingleQuestionType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestions_SingleQuestionTypeId",
                table: "SingleQuestions",
                column: "SingleQuestionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleQuestions_SingleQuestionType_SingleQuestionTypeId",
                table: "SingleQuestions",
                column: "SingleQuestionTypeId",
                principalTable: "SingleQuestionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
