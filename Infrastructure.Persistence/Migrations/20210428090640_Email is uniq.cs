using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class Emailisuniq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "DuplicateExceptions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DuplicateExceptions_Email",
                table: "DuplicateExceptions",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DuplicateExceptions_Email",
                table: "DuplicateExceptions");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "DuplicateExceptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
