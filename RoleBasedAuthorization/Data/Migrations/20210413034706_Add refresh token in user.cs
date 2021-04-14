using Microsoft.EntityFrameworkCore.Migrations;

namespace RoleBasedAuthorization.Migrations
{
    public partial class Addrefreshtokeninuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "user_refreshToken",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_refreshToken",
                table: "Users");
        }
    }
}
