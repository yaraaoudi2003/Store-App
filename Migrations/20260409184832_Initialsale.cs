using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.Migrations
{
    /// <inheritdoc />
    public partial class Initialsale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "DebtPayments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DebtPayments_SaleId",
                table: "DebtPayments",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayments_Sales_SaleId",
                table: "DebtPayments",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayments_Sales_SaleId",
                table: "DebtPayments");

            migrationBuilder.DropIndex(
                name: "IX_DebtPayments_SaleId",
                table: "DebtPayments");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "DebtPayments");
        }
    }
}
