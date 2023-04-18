using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestmentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCostTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "Transactions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
