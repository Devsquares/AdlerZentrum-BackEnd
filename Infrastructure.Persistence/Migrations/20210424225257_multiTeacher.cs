using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class multiTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeacherGroupInstances_GroupInstanceId",
                table: "TeacherGroupInstances");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGroupInstances_GroupInstanceId",
                table: "TeacherGroupInstances",
                column: "GroupInstanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeacherGroupInstances_GroupInstanceId",
                table: "TeacherGroupInstances");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGroupInstances_GroupInstanceId",
                table: "TeacherGroupInstances",
                column: "GroupInstanceId",
                unique: true);
        }
    }
}
