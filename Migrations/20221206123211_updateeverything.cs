using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_C.Migrations
{
    public partial class updateeverything : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
