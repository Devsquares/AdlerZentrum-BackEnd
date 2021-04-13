using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class adddelayseenlessoninstancetestinstancehomeworksub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DelaySeen",
                table: "TestInstances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DelaySeen",
                table: "LessonInstances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DelaySeen",
                table: "HomeWorkSubmitions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelaySeen",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "DelaySeen",
                table: "LessonInstances");

            migrationBuilder.DropColumn(
                name: "DelaySeen",
                table: "HomeWorkSubmitions");
        }
    }
}
