using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class isFinalSubLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_TestInstances_LessonInstances_LessonInstanceId",
            //    table: "TestInstances");

            //migrationBuilder.DropColumn(
            //    name: "NumberOfSolts",
            //    table: "GroupCondition");

            //migrationBuilder.AlterColumn<int>(
            //    name: "LessonInstanceId",
            //    table: "TestInstances",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsFinal",
            //    table: "SubLevels",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<int>(
            //    name: "NumberOfSlots",
            //    table: "GroupCondition",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TestInstances_LessonInstances_LessonInstanceId",
            //    table: "TestInstances",
            //    column: "LessonInstanceId",
            //    principalTable: "LessonInstances",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestInstances_LessonInstances_LessonInstanceId",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "IsFinal",
                table: "SubLevels");

            migrationBuilder.DropColumn(
                name: "NumberOfSlots",
                table: "GroupCondition");

            migrationBuilder.AlterColumn<int>(
                name: "LessonInstanceId",
                table: "TestInstances",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSolts",
                table: "GroupCondition",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TestInstances_LessonInstances_LessonInstanceId",
                table: "TestInstances",
                column: "LessonInstanceId",
                principalTable: "LessonInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
