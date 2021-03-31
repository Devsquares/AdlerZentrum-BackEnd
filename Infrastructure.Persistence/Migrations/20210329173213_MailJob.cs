using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class MailJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateBody",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "EmailTemplates");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "EmailTemplates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EmailTemplates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "EmailTemplates",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MailJobs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    TestInstanceId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Failure = table.Column<string>(nullable: true),
                    StudentId = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    GroupInstanceId = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    ExecutionDate = table.Column<DateTime>(nullable: true),
                    FinishDate = table.Column<DateTime>(nullable: true),
                    TeacherId = table.Column<string>(nullable: true),
                    HomeworkId = table.Column<int>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailJobs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailJobs");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "EmailTemplates");

            migrationBuilder.AddColumn<string>(
                name: "TemplateBody",
                table: "EmailTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "EmailTemplates",
                type: "text",
                nullable: true);
        }
    }
}
