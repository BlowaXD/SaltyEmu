using Microsoft.EntityFrameworkCore.Migrations;

namespace SaltyEmu.DatabasePlugin.Migrations
{
    public partial class ItemInstance_property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Rarity",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(byte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Rarity",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(short));
        }
    }
}
