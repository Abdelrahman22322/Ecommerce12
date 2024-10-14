using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Products_ProductId",
                table: "Discount");

            migrationBuilder.DropIndex(
                name: "IX_Discount_ProductId",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Discount");

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Products",
                type: "int",
                nullable: true); // Allow null values

            migrationBuilder.AddColumn<string>(
                name: "DiscountName",
                table: "Discount",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DiscountId",
                table: "Products",
                column: "DiscountId");

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

            migrationBuilder.DropIndex(
                name: "IX_Products_DiscountId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountName",
                table: "Discount");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Discount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Discount_ProductId",
                table: "Discount",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Products_ProductId",
                table: "Discount",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
