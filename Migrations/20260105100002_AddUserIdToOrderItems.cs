using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CartItems",
                newName: "CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CartItems",
                newName: "UserId");
        }
    }
}
