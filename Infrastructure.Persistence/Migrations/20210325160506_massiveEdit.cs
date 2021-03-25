using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class massiveEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<bool>(
            //     name: "IsEligible",
            //     table: "OverPaymentStudents",
            //     nullable: false,
            //     defaultValue: false);

            // migrationBuilder.AddColumn<bool>(
            //     name: "LastLesson",
            //     table: "LessonDefinition",
            //     nullable: false,
            //     defaultValue: false);

            // migrationBuilder.AlterColumn<int>(
            //     name: "TestInstanceId",
            //     table: "Jobs",
            //     nullable: true,
            //     oldClrType: typeof(int),
            //     oldType: "int");

            // migrationBuilder.AddColumn<DateTime>(
            //     name: "ExecutionDate",
            //     table: "Jobs",
            //     nullable: true);

            // migrationBuilder.AddColumn<DateTime>(
            //     name: "FinishDate",
            //     table: "Jobs",
            //     nullable: true);

            // migrationBuilder.AddColumn<int>(
            //     name: "GroupInstanceId",
            //     table: "Jobs",
            //     nullable: true);

            // migrationBuilder.AddColumn<DateTime>(
            //     name: "StartDate",
            //     table: "Jobs",
            //     nullable: true);

            // // migrationBuilder.AddColumn<bool>(
            // //     name: "IsEligible",
            // //     table: "InterestedStudents",
            // //     nullable: false,
            // //     defaultValue: false);

            // migrationBuilder.CreateTable(
            //     name: "Settings",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(nullable: false)
            //             .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
            //         CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
            //         CreatedDate = table.Column<DateTime>(nullable: true),
            //         LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
            //         LastModifiedDate = table.Column<DateTime>(nullable: true),
            //         PlacementA1 = table.Column<double>(nullable: false),
            //         PlacementA2 = table.Column<double>(nullable: false),
            //         PlacementB1 = table.Column<double>(nullable: false),
            //         PlacementB2 = table.Column<double>(nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Settings", x => x.Id);
            //     });
            migrationBuilder.CreateTable(
                name: "StudentInfos",
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
                    table.PrimaryKey("PK_StudentInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentInfos_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentInfos_SubLevels_SublevelId",
                        column: x => x.SublevelId,
                        principalTable: "SubLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfos_StudentId",
                table: "StudentInfos",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfos_SublevelId",
                table: "StudentInfos",
                column: "SublevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "StudentInfos");

            migrationBuilder.DropColumn(
                name: "IsEligible",
                table: "OverPaymentStudents");

            migrationBuilder.DropColumn(
                name: "LastLesson",
                table: "LessonDefinition");

            migrationBuilder.DropColumn(
                name: "ExecutionDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "FinishDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "GroupInstanceId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IsEligible",
                table: "InterestedStudents");

            migrationBuilder.AlterColumn<int>(
                name: "TestInstanceId",
                table: "Jobs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
