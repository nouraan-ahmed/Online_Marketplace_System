using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_MarketPlace_System.Migrations
{
    public partial class mymigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User_Id",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentVM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardHolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<int>(type: "int", nullable: false),
                    ExpDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVcode = table.Column<int>(type: "int", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVM", x => x.Id);
                });

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

            migrationBuilder.DropTable(
                name: "PaymentVM");

            migrationBuilder.DropIndex(
                name: "IX_Product_User_Id",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "Product");
        }
    }
}
