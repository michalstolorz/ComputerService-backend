using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerService.Data.Migrations
{
    public partial class AddDescritionCollumnRepairTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Repair",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Repair");
        }
    }
}
