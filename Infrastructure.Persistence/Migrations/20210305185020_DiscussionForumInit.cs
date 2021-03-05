using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class DiscussionForumInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForumTopics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    WriterId = table.Column<string>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    ForumType = table.Column<int>(nullable: false),
                    GroupInstanceId = table.Column<int>(nullable: false),
                    GroupDefinitionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumTopics_GroupDefinition_GroupDefinitionId",
                        column: x => x.GroupDefinitionId,
                        principalTable: "GroupDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumTopics_GroupInstances_GroupInstanceId",
                        column: x => x.GroupInstanceId,
                        principalTable: "GroupInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumTopics_ApplicationUsers_WriterId",
                        column: x => x.WriterId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ForumComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    WriterId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    ForumTopicId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumComments_ForumTopics_ForumTopicId",
                        column: x => x.ForumTopicId,
                        principalTable: "ForumTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumComments_ApplicationUsers_WriterId",
                        column: x => x.WriterId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ForumReplys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    WriterId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    ForumCommentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumReplys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumReplys_ForumComments_ForumCommentId",
                        column: x => x.ForumCommentId,
                        principalTable: "ForumComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ForumReplys_ApplicationUsers_WriterId",
                        column: x => x.WriterId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumComments_ForumTopicId",
                table: "ForumComments",
                column: "ForumTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumComments_WriterId",
                table: "ForumComments",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumReplys_ForumCommentId",
                table: "ForumReplys",
                column: "ForumCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumReplys_WriterId",
                table: "ForumReplys",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopics_GroupDefinitionId",
                table: "ForumTopics",
                column: "GroupDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopics_GroupInstanceId",
                table: "ForumTopics",
                column: "GroupInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopics_WriterId",
                table: "ForumTopics",
                column: "WriterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumReplys");

            migrationBuilder.DropTable(
                name: "ForumComments");

            migrationBuilder.DropTable(
                name: "ForumTopics");
        }
    }
}
