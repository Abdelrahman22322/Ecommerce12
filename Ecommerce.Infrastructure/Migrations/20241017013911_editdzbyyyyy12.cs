using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editdzbyyyyy12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Discount_DiscountId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Discount_DiscountId",
                table: "Products",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Discount_DiscountId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Discount_DiscountId",
                table: "Products",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
