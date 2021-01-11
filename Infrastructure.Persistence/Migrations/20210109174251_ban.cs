using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class ban : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BanComment",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Banned",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BanComment",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Banned",
                table: "ApplicationUsers");
        }
    }
}
