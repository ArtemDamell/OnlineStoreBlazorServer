using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorShop.Migrations
{
    public partial class AppointmensProductRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Appointments_AppointmentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AppointmentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "AppointmentProduct",
                columns: table => new
                {
                    AppointmentsId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentProduct", x => new { x.AppointmentsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_AppointmentProduct_Appointments_AppointmentsId",
                        column: x => x.AppointmentsId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentProduct_ProductsId",
                table: "AppointmentProduct",
                column: "ProductsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentProduct");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_AppointmentId",
                table: "Products",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Appointments_AppointmentId",
                table: "Products",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
