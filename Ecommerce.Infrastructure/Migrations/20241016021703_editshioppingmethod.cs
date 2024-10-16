using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editshioppingmethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Shippings");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Shippings",
                newName: "ShippingStateId");

            migrationBuilder.RenameColumn(
                name: "Method",
                table: "Shippings",
                newName: "ShippingCostId");

            migrationBuilder.AddColumn<int>(
                name: "MethodId",
                table: "Shippings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShippingCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingCost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingState", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_MethodId",
                table: "Shippings",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_ShippingCostId",
                table: "Shippings",
                column: "ShippingCostId");

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_ShippingStateId",
                table: "Shippings",
                column: "ShippingStateId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_ShippingState_ShippingStateId",
                table: "Shippings",
                column: "ShippingStateId",
                principalTable: "ShippingState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_ShippingCost_MethodId",
                table: "Shippings");

            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_ShippingCost_ShippingCostId",
                table: "Shippings");

            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_ShippingState_ShippingStateId",
                table: "Shippings");

            migrationBuilder.DropTable(
                name: "ShippingCost");

            migrationBuilder.DropTable(
                name: "ShippingState");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_MethodId",
                table: "Shippings");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_ShippingCostId",
                table: "Shippings");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_ShippingStateId",
                table: "Shippings");

            migrationBuilder.DropColumn(
                name: "MethodId",
                table: "Shippings");

            migrationBuilder.RenameColumn(
                name: "ShippingStateId",
                table: "Shippings",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ShippingCostId",
                table: "Shippings",
                newName: "Method");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Shippings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
