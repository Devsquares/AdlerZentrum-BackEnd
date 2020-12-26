﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddLessonDefinitionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonDefinitionId",
                table: "Tests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_LessonDefinitionId",
                table: "Tests",
                column: "LessonDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_LessonDefinition_LessonDefinitionId",
                table: "Tests",
                column: "LessonDefinitionId",
                principalTable: "LessonDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_LessonDefinition_LessonDefinitionId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_LessonDefinitionId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "LessonDefinitionId",
                table: "Tests");
        }
    }
}
