using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.Platform.Migrations
{
    public partial class UserImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageExtension",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageExtension",
                table: "AspNetUsers");
        }
    }
}
