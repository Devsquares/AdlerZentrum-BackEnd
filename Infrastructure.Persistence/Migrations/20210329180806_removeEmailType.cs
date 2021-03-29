using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class removeEmailType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_EmailTemplates_EmailTypes_EmailTypeId",
            //     table: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "EmailTypes");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplates_EmailTypeId",
                table: "EmailTemplates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TypeName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_EmailTypeId",
                table: "EmailTemplates",
                column: "EmailTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplates_EmailTypes_EmailTypeId",
                table: "EmailTemplates",
                column: "EmailTypeId",
                principalTable: "EmailTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
