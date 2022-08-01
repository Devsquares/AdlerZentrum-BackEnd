using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Method = table.Column<string>(nullable: true),
                    TransactionId = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    GroupDefinitionId = table.Column<int>(nullable: true),
                    PurchasingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_GroupDefinition_GroupDefinitionId",
                        column: x => x.GroupDefinitionId,
                        principalTable: "GroupDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_GroupDefinitionId",
                table: "PaymentTransactions",
                column: "GroupDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_UserId",
                table: "PaymentTransactions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTransactions");
        }
    }
}
