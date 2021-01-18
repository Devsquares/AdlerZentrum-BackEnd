using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addisplacementtestpromocodeidgroupinstancestudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlacementTest",
                table: "GroupInstanceStudents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PromoCodeId",
                table: "GroupInstanceStudents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupInstanceStudents_PromoCodeId",
                table: "GroupInstanceStudents",
                column: "PromoCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInstanceStudents_PromoCodes_PromoCodeId",
                table: "GroupInstanceStudents",
                column: "PromoCodeId",
                principalTable: "PromoCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInstanceStudents_PromoCodes_PromoCodeId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupInstanceStudents_PromoCodeId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "IsPlacementTest",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "PromoCodeId",
                table: "GroupInstanceStudents");
        }
    }
}
