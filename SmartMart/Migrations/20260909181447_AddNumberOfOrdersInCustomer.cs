using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMart.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfOrdersInCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "numberOfOrders",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numberOfOrders",
                table: "Customers");
        }
    }
}
