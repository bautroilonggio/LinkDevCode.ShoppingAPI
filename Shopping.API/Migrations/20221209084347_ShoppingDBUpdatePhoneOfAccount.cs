using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.API.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingDBUpdatePhoneOfAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 9, 15, 43, 46, 377, DateTimeKind.Local).AddTicks(7719));

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 9, 15, 43, 46, 377, DateTimeKind.Local).AddTicks(7920));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 9, 15, 34, 26, 131, DateTimeKind.Local).AddTicks(8923));

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 9, 15, 34, 26, 131, DateTimeKind.Local).AddTicks(9080));
        }
    }
}
