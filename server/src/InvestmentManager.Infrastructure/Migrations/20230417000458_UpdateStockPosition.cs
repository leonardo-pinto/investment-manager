using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestmentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStockPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "StockPositions");

            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "StockPositions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "StockPositions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentPrice",
                table: "StockPositions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
