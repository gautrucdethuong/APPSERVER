using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoleBasedAuthorization.Migrations
{
    public partial class addTableReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(maxLength: 2048, nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    Sent = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_user_id",
                table: "Reviews",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
