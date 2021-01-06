using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    HouseNumber = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    PostalCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupCondition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    NumberOfSolts = table.Column<int>(nullable: false),
                    NumberOfSlotsWithPlacementTest = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupCondition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pricing",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 85, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 85, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubLevels",
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
                    NumberOflessons = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubLevels_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 85, nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 85, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 85, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Profilephoto = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    AddressId = table.Column<int>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    ChangePassword = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 85, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(maxLength: 85, nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlotDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    TimeSlotId = table.Column<int>(nullable: false),
                    WeekDay = table.Column<int>(nullable: false),
                    TimeFrom = table.Column<DateTime>(nullable: false),
                    TimeTo = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlotDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSlotDetails_TimeSlot_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalTable: "TimeSlot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    SubLevelId = table.Column<int>(nullable: false),
                    TimeSlotId = table.Column<int>(nullable: false),
                    PricingId = table.Column<int>(nullable: false),
                    GroupConditionId = table.Column<int>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    FinalTestDate = table.Column<DateTime>(nullable: true),
                    MaxInstances = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupDefinition_GroupCondition_GroupConditionId",
                        column: x => x.GroupConditionId,
                        principalTable: "GroupCondition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupDefinition_Pricing_PricingId",
                        column: x => x.PricingId,
                        principalTable: "Pricing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupDefinition_SubLevels_SubLevelId",
                        column: x => x.SubLevelId,
                        principalTable: "SubLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupDefinition_TimeSlot_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalTable: "TimeSlot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 85, nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 85, nullable: false),
                    Name = table.Column<string>(maxLength: 85, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedByIp = table.Column<string>(nullable: true),
                    Revoked = table.Column<DateTime>(nullable: true),
                    RevokedByIp = table.Column<string>(nullable: true),
                    ReplacedByToken = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 85, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(maxLength: 85, nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 85, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 85, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(maxLength: 85, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 85, nullable: false),
                    RoleId = table.Column<string>(maxLength: 85, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    GroupDefinitionId = table.Column<int>(nullable: false),
                    Serial = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupInstances_GroupDefinition_GroupDefinitionId",
                        column: x => x.GroupDefinitionId,
                        principalTable: "GroupDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TestDuration = table.Column<int>(nullable: false),
                    TestTypeId = table.Column<int>(nullable: false),
                    LessonDefinitionId = table.Column<int>(nullable: false),
                    AutoCorrect = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_LessonDefinition_LessonDefinitionId",
                        column: x => x.LessonDefinitionId,
                        principalTable: "LessonDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupInstanceStudents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StudentId = table.Column<string>(nullable: true),
                    GroupInstanceId = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInstanceStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupInstanceStudents_GroupInstances_GroupInstanceId",
                        column: x => x.GroupInstanceId,
                        principalTable: "GroupInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupInstanceStudents_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherGroupInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    TeacherId = table.Column<string>(nullable: true),
                    GroupInstanceId = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherGroupInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherGroupInstances_GroupInstances_GroupInstanceId",
                        column: x => x.GroupInstanceId,
                        principalTable: "GroupInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherGroupInstances_ApplicationUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    MaterialDone = table.Column<string>(nullable: true),
                    MaterialToDo = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(nullable: true),
                    SubmittedReport = table.Column<bool>(nullable: false),
                    TestId = table.Column<int>(nullable: true),
                    TestStatus = table.Column<int>(nullable: true)
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
                        name: "FK_LessonInstances_LessonDefinition_LessonDefinitionId",
                        column: x => x.LessonDefinitionId,
                        principalTable: "LessonDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonInstances_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    TestId = table.Column<int>(nullable: true),
                    QuestionTypeId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Header = table.Column<string>(nullable: true),
                    MinCharacters = table.Column<int>(nullable: true),
                    AudioPath = table.Column<string>(nullable: true),
                    NoOfRepeats = table.Column<int>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    BonusPoints = table.Column<int>(nullable: false),
                    BonusPointsStatus = table.Column<int>(nullable: false),
                    GroupInstanceId = table.Column<int>(nullable: false),
                    LessonInstanceId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homeworks_GroupInstances_GroupInstanceId",
                        column: x => x.GroupInstanceId,
                        principalTable: "GroupInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Homeworks_LessonInstances_LessonInstanceId",
                        column: x => x.LessonInstanceId,
                        principalTable: "LessonInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Homeworks_ApplicationUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessonInstanceStudent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LessonInstanceId = table.Column<int>(nullable: false),
                    StudentId = table.Column<string>(nullable: true),
                    Attend = table.Column<bool>(nullable: false),
                    Homework = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonInstanceStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonInstanceStudent_LessonInstances_LessonInstanceId",
                        column: x => x.LessonInstanceId,
                        principalTable: "LessonInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonInstanceStudent_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StudentId = table.Column<string>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    LessonInstanceId = table.Column<int>(nullable: false),
                    CorrectionTeacherId = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    SubmissionDate = table.Column<DateTime>(nullable: false),
                    TestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestInstances_ApplicationUsers_CorrectionTeacherId",
                        column: x => x.CorrectionTeacherId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestInstances_LessonInstances_LessonInstanceId",
                        column: x => x.LessonInstanceId,
                        principalTable: "LessonInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestInstances_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SingleQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    QuestionId = table.Column<int>(nullable: true),
                    SingleQuestionType = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    AnswerIsTrueOrFalse = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HomeWorkSubmitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StudentId = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    HomeworkId = table.Column<int>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Solution = table.Column<string>(nullable: true),
                    CorrectionDate = table.Column<DateTime>(nullable: true),
                    CorrectionTeacherId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    SubmitionDate = table.Column<DateTime>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeWorkSubmitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeWorkSubmitions_ApplicationUsers_CorrectionTeacherId",
                        column: x => x.CorrectionTeacherId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HomeWorkSubmitions_Homeworks_HomeworkId",
                        column: x => x.HomeworkId,
                        principalTable: "Homeworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeWorkSubmitions_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Choice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SingleQuestionId = table.Column<int>(nullable: false),
                    IsCorrect = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choice_SingleQuestions_SingleQuestionId",
                        column: x => x.SingleQuestionId,
                        principalTable: "SingleQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SingleQuestionSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    AnswerText = table.Column<string>(nullable: true),
                    TrueOrFalseSubmission = table.Column<bool>(nullable: false),
                    StudentId = table.Column<string>(nullable: true),
                    SingleQuestionId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleQuestionSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleQuestionSubmissions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SingleQuestionSubmissions_SingleQuestions_SingleQuestionId",
                        column: x => x.SingleQuestionId,
                        principalTable: "SingleQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SingleQuestionSubmissions_ApplicationUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChoiceSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(maxLength: 256, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    ChoiceId = table.Column<int>(nullable: false),
                    SingleQuestionSubmissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoiceSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChoiceSubmissions_Choice_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "Choice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChoiceSubmissions_SingleQuestionSubmissions_SingleQuestionSu~",
                        column: x => x.SingleQuestionSubmissionId,
                        principalTable: "SingleQuestionSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_AddressId",
                table: "ApplicationUsers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "ApplicationUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "ApplicationUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_RoleId",
                table: "ApplicationUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_SingleQuestionId",
                table: "Choice",
                column: "SingleQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceSubmissions_ChoiceId",
                table: "ChoiceSubmissions",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceSubmissions_SingleQuestionSubmissionId",
                table: "ChoiceSubmissions",
                column: "SingleQuestionSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDefinition_GroupConditionId",
                table: "GroupDefinition",
                column: "GroupConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDefinition_PricingId",
                table: "GroupDefinition",
                column: "PricingId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDefinition_SubLevelId",
                table: "GroupDefinition",
                column: "SubLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDefinition_TimeSlotId",
                table: "GroupDefinition",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInstances_GroupDefinitionId",
                table: "GroupInstances",
                column: "GroupDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInstanceStudents_GroupInstanceId",
                table: "GroupInstanceStudents",
                column: "GroupInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupInstanceStudents_StudentId",
                table: "GroupInstanceStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_GroupInstanceId",
                table: "Homeworks",
                column: "GroupInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_LessonInstanceId",
                table: "Homeworks",
                column: "LessonInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_TeacherId",
                table: "Homeworks",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorkSubmitions_CorrectionTeacherId",
                table: "HomeWorkSubmitions",
                column: "CorrectionTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorkSubmitions_HomeworkId",
                table: "HomeWorkSubmitions",
                column: "HomeworkId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorkSubmitions_StudentId",
                table: "HomeWorkSubmitions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonDefinition_SublevelId",
                table: "LessonDefinition",
                column: "SublevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstances_GroupInstanceId",
                table: "LessonInstances",
                column: "GroupInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstances_LessonDefinitionId",
                table: "LessonInstances",
                column: "LessonDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstances_TestId",
                table: "LessonInstances",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstanceStudent_LessonInstanceId",
                table: "LessonInstanceStudent",
                column: "LessonInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonInstanceStudent_StudentId",
                table: "LessonInstanceStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestId",
                table: "Questions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ApplicationUserId",
                table: "RefreshToken",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestions_QuestionId",
                table: "SingleQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestionSubmissions_QuestionId",
                table: "SingleQuestionSubmissions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestionSubmissions_SingleQuestionId",
                table: "SingleQuestionSubmissions",
                column: "SingleQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleQuestionSubmissions_StudentId",
                table: "SingleQuestionSubmissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubLevels_LevelId",
                table: "SubLevels",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGroupInstances_GroupInstanceId",
                table: "TeacherGroupInstances",
                column: "GroupInstanceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGroupInstances_TeacherId",
                table: "TeacherGroupInstances",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TestInstances_CorrectionTeacherId",
                table: "TestInstances",
                column: "CorrectionTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TestInstances_LessonInstanceId",
                table: "TestInstances",
                column: "LessonInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_TestInstances_TestId",
                table: "TestInstances",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_LessonDefinitionId",
                table: "Tests",
                column: "LessonDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlotDetails_TimeSlotId",
                table: "TimeSlotDetails",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChoiceSubmissions");

            migrationBuilder.DropTable(
                name: "GroupInstanceStudents");

            migrationBuilder.DropTable(
                name: "HomeWorkSubmitions");

            migrationBuilder.DropTable(
                name: "LessonInstanceStudent");

            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "TeacherGroupInstances");

            migrationBuilder.DropTable(
                name: "TestInstances");

            migrationBuilder.DropTable(
                name: "TimeSlotDetails");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Choice");

            migrationBuilder.DropTable(
                name: "SingleQuestionSubmissions");

            migrationBuilder.DropTable(
                name: "Homeworks");

            migrationBuilder.DropTable(
                name: "SingleQuestions");

            migrationBuilder.DropTable(
                name: "LessonInstances");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "GroupInstances");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "GroupDefinition");

            migrationBuilder.DropTable(
                name: "LessonDefinition");

            migrationBuilder.DropTable(
                name: "GroupCondition");

            migrationBuilder.DropTable(
                name: "Pricing");

            migrationBuilder.DropTable(
                name: "TimeSlot");

            migrationBuilder.DropTable(
                name: "SubLevels");

            migrationBuilder.DropTable(
                name: "Levels");
        }
    }
}
