using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class editgroupinstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropIndex(
            //     name: "IX_TeacherGroupInstances_GroupInstanceId",
            //     table: "TeacherGroupInstances");

            //migrationBuilder.AddColumn<int>(
            //    name: "LessonDefinitionId",
            //    table: "Tests",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tests_LessonDefinitionId",
            //    table: "Tests",
            //    column: "LessonDefinitionId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeacherGroupInstances_GroupInstanceId",
            //    table: "TeacherGroupInstances",
            //    column: "GroupInstanceId",
            //    unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Tests_LessonDefinition_LessonDefinitionId",
            //    table: "Tests",
            //    column: "LessonDefinitionId",
            //    principalTable: "LessonDefinition",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_LessonDefinition_LessonDefinitionId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_LessonDefinitionId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_TeacherGroupInstances_GroupInstanceId",
                table: "TeacherGroupInstances");

            migrationBuilder.DropColumn(
                name: "LessonDefinitionId",
                table: "Tests");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGroupInstances_GroupInstanceId",
                table: "TeacherGroupInstances",
                column: "GroupInstanceId");
        }
    }
}
