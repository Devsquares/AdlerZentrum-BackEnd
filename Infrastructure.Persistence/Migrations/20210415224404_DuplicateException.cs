using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class DuplicateException : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DuplicateExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuplicateExceptions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "DuplicateExceptions"); 

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "DelaySeen",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "DelaySeen",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "DelaySeen",
                table: "HomeWorkSubmitions");

            migrationBuilder.RenameTable(
                name: "TeacherAbsences",
                newName: "TeacherAbsences");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAbsences_TeacherId",
                table: "TeacherAbsences",
                newName: "IX_teacherAbsences_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAbsences_LessonInstanceId",
                table: "TeacherAbsences",
                newName: "IX_teacherAbsences_LessonInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_teacherAbsences_LessonInstances_LessonInstanceId",
                table: "teacherAbsences",
                column: "LessonInstanceId",
                principalTable: "LessonInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
