using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gahar_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSeoAndAnalyticsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalyticsEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventData = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BrowserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OsName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SearchVolume = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    SearchIntent = table.Column<int>(type: "int", nullable: false),
                    RankingPages = table.Column<int>(type: "int", nullable: false),
                    IsTargeted = table.Column<bool>(type: "bit", nullable: false),
                    TargetEntity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetEntityId = table.Column<int>(type: "int", nullable: true),
                    Impressions = table.Column<int>(type: "int", nullable: false),
                    Clicks = table.Column<int>(type: "int", nullable: false),
                    ClickThroughRate = table.Column<double>(type: "float", nullable: true),
                    AveragePosition = table.Column<double>(type: "float", nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageAnalytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PageViews = table.Column<long>(type: "bigint", nullable: false),
                    UniqueVisitors = table.Column<long>(type: "bigint", nullable: false),
                    AverageTimeOnPage = table.Column<double>(type: "float", nullable: false),
                    BounceRate = table.Column<double>(type: "float", nullable: false),
                    ClickCount = table.Column<long>(type: "bigint", nullable: false),
                    TopReferrer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TopDevice = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastAnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConversionCount = table.Column<int>(type: "int", nullable: true),
                    ConversionRate = table.Column<double>(type: "float", nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageAnalytics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeoMetadata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OgTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OgDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OgImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CanonicalUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsIndexable = table.Column<bool>(type: "bit", nullable: false),
                    IsFollowable = table.Column<bool>(type: "bit", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    LastOptimizedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SeoScore = table.Column<int>(type: "int", nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoMetadata", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 8, 3, 21, 440, DateTimeKind.Utc).AddTicks(2840));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 8, 3, 21, 440, DateTimeKind.Utc).AddTicks(2842));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 8, 3, 21, 440, DateTimeKind.Utc).AddTicks(2944));

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_CreatedAt",
                table: "AnalyticsEvents",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_EventType",
                table: "AnalyticsEvents",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_PageUrl",
                table: "AnalyticsEvents",
                column: "PageUrl");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_SessionId",
                table: "AnalyticsEvents",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_UserId_CreatedAt",
                table: "AnalyticsEvents",
                columns: new[] { "UserId", "CreatedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_Difficulty",
                table: "Keywords",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_SearchVolume",
                table: "Keywords",
                column: "SearchVolume",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_TargetEntity_TargetEntityId",
                table: "Keywords",
                columns: new[] { "TargetEntity", "TargetEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_Term",
                table: "Keywords",
                column: "Term",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageAnalytics_EntityType_EntityId",
                table: "PageAnalytics",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_PageAnalytics_LastAnalyzedAt",
                table: "PageAnalytics",
                column: "LastAnalyzedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_PageAnalytics_PageUrl",
                table: "PageAnalytics",
                column: "PageUrl");

            migrationBuilder.CreateIndex(
                name: "IX_SeoMetadata_CreatedAt",
                table: "SeoMetadata",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_SeoMetadata_EntityType_EntityId",
                table: "SeoMetadata",
                columns: new[] { "EntityType", "EntityId" },
                unique: true,
                filter: "[EntityType] IS NOT NULL AND [EntityId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalyticsEvents");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "PageAnalytics");

            migrationBuilder.DropTable(
                name: "SeoMetadata");

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 7, 55, 32, 24, DateTimeKind.Utc).AddTicks(9771));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 7, 55, 32, 24, DateTimeKind.Utc).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 7, 55, 32, 24, DateTimeKind.Utc).AddTicks(9859));
        }
    }
}
