using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class updateteacherfrominttostringadlersub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_AdlerCardSubmissions_ApplicationUsers_TeacherId1",
            //     table: "AdlerCardSubmissions");

            // migrationBuilder.DropIndex(
            //     name: "IX_AdlerCardSubmissions_TeacherId1",
            //     table: "AdlerCardSubmissions");

            // migrationBuilder.DropColumn(
            //     name: "TeacherId1",
            //     table: "AdlerCardSubmissions");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "AdlerCardSubmissions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCardSubmissions_TeacherId",
                table: "AdlerCardSubmissions",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdlerCardSubmissions_ApplicationUsers_TeacherId",
                table: "AdlerCardSubmissions",
                column: "TeacherId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdlerCardSubmissions_ApplicationUsers_TeacherId",
                table: "AdlerCardSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_AdlerCardSubmissions_TeacherId",
                table: "AdlerCardSubmissions");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "AdlerCardSubmissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherId1",
                table: "AdlerCardSubmissions",
                type: "varchar(85)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCardSubmissions_TeacherId1",
                table: "AdlerCardSubmissions",
                column: "TeacherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AdlerCardSubmissions_ApplicationUsers_TeacherId1",
                table: "AdlerCardSubmissions",
                column: "TeacherId1",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
