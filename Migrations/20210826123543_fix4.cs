using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorShop.Migrations
{
    public partial class fix4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentDetailsId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentDetailsId",
                table: "Orders",
                column: "PaymentDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentDetails_PaymentDetailsId",
                table: "Orders",
                column: "PaymentDetailsId",
                principalTable: "PaymentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentDetails_PaymentDetailsId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "PaymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentDetailsId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentDetailsId",
                table: "Orders");
        }
    }
}
