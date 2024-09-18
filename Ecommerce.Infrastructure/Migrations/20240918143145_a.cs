using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WishlistItems",
                table: "WishlistItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishlistItems",
                table: "WishlistItems",
                columns: new[] { "WishlistId", "ProductId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WishlistItems",
                table: "WishlistItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishlistItems",
                table: "WishlistItems",
                column: "WishlistId");
        }
    }
}
