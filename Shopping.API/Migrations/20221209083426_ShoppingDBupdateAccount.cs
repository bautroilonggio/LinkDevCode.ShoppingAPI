using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.API.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingDBupdateAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DateOfBirth", "RefreshTokenCreatedAt" },
                values: new object[] { new DateTime(2022, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 9, 15, 34, 26, 131, DateTimeKind.Local).AddTicks(8923) });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DateOfBirth", "RefreshTokenCreatedAt" },
                values: new object[] { new DateTime(2022, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 9, 15, 34, 26, 131, DateTimeKind.Local).AddTicks(9080) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DateOfBirth", "RefreshTokenCreatedAt" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 2, 18, 11, 34, 474, DateTimeKind.Local).AddTicks(9869) });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DateOfBirth", "RefreshTokenCreatedAt" },
                values: new object[] { new DateTime(2022, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 2, 18, 11, 34, 474, DateTimeKind.Local).AddTicks(9947) });
        }
    }
}
