using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editorderdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Shippers");

            migrationBuilder.AddColumn<int>(
                name: "AssignedOrdersCount",
                table: "Shippers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Shippers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ShipperId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipperId",
                table: "Orders",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_UserProfileId",
                table: "OrderDetails",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_UserProfiles_UserProfileId",
                table: "OrderDetails",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shippers_ShipperId",
                table: "Orders",
                column: "ShipperId",
                principalTable: "Shippers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_UserProfiles_UserProfileId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shippers_ShipperId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShipperId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_UserProfileId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "AssignedOrdersCount",
                table: "Shippers");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Shippers");

            migrationBuilder.DropColumn(
                name: "ShipperId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Shippers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
