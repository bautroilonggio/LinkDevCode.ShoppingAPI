using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalog.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingDBCatalogInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SellingPrice = table.Column<double>(type: "float", nullable: true),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "Category", "CreateAt", "Description", "Name", "SellingPrice", "TotalQuantity" },
                values: new object[,]
                {
                    { 1, "Samsung", "Smartphone", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "8GB/128GB", "Galaxy S21 FE (5G)", 11850000.0, 20 },
                    { 2, "Samsung", "Smartphone", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "12GB/512GB", "Galaxy S22 Ultra", 28990000.0, 25 },
                    { 3, "Samsung", "Smartwatch", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "45mm", "Galaxy Watch 5 Pro", 10990000.0, 21 },
                    { 4, "Samsung", "Bluetooth Headphone", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "In-Ear", "Galaxy Buds 2 Pro", 4590000.0, 14 },
                    { 5, "Samsung", "Bluetooth Speaker", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sound Tower", "Samsung Sound Tower ST50B/XV", 8490000.0, 18 },
                    { 6, "Samsung", "Tablet", new DateTime(2022, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "8B/128GB", "Galaxy Tab S8 Ultra", 25390000.0, 12 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
