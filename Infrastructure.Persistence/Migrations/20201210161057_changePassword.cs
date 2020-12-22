using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class changePassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            // migrationBuilder.AddColumn<string>(
            //     name: "Color",
            //     table: "SubLevels",
            //     nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ChangePassword",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "SubLevels");

            migrationBuilder.DropColumn(
                name: "ChangePassword",
                table: "ApplicationUsers"); 
        }
    }
}
