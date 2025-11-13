using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gahar_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddContentManagementSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                table: "Translations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContentTypeFieldId",
                table: "Translations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContentTypeId",
                table: "Translations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LayoutId",
                table: "Translations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Translations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsSinglePage = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    MetaTitleTemplate = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MetaDescriptionTemplate = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Layouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Configuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PreviewImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypeFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsTranslatable = table.Column<bool>(type: "bit", nullable: false),
                    ShowInList = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    ValidationRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Placeholder = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypeFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTypeFields_ContentTypes_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentTypeId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeaturedImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LayoutId = table.Column<int>(type: "int", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OgTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OgDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OgImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    AllowComments = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contents_ContentTypes_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contents_Layouts_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "Layouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Contents_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContentFieldValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    ContentTypeFieldId = table.Column<int>(type: "int", nullable: false),
                    FieldKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFieldValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentFieldValues_ContentTypeFields_ContentTypeFieldId",
                        column: x => x.ContentTypeFieldId,
                        principalTable: "ContentTypeFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentFieldValues_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentFieldValues_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContentTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTags_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ContentId",
                table: "Translations",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ContentTypeFieldId",
                table: "Translations",
                column: "ContentTypeFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ContentTypeId",
                table: "Translations",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_LayoutId",
                table: "Translations",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_TagId",
                table: "Translations",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentFieldValues_ContentId_FieldKey_LanguageId",
                table: "ContentFieldValues",
                columns: new[] { "ContentId", "FieldKey", "LanguageId" },
                unique: true,
                filter: "[LanguageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContentFieldValues_ContentTypeFieldId",
                table: "ContentFieldValues",
                column: "ContentTypeFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentFieldValues_LanguageId",
                table: "ContentFieldValues",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_AuthorId",
                table: "Contents",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContentTypeId_Slug",
                table: "Contents",
                columns: new[] { "ContentTypeId", "Slug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contents_IsFeatured_PublishedAt",
                table: "Contents",
                columns: new[] { "IsFeatured", "PublishedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_LayoutId",
                table: "Contents",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Status_PublishedAt",
                table: "Contents",
                columns: new[] { "Status", "PublishedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ViewCount",
                table: "Contents",
                column: "ViewCount",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_ContentId_TagId",
                table: "ContentTags",
                columns: new[] { "ContentId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_TagId",
                table: "ContentTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeFields_ContentTypeId_DisplayOrder",
                table: "ContentTypeFields",
                columns: new[] { "ContentTypeId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeFields_ContentTypeId_FieldKey",
                table: "ContentTypeFields",
                columns: new[] { "ContentTypeId", "FieldKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypes_IsActive_DisplayOrder",
                table: "ContentTypes",
                columns: new[] { "IsActive", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypes_Slug",
                table: "ContentTypes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Layouts_IsDefault",
                table: "Layouts",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Slug",
                table: "Tags",
                column: "Slug",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_ContentTypeFields_ContentTypeFieldId",
                table: "Translations",
                column: "ContentTypeFieldId",
                principalTable: "ContentTypeFields",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_ContentTypes_ContentTypeId",
                table: "Translations",
                column: "ContentTypeId",
                principalTable: "ContentTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Contents_ContentId",
                table: "Translations",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Layouts_LayoutId",
                table: "Translations",
                column: "LayoutId",
                principalTable: "Layouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Tags_TagId",
                table: "Translations",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_ContentTypeFields_ContentTypeFieldId",
                table: "Translations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_ContentTypes_ContentTypeId",
                table: "Translations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Contents_ContentId",
                table: "Translations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Layouts_LayoutId",
                table: "Translations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Tags_TagId",
                table: "Translations");

            migrationBuilder.DropTable(
                name: "ContentFieldValues");

            migrationBuilder.DropTable(
                name: "ContentTags");

            migrationBuilder.DropTable(
                name: "ContentTypeFields");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "ContentTypes");

            migrationBuilder.DropTable(
                name: "Layouts");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ContentId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ContentTypeFieldId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ContentTypeId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_LayoutId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_TagId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "ContentTypeFieldId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "ContentTypeId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "LayoutId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Translations");

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
        }
    }
}
