using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteshitagainandagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shippings_ShippingMethodId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingMethodId",
                table: "Orders",
                newName: "ShippingId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingMethodId",
                table: "Orders",
                newName: "IX_Orders_ShippingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shippings_ShippingId",
                table: "Orders",
                column: "ShippingId",
                principalTable: "Shippings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shippings_ShippingId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingId",
                table: "Orders",
                newName: "ShippingMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingId",
                table: "Orders",
                newName: "IX_Orders_ShippingMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shippings_ShippingMethodId",
                table: "Orders",
                column: "ShippingMethodId",
                principalTable: "Shippings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
