using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addadlercardbundelstudenttable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdlerCardBundleStudents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StudentId = table.Column<string>(nullable: true),
                    AdlerCardsBundleId = table.Column<int>(nullable: false),
                    PurchasingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdlerCardBundleStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdlerCardBundleStudents_AdlerCardsBundles_AdlerCardsBundleId",
                        column: x => x.AdlerCardsBundleId,
                        principalTable: "AdlerCardsBundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdlerCardBundleStudents_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCardBundleStudents_AdlerCardsBundleId",
                table: "AdlerCardBundleStudents",
                column: "AdlerCardsBundleId");

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCardBundleStudents_StudentId",
                table: "AdlerCardBundleStudents",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdlerCardBundleStudents");
        }
    }
}
