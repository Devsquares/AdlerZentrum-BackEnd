using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addgroupdefinitionIdpromocodeinstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupDefinitionId",
                table: "PromoCodeInstances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodeInstances_GroupDefinitionId",
                table: "PromoCodeInstances",
                column: "GroupDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodeInstances_GroupDefinition_GroupDefinitionId",
                table: "PromoCodeInstances",
                column: "GroupDefinitionId",
                principalTable: "GroupDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodeInstances_GroupDefinition_GroupDefinitionId",
                table: "PromoCodeInstances");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodeInstances_GroupDefinitionId",
                table: "PromoCodeInstances");

            migrationBuilder.DropColumn(
                name: "GroupDefinitionId",
                table: "PromoCodeInstances");
        }
    }
}
