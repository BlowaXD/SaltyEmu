using Microsoft.EntityFrameworkCore.Migrations;

namespace SaltyEmu.DatabasePlugin.Migrations
{
    public partial class ItemInstance_FixedMode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                table: "character_item",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFixed",
                table: "character_item");
        }
    }
}
