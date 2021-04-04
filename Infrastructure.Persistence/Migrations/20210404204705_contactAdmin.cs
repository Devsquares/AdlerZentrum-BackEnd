using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class contactAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "MailJobs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "MailJobs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "MailJobs");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "MailJobs");
        }
    }
}
