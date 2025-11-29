using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserCredentialsEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Salt = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserCredentialsEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FamilyDisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    FamilyName = table.Column<string>(type: "TEXT", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LearnTopicTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TopicName = table.Column<string>(type: "TEXT", nullable: false),
                    TopicDescription = table.Column<string>(type: "TEXT", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnTopicTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "TEXT", nullable: false),
                    Stacktrace = table.Column<string>(type: "TEXT", nullable: false),
                    LogLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleSettingsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    Hour = table.Column<int>(type: "INTEGER", nullable: false),
                    Minute = table.Column<int>(type: "INTEGER", nullable: false),
                    Interval = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSettingsTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SyncronisationName = table.Column<string>(type: "TEXT", nullable: false),
                    SyncronisationType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VocabularyTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    German = table.Column<string>(type: "TEXT", nullable: false),
                    English = table.Column<string>(type: "TEXT", nullable: false),
                    Danish = table.Column<string>(type: "TEXT", nullable: false),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "SettingsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Theme = table.Column<string>(type: "TEXT", nullable: false),
                    AutoSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    ScheduleSettingsId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "AppUserTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserRole = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    FamilyId = table.Column<int>(type: "INTEGER", nullable: true),
                    CredentialsId = table.Column<int>(type: "INTEGER", nullable: false),
                    SettingsId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "UserLearnTopicTable",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    CanView = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEdit = table.Column<bool>(type: "INTEGER", nullable: false),
                    Deny = table.Column<bool>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInSync = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                });

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
