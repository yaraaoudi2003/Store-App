using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDebtItemProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "DebtItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DebtItems_ProductId",
                table: "DebtItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtItems_Products_ProductId",
                table: "DebtItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtItems_Products_ProductId",
                table: "DebtItems");

            migrationBuilder.DropIndex(
                name: "IX_DebtItems_ProductId",
                table: "DebtItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "DebtItems");
        }
    }
}
