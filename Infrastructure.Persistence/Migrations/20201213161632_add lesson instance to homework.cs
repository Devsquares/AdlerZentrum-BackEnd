using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addlessoninstancetohomework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_LessonInstances_Homeworks_HomeworkId",
               table: "LessonInstances");

            migrationBuilder.DropIndex(
                name: "IX_LessonInstances_HomeworkId",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "HomeworkId",
                table: "LessonInstances");

            migrationBuilder.AddColumn<int>(
                name: "LessonInstanceId",
                table: "Homeworks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_LessonInstanceId",
                table: "Homeworks",
                column: "LessonInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_LessonInstances_LessonInstanceId",
                table: "Homeworks",
                column: "LessonInstanceId",
                principalTable: "LessonInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_LessonInstances_LessonInstanceId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_LessonInstanceId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "LessonInstanceId",
                table: "Homeworks");

            migrationBuilder.AddColumn<int>(
                name: "HomeworkId",
                table: "LessonInstances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstances_HomeworkId",
                table: "LessonInstances",
                column: "HomeworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonInstances_Homeworks_HomeworkId",
                table: "LessonInstances",
                column: "HomeworkId",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
