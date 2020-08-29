using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreMVC.Migrations
{
    public partial class Order_ClientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Order",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Order");

        }
    }
}
