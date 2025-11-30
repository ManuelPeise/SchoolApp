using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data.ContextMysql.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppUserCredentialsEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Salt = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserCredentialsEntity", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FamilyTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FamilyDisplayName = table.Column<string>(type: "longtext", nullable: false),
                    FamilyName = table.Column<string>(type: "longtext", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearnTopicTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TopicName = table.Column<string>(type: "longtext", nullable: false),
                    TopicDescription = table.Column<string>(type: "longtext", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnTopicTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LogTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(type: "longtext", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "longtext", nullable: false),
                    Stacktrace = table.Column<string>(type: "longtext", nullable: false),
                    LogLevel = table.Column<int>(type: "int", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ScheduleSettingsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    Minute = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSettingsTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SyncTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SyncronisationName = table.Column<string>(type: "longtext", nullable: false),
                    SyncronisationType = table.Column<int>(type: "int", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncTable", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VocabularyTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    German = table.Column<string>(type: "longtext", nullable: false),
                    English = table.Column<string>(type: "longtext", nullable: false),
                    Danish = table.Column<string>(type: "longtext", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocabularyTable_LearnTopicTable_TopicId",
                        column: x => x.TopicId,
                        principalTable: "LearnTopicTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SettingsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Theme = table.Column<int>(type: "int", nullable: false),
                    AutoSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ScheduleSettingsId = table.Column<int>(type: "int", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingsTable_ScheduleSettingsTable_ScheduleSettingsId",
                        column: x => x.ScheduleSettingsId,
                        principalTable: "ScheduleSettingsTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppUserTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LastName = table.Column<string>(type: "longtext", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserRole = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FamilyId = table.Column<int>(type: "int", nullable: true),
                    CredentialsId = table.Column<int>(type: "int", nullable: false),
                    SettingsId = table.Column<int>(type: "int", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserTable_AppUserCredentialsEntity_CredentialsId",
                        column: x => x.CredentialsId,
                        principalTable: "AppUserCredentialsEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserTable_FamilyTable_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "FamilyTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppUserTable_SettingsTable_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "SettingsTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserLearnTopicTable",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    CanView = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanEdit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Deny = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsInSync = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLearnTopicTable", x => new { x.TopicId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserLearnTopicTable_AppUserTable_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUserTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLearnTopicTable_LearnTopicTable_TopicId",
                        column: x => x.TopicId,
                        principalTable: "LearnTopicTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTable_CredentialsId",
                table: "AppUserTable",
                column: "CredentialsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTable_FamilyId",
                table: "AppUserTable",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTable_SettingsId",
                table: "AppUserTable",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_SettingsTable_ScheduleSettingsId",
                table: "SettingsTable",
                column: "ScheduleSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLearnTopicTable_UserId",
                table: "UserLearnTopicTable",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyTable_TopicId",
                table: "VocabularyTable",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogTable");

            migrationBuilder.DropTable(
                name: "SyncTable");

            migrationBuilder.DropTable(
                name: "UserLearnTopicTable");

            migrationBuilder.DropTable(
                name: "VocabularyTable");

            migrationBuilder.DropTable(
                name: "AppUserTable");

            migrationBuilder.DropTable(
                name: "LearnTopicTable");

            migrationBuilder.DropTable(
                name: "AppUserCredentialsEntity");

            migrationBuilder.DropTable(
                name: "FamilyTable");

            migrationBuilder.DropTable(
                name: "SettingsTable");

            migrationBuilder.DropTable(
                name: "ScheduleSettingsTable");
        }
    }
}
