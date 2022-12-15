using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingDBIdentityInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "date", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshTokenExpries = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Address", "CreatedAt", "DateOfBirth", "Email", "FirstName", "LastName", "Password", "Phone", "RefreshToken", "RefreshTokenCreatedAt", "RefreshTokenExpries", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "Hoang Mai", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Linh", "Nguyen", "admin", "0972901427", "", new DateTime(2022, 12, 11, 17, 58, 27, 1, DateTimeKind.Local).AddTicks(2117), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN", "admin" },
                    { 2, "Hai Ba Trung", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@gmail.com", "Linh", "Nguyen", "user", "0928347519", "", new DateTime(2022, 12, 11, 17, 58, 27, 1, DateTimeKind.Local).AddTicks(2138), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "USER", "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserName",
                table: "Accounts",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
