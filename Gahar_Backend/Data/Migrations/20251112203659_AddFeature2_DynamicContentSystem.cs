using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gahar_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFeature2_DynamicContentSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContentTags_ContentId",
                table: "ContentTags");

            migrationBuilder.DropIndex(
                name: "IX_Contents_ContentTypeId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_ContentFieldValues_ContentId",
                table: "ContentFieldValues");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Contents");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Tags",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tags",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Tags",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tags",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsageCount",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ContentTags",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Contents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Contents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Contents",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "AllowComments",
                table: "Contents",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Contents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeaturedImage",
                table: "Contents",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Contents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LayoutId",
                table: "Contents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "Contents",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaKeywords",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "Contents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OgDescription",
                table: "Contents",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OgImage",
                table: "Contents",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OgTitle",
                table: "Contents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Contents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledAt",
                table: "Contents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Contents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Draft");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Contents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ContentFieldValues",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "FieldKey",
                table: "ContentFieldValues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Layouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layouts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 20, 36, 58, 949, DateTimeKind.Utc).AddTicks(3392));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 20, 36, 58, 949, DateTimeKind.Utc).AddTicks(3394));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 20, 36, 58, 949, DateTimeKind.Utc).AddTicks(3494));

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Slug",
                table: "Tags",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UsageCount",
                table: "Tags",
                column: "UsageCount");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_ContentId_TagId",
                table: "ContentTags",
                columns: new[] { "ContentId", "TagId" },
                unique: true);

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
                name: "IX_Contents_IsFeatured",
                table: "Contents",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_LayoutId",
                table: "Contents",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_PublishedAt",
                table: "Contents",
                column: "PublishedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Status",
                table: "Contents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ContentFieldValues_ContentId_ContentTypeFieldId_LanguageId",
                table: "ContentFieldValues",
                columns: new[] { "ContentId", "ContentTypeFieldId", "LanguageId" },
                unique: true,
                filter: "[LanguageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Layouts_LayoutId",
                table: "Contents",
                column: "LayoutId",
                principalTable: "Layouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Users_AuthorId",
                table: "Contents",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Layouts_LayoutId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Users_AuthorId",
                table: "Contents");

            migrationBuilder.DropTable(
                name: "Layouts");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Slug",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_UsageCount",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_ContentTags_ContentId_TagId",
                table: "ContentTags");

            migrationBuilder.DropIndex(
                name: "IX_Contents_AuthorId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_ContentTypeId_Slug",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_IsFeatured",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_LayoutId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_PublishedAt",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_Status",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_ContentFieldValues_ContentId_ContentTypeFieldId_LanguageId",
                table: "ContentFieldValues");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "UsageCount",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "AllowComments",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "FeaturedImage",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "LayoutId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "MetaKeywords",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "OgDescription",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "OgImage",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "OgTitle",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "ScheduledAt",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "FieldKey",
                table: "ContentFieldValues");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tags",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ContentTags",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Contents",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Contents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ContentFieldValues",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 19, 7, 15, 933, DateTimeKind.Utc).AddTicks(8588));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 19, 7, 15, 933, DateTimeKind.Utc).AddTicks(8590));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 19, 7, 15, 933, DateTimeKind.Utc).AddTicks(8676));

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_ContentId",
                table: "ContentTags",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ContentTypeId",
                table: "Contents",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentFieldValues_ContentId",
                table: "ContentFieldValues",
                column: "ContentId");
        }
    }
}
