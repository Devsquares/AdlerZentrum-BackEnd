using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class assginTeacherToGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteWords",
                table: "SingleQuestionSubmissions");

            migrationBuilder.DropColumn(
                name: "CompleteWordsCorrections",
                table: "SingleQuestionSubmissions");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "SingleQuestionSubmissions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "SingleQuestionSubmissions");

            migrationBuilder.AddColumn<string>(
                name: "CompleteWords",
                table: "SingleQuestionSubmissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompleteWordsCorrections",
                table: "SingleQuestionSubmissions",
                type: "text",
                nullable: true);
        }
    }
}
