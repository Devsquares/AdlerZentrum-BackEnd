using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Infrastructure.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime?>(
              name: "StartDate",
              table: "TestInstances",
              nullable: true,
              oldNullable: false); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
