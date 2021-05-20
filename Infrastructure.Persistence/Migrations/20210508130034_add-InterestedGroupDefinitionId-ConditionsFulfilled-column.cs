using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addInterestedGroupDefinitionIdConditionsFulfilledcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupDefinitionId",
                table: "GroupInstanceStudents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterestedGroupDefinitionId",
                table: "GroupInstanceStudents",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConditionsFulfilled",
                table: "GroupDefinition",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_GroupInstanceStudents_GroupDefinitionId",
                table: "GroupInstanceStudents",
                column: "GroupDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInstanceStudents_GroupDefinition_GroupDefinitionId",
                table: "GroupInstanceStudents",
                column: "GroupDefinitionId",
                principalTable: "GroupDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInstanceStudents_GroupDefinition_GroupDefinitionId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupInstanceStudents_GroupDefinitionId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "GroupDefinitionId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "InterestedGroupDefinitionId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropColumn(
                name: "ConditionsFulfilled",
                table: "GroupDefinition");
        }
    }
}
