using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementDAL.Migrations
{
    /// <inheritdoc />
    public partial class updateCascadeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_Products_ProductID",
                table: "InventoryTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_Products_ProductID",
                table: "InventoryTransactions",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_Products_ProductID",
                table: "InventoryTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_Products_ProductID",
                table: "InventoryTransactions",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
