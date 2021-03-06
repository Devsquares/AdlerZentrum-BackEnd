using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class DiscussionForumInit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.AlterColumn<int>(
                name: "ForumCommentId",
                table: "ForumReplys",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ForumCommentId",
                table: "ForumReplys",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
