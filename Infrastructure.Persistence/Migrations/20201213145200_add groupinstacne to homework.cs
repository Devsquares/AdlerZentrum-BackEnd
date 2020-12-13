using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addgroupinstacnetohomework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_GroupInstanceId",
                table: "Homeworks",
                column: "GroupInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_GroupInstances_GroupInstanceId",
                table: "Homeworks",
                column: "GroupInstanceId",
                principalTable: "GroupInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_GroupInstances_GroupInstanceId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_GroupInstanceId",
                table: "Homeworks");
        }
    }
}
