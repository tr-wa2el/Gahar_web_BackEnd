using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gahar_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLayoutsMediaAndAlbums : Migration
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
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ThumbnailPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WebPPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    WebPFileSize = table.Column<long>(type: "bigint", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Alt = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UploadedBy = table.Column<int>(type: "int", nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Media_Users_UploadedBy",
                        column: x => x.UploadedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

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
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
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
                    Caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
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
                value: new DateTime(2025, 11, 13, 10, 50, 51, 338, DateTimeKind.Utc).AddTicks(4414));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 10, 50, 51, 338, DateTimeKind.Utc).AddTicks(4416));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 10, 50, 51, 338, DateTimeKind.Utc).AddTicks(4514));

            migrationBuilder.CreateIndex(
                name: "IX_Translations_AlbumId",
                table: "Translations",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumMedias_AlbumId_DisplayOrder",
                table: "AlbumMedias",
                columns: new[] { "AlbumId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumMedias_AlbumId_IsFeatured",
                table: "AlbumMedias",
                columns: new[] { "AlbumId", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumMedias_MediaId",
                table: "AlbumMedias",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_CoverImageId",
                table: "Albums",
                column: "CoverImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_CreatedBy",
                table: "Albums",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_IsPublished",
                table: "Albums",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_PublishedAt",
                table: "Albums",
                column: "PublishedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_Slug",
                table: "Albums",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ViewCount",
                table: "Albums",
                column: "ViewCount",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Media_CreatedAt",
                table: "Media",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Media_IsProcessed",
                table: "Media",
                column: "IsProcessed");

            migrationBuilder.CreateIndex(
                name: "IX_Media_MediaType",
                table: "Media",
                column: "MediaType");

            migrationBuilder.CreateIndex(
                name: "IX_Media_MimeType",
                table: "Media",
                column: "MimeType");

            migrationBuilder.CreateIndex(
                name: "IX_Media_UploadedBy",
                table: "Media",
                column: "UploadedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Media_UsageCount",
                table: "Media",
                column: "UsageCount",
                descending: new bool[0]);

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

            migrationBuilder.DropTable(
                name: "Media");

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
                value: new DateTime(2025, 11, 13, 10, 27, 13, 401, DateTimeKind.Utc).AddTicks(4801));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 10, 27, 13, 401, DateTimeKind.Utc).AddTicks(4803));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 10, 27, 13, 401, DateTimeKind.Utc).AddTicks(4893));
        }
    }
}
