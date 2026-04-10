using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyAndExchangeRateToSales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Sales",
                newName: "SaleCurrency");

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseCurrency",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PurchaseCurrency",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "SaleCurrency",
                table: "Sales",
                newName: "Currency");
        }
    }
}
