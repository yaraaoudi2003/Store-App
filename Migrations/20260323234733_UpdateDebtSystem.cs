using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDebtSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerDebtId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CustomerDebtId",
                table: "Sales",
                column: "CustomerDebtId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_CustomerDebts_CustomerDebtId",
                table: "Sales",
                column: "CustomerDebtId",
                principalTable: "CustomerDebts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_CustomerDebts_CustomerDebtId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_CustomerDebtId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CustomerDebtId",
                table: "Sales");
        }
    }
}
