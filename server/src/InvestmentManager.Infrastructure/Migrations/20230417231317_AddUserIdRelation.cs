using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestmentManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "StockPositions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_StockPositions_UserId",
                table: "StockPositions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockPositions_AspNetUsers_UserId",
                table: "StockPositions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockPositions_AspNetUsers_UserId",
                table: "StockPositions");

            migrationBuilder.DropIndex(
                name: "IX_StockPositions_UserId",
                table: "StockPositions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "StockPositions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
