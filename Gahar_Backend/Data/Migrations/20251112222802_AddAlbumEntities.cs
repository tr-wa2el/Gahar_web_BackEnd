using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gahar_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbumEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "Translations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CoverImageId = table.Column<int>(type: "int", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Media_CoverImageId",
                        column: x => x.CoverImageId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Albums_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AlbumMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    MediaId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlbumMedias_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumMedias_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 22, 28, 2, 308, DateTimeKind.Utc).AddTicks(2805));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 22, 28, 2, 308, DateTimeKind.Utc).AddTicks(2882));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 22, 28, 2, 308, DateTimeKind.Utc).AddTicks(6042));

            migrationBuilder.CreateIndex(
                name: "IX_Translations_AlbumId",
                table: "Translations",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumMedias_AlbumId_MediaId",
                table: "AlbumMedias",
                columns: new[] { "AlbumId", "MediaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlbumMedias_DisplayOrder",
                table: "AlbumMedias",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumMedias_MediaId",
                table: "AlbumMedias",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_CoverImageId",
                table: "Albums",
                column: "CoverImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_CreatedAt",
                table: "Albums",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_CreatedBy",
                table: "Albums",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_IsPublished",
                table: "Albums",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_Slug",
                table: "Albums",
                column: "Slug",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Albums_AlbumId",
                table: "Translations",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Albums_AlbumId",
                table: "Translations");

            migrationBuilder.DropTable(
                name: "AlbumMedias");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Translations_AlbumId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "Translations");

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 22, 4, 6, 18, DateTimeKind.Utc).AddTicks(4549));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 22, 4, 6, 18, DateTimeKind.Utc).AddTicks(4631));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 22, 4, 6, 18, DateTimeKind.Utc).AddTicks(8456));
        }
    }
}
