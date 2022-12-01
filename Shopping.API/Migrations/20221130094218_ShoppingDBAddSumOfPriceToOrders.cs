using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.API.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingDBAddSumOfPriceToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SumOfPrice",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumOfPrice",
                table: "Orders");
        }
    }
}
