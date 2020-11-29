using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addGroupConditioncloumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfSlotsWithPlacementTest",
                table: "GroupCondition",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSolts",
                table: "GroupCondition",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfSlotsWithPlacementTest",
                table: "GroupCondition");

            migrationBuilder.DropColumn(
                name: "NumberOfSolts",
                table: "GroupCondition");
        }
    }
}
