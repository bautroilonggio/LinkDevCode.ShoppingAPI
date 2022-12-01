using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.API.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingDBAddTotalQuantityToProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 1, 10, 3, 0, 31, DateTimeKind.Local).AddTicks(1902));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 1, 10, 3, 0, 31, DateTimeKind.Local).AddTicks(2268));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 1, 0, 7, 3, 967, DateTimeKind.Local).AddTicks(1557));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 1, 0, 7, 3, 967, DateTimeKind.Local).AddTicks(1636));
        }
    }
}
