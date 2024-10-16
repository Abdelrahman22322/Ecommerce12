using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editshioppingmethod12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_ShippingCost_MethodId",
                table: "Shippings");

            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_ShippingCost_ShippingCostId",
                table: "Shippings");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_MethodId",
                table: "Shippings");

            migrationBuilder.DropColumn(
                name: "MethodId",
                table: "Shippings");

            migrationBuilder.RenameColumn(
                name: "ShippingCostId",
                table: "Shippings",
                newName: "ShippingMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Shippings_ShippingCostId",
                table: "Shippings",
                newName: "IX_Shippings_ShippingMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_ShippingCost_ShippingMethodId",
                table: "Shippings",
                column: "ShippingMethodId",
                principalTable: "ShippingCost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_ShippingCost_ShippingMethodId",
                table: "Shippings");

            migrationBuilder.RenameColumn(
                name: "ShippingMethodId",
                table: "Shippings",
                newName: "ShippingCostId");

            migrationBuilder.RenameIndex(
                name: "IX_Shippings_ShippingMethodId",
                table: "Shippings",
                newName: "IX_Shippings_ShippingCostId");

            migrationBuilder.AddColumn<int>(
                name: "MethodId",
                table: "Shippings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_MethodId",
                table: "Shippings",
                column: "MethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_ShippingCost_MethodId",
                table: "Shippings",
                column: "MethodId",
                principalTable: "ShippingCost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_ShippingCost_ShippingCostId",
                table: "Shippings",
                column: "ShippingCostId",
                principalTable: "ShippingCost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
