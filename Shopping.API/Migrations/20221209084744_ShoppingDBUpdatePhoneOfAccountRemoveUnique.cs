using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.API.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingDBUpdatePhoneOfAccountRemoveUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_Phone",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 9, 15, 47, 43, 926, DateTimeKind.Local).AddTicks(1319));

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "RefreshTokenCreatedAt",
                value: new DateTime(2022, 12, 9, 15, 47, 43, 926, DateTimeKind.Local).AddTicks(1572));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Phone",
                table: "Accounts",
                column: "Phone",
                unique: true);
        }
    }
}
