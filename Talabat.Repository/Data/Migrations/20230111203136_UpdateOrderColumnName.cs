using Microsoft.EntityFrameworkCore.Migrations;

namespace Talabat.Repository.Data.Migrations
{
    public partial class UpdateOrderColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayerEmail",
                table: "Orders",
                newName: "BuyerEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuyerEmail",
                table: "Orders",
                newName: "PayerEmail");
        }
    }
}
