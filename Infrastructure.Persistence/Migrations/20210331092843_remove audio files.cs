using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class removeaudiofiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_ListeningAudioFile_AudioPathId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "ListeningAudioFile");

            migrationBuilder.DropIndex(
                name: "IX_Questions_AudioPathId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AudioPathId",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "AudioPath",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioPath",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "AudioPathId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ListeningAudioFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningAudioFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AudioPathId",
                table: "Questions",
                column: "AudioPathId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_ListeningAudioFile_AudioPathId",
                table: "Questions",
                column: "AudioPathId",
                principalTable: "ListeningAudioFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
