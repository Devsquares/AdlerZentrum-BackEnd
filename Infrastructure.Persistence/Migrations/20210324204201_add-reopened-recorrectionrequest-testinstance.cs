using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class addreopenedrecorrectionrequesttestinstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<bool>(
            //     name: "ReCorrectionRequest",
            //     table: "TestInstances",
            //     nullable: false,
            //     defaultValue: false);

            // migrationBuilder.AddColumn<bool>(
            //     name: "Reopened",
            //     table: "TestInstances",
            //     nullable: false,
            //     defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReCorrectionRequest",
                table: "TestInstances");

            migrationBuilder.DropColumn(
                name: "Reopened",
                table: "TestInstances");
        }
    }
}
