using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestmentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTradingCountryToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TradingCountry",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TradingCountry",
                table: "Transactions");
        }
    }
}
