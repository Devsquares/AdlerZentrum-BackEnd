using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class LessonInstanceStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Maybe this line not work,you can comment it.
            migrationBuilder.DropForeignKey(
                name: "FK_LessonInstances_Homeworks_HomeworkId",
                table: "LessonInstances");

            migrationBuilder.AlterColumn<int>(
                name: "HomeworkId",
                table: "LessonInstances",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "LessonInstanceStudent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LessonInstanceId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: true),
                    Attend = table.Column<bool>(nullable: false),
                    Homework = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonInstanceStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonInstanceStudent_LessonInstances_LessonInstanceId",
                        column: x => x.LessonInstanceId,
                        principalTable: "LessonInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonInstanceStudent_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstanceStudent_LessonInstanceId",
                table: "LessonInstanceStudent",
                column: "LessonInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstanceStudent_StudentId",
                table: "LessonInstanceStudent",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonInstances_Homeworks_HomeworkId",
                table: "LessonInstances",
                column: "HomeworkId",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonInstances_Homeworks_HomeworkId",
                table: "LessonInstances");

            migrationBuilder.DropTable(
                name: "LessonInstanceStudent");

            migrationBuilder.AlterColumn<int>(
                name: "HomeworkId",
                table: "LessonInstances",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonInstances_Homeworks_HomeworkId",
                table: "LessonInstances",
                column: "HomeworkId",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
