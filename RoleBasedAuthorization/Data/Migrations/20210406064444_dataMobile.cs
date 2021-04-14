using Microsoft.EntityFrameworkCore.Migrations;

namespace RoleBasedAuthorization.Migrations
{
    public partial class dataMobile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthenticateModel",
                columns: table => new
                {
                    username = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticateModel", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    cart_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(nullable: false),
                    quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.cart_id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    product_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(nullable: false),
                    product_origin = table.Column<string>(nullable: false),
                    product_price = table.Column<double>(nullable: false),
                    product_description = table.Column<string>(nullable: false),
                    product_size = table.Column<string>(nullable: false),
                    product_rating = table.Column<string>(nullable: true),
                    product_image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.product_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_username = table.Column<string>(maxLength: 16, nullable: false),
                    user_password = table.Column<string>(maxLength: 50, nullable: false),
                    user_fullname = table.Column<string>(maxLength: 50, nullable: false),
                    user_email = table.Column<string>(maxLength: 32, nullable: false),
                    user_phone = table.Column<string>(maxLength: 15, nullable: false),
                    user_role = table.Column<string>(nullable: true),
                    user_token = table.Column<string>(nullable: true),
                    user_otp = table.Column<string>(nullable: true),
                    user_avt = table.Column<string>(nullable: true),
                    user_picture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticateModel");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
