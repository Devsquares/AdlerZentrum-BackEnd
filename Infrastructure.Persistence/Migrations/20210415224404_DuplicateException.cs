using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class DuplicateException : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_teacherAbsences_LessonInstances_LessonInstanceId",
                table: "teacherAbsences");

            migrationBuilder.DropForeignKey(
                name: "FK_teacherAbsences_ApplicationUsers_TeacherId",
                table: "teacherAbsences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_teacherAbsences",
                table: "teacherAbsences");

            migrationBuilder.RenameTable(
                name: "teacherAbsences",
                newName: "TeacherAbsences");

            migrationBuilder.RenameIndex(
                name: "IX_teacherAbsences_TeacherId",
                table: "TeacherAbsences",
                newName: "IX_TeacherAbsences_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_teacherAbsences_LessonInstanceId",
                table: "TeacherAbsences",
                newName: "IX_TeacherAbsences_LessonInstanceId");

            // migrationBuilder.AddColumn<bool>(
            //     name: "DelaySeen",
            //     table: "TestInstances",
            //     nullable: false,
            //     defaultValue: false);

            // migrationBuilder.AddColumn<bool>(
            //     name: "DelaySeen",
            //     table: "LessonInstances",
            //     nullable: false,
            //     defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "LessonInstances",
                nullable: true);

            // migrationBuilder.AddColumn<bool>(
            //     name: "DelaySeen",
            //     table: "HomeWorkSubmitions",
            //     nullable: false,
            //     defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherAbsences",
                table: "TeacherAbsences",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DuplicateExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
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

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAbsences_LessonInstances_LessonInstanceId",
                table: "TeacherAbsences",
                column: "LessonInstanceId",
                principalTable: "LessonInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAbsences_ApplicationUsers_TeacherId",
                table: "TeacherAbsences",
                column: "TeacherId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAbsences_LessonInstances_LessonInstanceId",
                table: "TeacherAbsences");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAbsences_ApplicationUsers_TeacherId",
                table: "TeacherAbsences");

            migrationBuilder.DropTable(
                name: "DuplicateExceptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherAbsences",
                table: "TeacherAbsences");

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
                newName: "teacherAbsences");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAbsences_TeacherId",
                table: "teacherAbsences",
                newName: "IX_teacherAbsences_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAbsences_LessonInstanceId",
                table: "teacherAbsences",
                newName: "IX_teacherAbsences_LessonInstanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_teacherAbsences",
                table: "teacherAbsences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_teacherAbsences_LessonInstances_LessonInstanceId",
                table: "teacherAbsences",
                column: "LessonInstanceId",
                principalTable: "LessonInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teacherAbsences_ApplicationUsers_TeacherId",
                table: "teacherAbsences",
                column: "TeacherId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
