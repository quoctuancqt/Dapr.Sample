using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderService.Infrastructure.Migrations
{
    public partial class Add_BuyerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                table: "Orders",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Orders");
        }
    }
}
