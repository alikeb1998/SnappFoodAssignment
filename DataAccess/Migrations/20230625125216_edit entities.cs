using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_BuyerId",
                schema: "Assign",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                schema: "Assign",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BuyerId",
                schema: "Assign",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                schema: "Assign",
                table: "Orders",
                column: "UserId",
                principalSchema: "Assign",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                schema: "Assign",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Assign",
                table: "Orders",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                schema: "Assign",
                table: "Orders",
                newName: "IX_Orders_BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_BuyerId",
                schema: "Assign",
                table: "Orders",
                column: "BuyerId",
                principalSchema: "Assign",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
