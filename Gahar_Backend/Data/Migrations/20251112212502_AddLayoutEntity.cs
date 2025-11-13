using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gahar_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLayoutEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Layouts_LayoutId",
                table: "Contents");

            migrationBuilder.AddColumn<int>(
                name: "LayoutId",
                table: "Translations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Layouts",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Layouts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Configuration",
                table: "Layouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Layouts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PreviewImage",
                table: "Layouts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 21, 25, 1, 658, DateTimeKind.Utc).AddTicks(8059));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 21, 25, 1, 658, DateTimeKind.Utc).AddTicks(8146));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 21, 25, 1, 659, DateTimeKind.Utc).AddTicks(1808));

            migrationBuilder.CreateIndex(
                name: "IX_Translations_LayoutId",
                table: "Translations",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Layouts_IsActive",
                table: "Layouts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Layouts_IsDefault",
                table: "Layouts",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_Layouts_Name",
                table: "Layouts",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Layouts_LayoutId",
                table: "Contents",
                column: "LayoutId",
                principalTable: "Layouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Layouts_LayoutId",
                table: "Translations",
                column: "LayoutId",
                principalTable: "Layouts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Layouts_LayoutId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Layouts_LayoutId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_LayoutId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Layouts_IsActive",
                table: "Layouts");

            migrationBuilder.DropIndex(
                name: "IX_Layouts_IsDefault",
                table: "Layouts");

            migrationBuilder.DropIndex(
                name: "IX_Layouts_Name",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "LayoutId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "Configuration",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Layouts");

            migrationBuilder.DropColumn(
                name: "PreviewImage",
                table: "Layouts");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Layouts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Layouts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 20, 42, 30, 603, DateTimeKind.Utc).AddTicks(2856));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 20, 42, 30, 603, DateTimeKind.Utc).AddTicks(2858));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 20, 42, 30, 603, DateTimeKind.Utc).AddTicks(2952));

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Layouts_LayoutId",
                table: "Contents",
                column: "LayoutId",
                principalTable: "Layouts",
                principalColumn: "Id");
        }
    }
}
