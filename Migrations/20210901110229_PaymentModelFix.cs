using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorShop.Migrations
{
    public partial class PaymentModelFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PaymentPrice",
                table: "PaymentDetails",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "PayPalPaymentId",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayPalPaymentId",
                table: "PaymentDetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentPrice",
                table: "PaymentDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
