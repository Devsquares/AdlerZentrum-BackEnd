using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class disqulified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disqualified",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "DisqualifiedComment",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Disqualified",
                table: "GroupInstanceStudents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DisqualifiedComment",
                table: "GroupInstanceStudents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisqualifiedUserId",
                table: "GroupInstanceStudents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disqualified",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "DisqualifiedComment",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "DisqualifiedUserId",
                table: "GroupInstanceStudents");

            migrationBuilder.AddColumn<short>(
                name: "Disqualified",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "DisqualifiedComment",
                table: "ApplicationUsers",
                type: "text",
                nullable: true);
        }
    }
}
