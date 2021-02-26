using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addpercentssubleveles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinalTestpercent",
                table: "SubLevels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quizpercent",
                table: "SubLevels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SublevelTestpercent",
                table: "SubLevels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalTestpercent",
                table: "SubLevels");

            migrationBuilder.DropColumn(
                name: "Quizpercent",
                table: "SubLevels");

            migrationBuilder.DropColumn(
                name: "SublevelTestpercent",
                table: "SubLevels");
        }
    }
}
