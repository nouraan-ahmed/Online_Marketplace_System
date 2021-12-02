using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_MarketPlace_System.Migrations
{
    public partial class adduserid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User_Id",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_User_Id",
                table: "Product",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_User_User_Id",
                table: "Product",
                column: "User_Id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_User_User_Id",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_User_Id",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "Product");
        }
    }
}
