using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class adddisqualificationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disqualified",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DisqualifiedComment",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "disqualificationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StudentId = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    DisqualificationRequestStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disqualificationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_disqualificationRequests_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_disqualificationRequests_StudentId",
                table: "disqualificationRequests",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "disqualificationRequests");

            migrationBuilder.DropColumn(
                name: "Disqualified",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "DisqualifiedComment",
                table: "ApplicationUsers");
        }
    }
}
