using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addteacherabsencetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeacherAbsences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    TeacherId = table.Column<string>(maxLength: 256, nullable: true),
                    LessonInstanceId = table.Column<int>(nullable: false),
                    IsEmergency = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacherAbsences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teacherAbsences_LessonInstances_LessonInstanceId",
                        column: x => x.LessonInstanceId,
                        principalTable: "LessonInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacherAbsences_ApplicationUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teacherAbsences_LessonInstanceId",
                table: "TeacherAbsences",
                column: "LessonInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_teacherAbsences_TeacherId",
                table: "TeacherAbsences",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherAbsences");
        }
    }
}
