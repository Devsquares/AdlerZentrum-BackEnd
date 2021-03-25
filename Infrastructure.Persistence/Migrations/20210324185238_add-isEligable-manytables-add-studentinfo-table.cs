using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addisEligablemanytablesaddstudentinfotable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEligible",
                table: "OverPaymentStudents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEligible",
                table: "InterestedStudents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "studentInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StudentId = table.Column<string>(nullable: true),
                    SublevelSuccess = table.Column<bool>(nullable: false),
                    SublevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_studentInfos_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_studentInfos_SubLevels_SublevelId",
                        column: x => x.SublevelId,
                        principalTable: "SubLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_studentInfos_StudentId",
                table: "studentInfos",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_studentInfos_SublevelId",
                table: "studentInfos",
                column: "SublevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "studentInfos");

            migrationBuilder.DropColumn(
                name: "IsEligible",
                table: "OverPaymentStudents");

            migrationBuilder.DropColumn(
                name: "IsEligible",
                table: "InterestedStudents");
        }
    }
}
