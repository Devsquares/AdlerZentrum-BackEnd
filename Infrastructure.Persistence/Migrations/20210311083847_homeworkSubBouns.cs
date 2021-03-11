using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class homeworkSubBouns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PlacementStartDate",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalPoint",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BonusPoints",
                table: "HomeWorkSubmitions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TestInstances_GroupInstanceId",
                table: "TestInstances",
                column: "GroupInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestInstances_GroupInstances_GroupInstanceId",
                table: "TestInstances",
                column: "GroupInstanceId",
                principalTable: "GroupInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestInstances_GroupInstances_GroupInstanceId",
                table: "TestInstances");

            migrationBuilder.DropIndex(
                name: "IX_TestInstances_GroupInstanceId",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "PlacementStartDate",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TotalPoint",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "BonusPoints",
                table: "HomeWorkSubmitions");
        }
    }
}
