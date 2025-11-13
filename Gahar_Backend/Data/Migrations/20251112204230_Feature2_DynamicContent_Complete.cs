using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gahar_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Feature2_DynamicContent_Complete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
