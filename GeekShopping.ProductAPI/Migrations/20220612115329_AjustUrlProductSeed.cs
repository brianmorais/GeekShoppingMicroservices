using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekShopping.ProductAPI.Migrations
{
    public partial class AjustUrlProductSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "id",
                keyValue: 12L,
                column: "image_url",
                value: "https://github.com/brianmorais/GeekShoppingMicroservices/blob/master/ShoppingImages/13_dragon_ball.jpg?raw=true");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "product",
                keyColumn: "id",
                keyValue: 12L,
                column: "image_url",
                value: "https://github.com/brianmorais/GeekShoppingMicroservices/blob/master/ShoppingImages/13_dragon_ball.jpg");
        }
    }
}
