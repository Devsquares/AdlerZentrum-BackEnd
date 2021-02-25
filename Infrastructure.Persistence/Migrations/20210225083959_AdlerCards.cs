using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AdlerCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInstanceStudents_promoCodeInstances_PromoCodeInstanceId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestedStudents_promoCodeInstances_PromoCodeInstanceId",
                table: "InterestedStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_promoCodeInstances_PromoCodes_PromoCodeId",
                table: "promoCodeInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_promoCodeInstances_ApplicationUsers_StudentId",
                table: "promoCodeInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_promoCodeInstances",
                table: "promoCodeInstances");

            migrationBuilder.DropColumn(
                name: "PromoCodenstanceId",
                table: "GroupInstanceStudents");

            migrationBuilder.RenameTable(
                name: "promoCodeInstances",
                newName: "PromoCodeInstances");

            migrationBuilder.RenameIndex(
                name: "IX_promoCodeInstances_StudentId",
                table: "PromoCodeInstances",
                newName: "IX_PromoCodeInstances_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_promoCodeInstances_PromoCodeId",
                table: "PromoCodeInstances",
                newName: "IX_PromoCodeInstances_PromoCodeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdlerService",
                table: "Questions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromoCodeInstances",
                table: "PromoCodeInstances",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AdlerCardsBundles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    DiscountPrice = table.Column<double>(nullable: false),
                    AvailableFrom = table.Column<DateTime>(nullable: true),
                    AvailableTill = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdlerCardsBundles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdlerCardsUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    LevelId = table.Column<int>(nullable: false),
                    AdlerCardsTypeId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdlerCardsUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdlerCardsUnits_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdlerCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    AdlerCardsUnitId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    AllowedDuration = table.Column<int>(nullable: false),
                    TotalScore = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    AdlerCardsTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdlerCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdlerCards_AdlerCardsUnits_AdlerCardsUnitId",
                        column: x => x.AdlerCardsUnitId,
                        principalTable: "AdlerCardsUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdlerCards_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdlerCardSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    AdlerCardId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: true),
                    TeacherId1 = table.Column<string>(nullable: true),
                    TeacherId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    AchievedScore = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdlerCardSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdlerCardSubmissions_AdlerCards_AdlerCardId",
                        column: x => x.AdlerCardId,
                        principalTable: "AdlerCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdlerCardSubmissions_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdlerCardSubmissions_ApplicationUsers_TeacherId1",
                        column: x => x.TeacherId1,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCards_AdlerCardsUnitId",
                table: "AdlerCards",
                column: "AdlerCardsUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCards_QuestionId",
                table: "AdlerCards",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCardSubmissions_AdlerCardId",
                table: "AdlerCardSubmissions",
                column: "AdlerCardId");

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCardSubmissions_StudentId",
                table: "AdlerCardSubmissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCardSubmissions_TeacherId1",
                table: "AdlerCardSubmissions",
                column: "TeacherId1");

            migrationBuilder.CreateIndex(
                name: "IX_AdlerCardsUnits_LevelId",
                table: "AdlerCardsUnits",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInstanceStudents_PromoCodeInstances_PromoCodeInstanceId",
                table: "GroupInstanceStudents",
                column: "PromoCodeInstanceId",
                principalTable: "PromoCodeInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedStudents_PromoCodeInstances_PromoCodeInstanceId",
                table: "InterestedStudents",
                column: "PromoCodeInstanceId",
                principalTable: "PromoCodeInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodeInstances_PromoCodes_PromoCodeId",
                table: "PromoCodeInstances",
                column: "PromoCodeId",
                principalTable: "PromoCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodeInstances_ApplicationUsers_StudentId",
                table: "PromoCodeInstances",
                column: "StudentId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInstanceStudents_PromoCodeInstances_PromoCodeInstanceId",
                table: "GroupInstanceStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestedStudents_PromoCodeInstances_PromoCodeInstanceId",
                table: "InterestedStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodeInstances_PromoCodes_PromoCodeId",
                table: "PromoCodeInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodeInstances_ApplicationUsers_StudentId",
                table: "PromoCodeInstances");

            migrationBuilder.DropTable(
                name: "AdlerCardsBundles");

            migrationBuilder.DropTable(
                name: "AdlerCardSubmissions");

            migrationBuilder.DropTable(
                name: "AdlerCards");

            migrationBuilder.DropTable(
                name: "AdlerCardsUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromoCodeInstances",
                table: "PromoCodeInstances");

            migrationBuilder.DropColumn(
                name: "IsAdlerService",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "PromoCodeInstances",
                newName: "promoCodeInstances");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCodeInstances_StudentId",
                table: "promoCodeInstances",
                newName: "IX_promoCodeInstances_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_PromoCodeInstances_PromoCodeId",
                table: "promoCodeInstances",
                newName: "IX_promoCodeInstances_PromoCodeId");

            migrationBuilder.AddColumn<int>(
                name: "PromoCodenstanceId",
                table: "GroupInstanceStudents",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_promoCodeInstances",
                table: "promoCodeInstances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInstanceStudents_promoCodeInstances_PromoCodeInstanceId",
                table: "GroupInstanceStudents",
                column: "PromoCodeInstanceId",
                principalTable: "promoCodeInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InterestedStudents_promoCodeInstances_PromoCodeInstanceId",
                table: "InterestedStudents",
                column: "PromoCodeInstanceId",
                principalTable: "promoCodeInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_promoCodeInstances_PromoCodes_PromoCodeId",
                table: "promoCodeInstances",
                column: "PromoCodeId",
                principalTable: "PromoCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_promoCodeInstances_ApplicationUsers_StudentId",
                table: "promoCodeInstances",
                column: "StudentId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
