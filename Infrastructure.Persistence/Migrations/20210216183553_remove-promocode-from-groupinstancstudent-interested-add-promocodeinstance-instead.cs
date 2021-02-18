using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class removepromocodefromgroupinstancstudentinterestedaddpromocodeinstanceinstead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInstanceStudents_PromoCodes_PromoCodeId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestedStudents_PromoCodes_PromoCodeId",
                table: "InterestedStudents");

            migrationBuilder.DropIndex(
                name: "IX_InterestedStudents_PromoCodeId",
                table: "InterestedStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupInstanceStudents_PromoCodeId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "PromoCodeId",
                table: "InterestedStudents");

            migrationBuilder.DropColumn(
                name: "PromoCodeId",
                table: "GroupInstanceStudents");

            migrationBuilder.AddColumn<int>(
                name: "PromoCodeInstanceId",
                table: "InterestedStudents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PromoCodeInstanceId",
                table: "GroupInstanceStudents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterestedStudents_PromoCodeInstanceId",
                table: "InterestedStudents",
                column: "PromoCodeInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInstanceStudents_PromoCodeInstanceId",
                table: "GroupInstanceStudents",
                column: "PromoCodeInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInstanceStudents_promoCodeInstances_PromoCodeInstanceId",
                table: "GroupInstanceStudents",
                column: "PromoCodeInstanceId",
                principalTable: "promoCodeInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedStudents_promoCodeInstances_PromoCodeInstanceId",
                table: "InterestedStudents",
                column: "PromoCodeInstanceId",
                principalTable: "promoCodeInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInstanceStudents_promoCodeInstances_PromoCodeInstanceId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestedStudents_promoCodeInstances_PromoCodeInstanceId",
                table: "InterestedStudents");

            migrationBuilder.DropIndex(
                name: "IX_InterestedStudents_PromoCodeInstanceId",
                table: "InterestedStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupInstanceStudents_PromoCodeInstanceId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "PromoCodeInstanceId",
                table: "InterestedStudents");

            migrationBuilder.DropColumn(
                name: "PromoCodeInstanceId",
                table: "GroupInstanceStudents");

            migrationBuilder.AddColumn<int>(
                name: "PromoCodeId",
                table: "InterestedStudents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PromoCodeId",
                table: "GroupInstanceStudents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterestedStudents_PromoCodeId",
                table: "InterestedStudents",
                column: "PromoCodeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedStudents_PromoCodes_PromoCodeId",
                table: "InterestedStudents",
                column: "PromoCodeId",
                principalTable: "PromoCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
