using Microsoft.EntityFrameworkCore.Migrations;

namespace RoleBasedAuthorization.Migrations
{
    public partial class UpdatetbCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "product_image",
                table: "Carts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_image",
                table: "Carts");
        }
    }
}
