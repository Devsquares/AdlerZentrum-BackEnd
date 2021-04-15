using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addlessonIdTeacherGroupInstanceAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonInstanceId",
                table: "TeacherGroupInstances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGroupInstances_LessonInstanceId",
                table: "TeacherGroupInstances",
                column: "LessonInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherGroupInstances_LessonInstances_LessonInstanceId",
                table: "TeacherGroupInstances",
                column: "LessonInstanceId",
                principalTable: "LessonInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherGroupInstances_LessonInstances_LessonInstanceId",
                table: "TeacherGroupInstances");

            migrationBuilder.DropIndex(
                name: "IX_TeacherGroupInstances_LessonInstanceId",
                table: "TeacherGroupInstances");

            migrationBuilder.DropColumn(
                name: "LessonInstanceId",
                table: "TeacherGroupInstances");
        }
    }
}
