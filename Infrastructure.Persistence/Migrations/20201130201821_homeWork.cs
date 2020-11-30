using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class homeWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "TeacherGroupInstances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Homeworks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    MinCharacters = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    BonusPoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    SublevelId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonDefinition_SubLevels_SublevelId",
                        column: x => x.SublevelId,
                        principalTable: "SubLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    GroupInstanceId = table.Column<int>(nullable: false),
                    LessonDefinitionId = table.Column<int>(nullable: false),
                    MaterialDone = table.Column<int>(nullable: false),
                    MaterialToDo = table.Column<int>(nullable: false),
                    HomeworkId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonInstances_GroupInstances_GroupInstanceId",
                        column: x => x.GroupInstanceId,
                        principalTable: "GroupInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonInstances_Homeworks_HomeworkId",
                        column: x => x.HomeworkId,
                        principalTable: "Homeworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonInstances_LessonDefinition_LessonDefinitionId",
                        column: x => x.LessonDefinitionId,
                        principalTable: "LessonDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonDefinition_SublevelId",
                table: "LessonDefinition",
                column: "SublevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstances_GroupInstanceId",
                table: "LessonInstances",
                column: "GroupInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstances_HomeworkId",
                table: "LessonInstances",
                column: "HomeworkId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstances_LessonDefinitionId",
                table: "LessonInstances",
                column: "LessonDefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonInstances");

            migrationBuilder.DropTable(
                name: "Homeworks");

            migrationBuilder.DropTable(
                name: "LessonDefinition");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "TeacherGroupInstances");
        }
    }
}
