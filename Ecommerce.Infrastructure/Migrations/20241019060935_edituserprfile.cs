using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edituserprfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Usermane",
                table: "UserProfiles",
                newName: "Username");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "UserProfiles",
                newName: "Usermane");
        }
    }
}
