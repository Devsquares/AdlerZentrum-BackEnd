using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addgroupconditionpromoCodestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "groupConditionPromoCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GroupConditionDetailsId = table.Column<int>(nullable: false),
                    PromoCodeId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true)
                   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groupConditionPromoCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_groupConditionPromoCodes_GroupConditionDetails_GroupConditio~",
                        column: x => x.GroupConditionDetailsId,
                        principalTable: "GroupConditionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_groupConditionPromoCodes_PromoCodes_PromoCodeId",
                        column: x => x.PromoCodeId,
                        principalTable: "PromoCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_groupConditionPromoCodes_GroupConditionDetailsId",
                table: "groupConditionPromoCodes",
                column: "GroupConditionDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_groupConditionPromoCodes_PromoCodeId",
                table: "groupConditionPromoCodes",
                column: "PromoCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "groupConditionPromoCodes");
        }
    }
}
