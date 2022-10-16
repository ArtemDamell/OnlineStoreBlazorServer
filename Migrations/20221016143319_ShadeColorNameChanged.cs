using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorShop.Migrations
{
    public partial class ShadeColorNameChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShadeColor",
                table: "Products",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "ShadeColor");
        }
    }
}
