using Microsoft.EntityFrameworkCore.Migrations;

namespace SaltyEmu.DatabasePlugin.Migrations
{
    public partial class ItemInstance_SpPerfection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecialistUpgrade2",
                table: "character_item",
                newName: "SpStoneUpgrade");

            migrationBuilder.AlterColumn<short>(
                name: "HpMpPoints",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<short>(
                name: "ElementPoints",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<short>(
                name: "DefensePoints",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<short>(
                name: "AttackPoints",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AddColumn<short>(
                name: "SpDamage",
                table: "character_item",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "SpDark",
                table: "character_item",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "SpDefence",
                table: "character_item",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "SpElement",
                table: "character_item",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "SpFire",
                table: "character_item",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "SpHP",
                table: "character_item",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "SpLight",
                table: "character_item",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "SpWater",
                table: "character_item",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpDamage",
                table: "character_item");

            migrationBuilder.DropColumn(
                name: "SpDark",
                table: "character_item");

            migrationBuilder.DropColumn(
                name: "SpDefence",
                table: "character_item");

            migrationBuilder.DropColumn(
                name: "SpElement",
                table: "character_item");

            migrationBuilder.DropColumn(
                name: "SpFire",
                table: "character_item");

            migrationBuilder.DropColumn(
                name: "SpHP",
                table: "character_item");

            migrationBuilder.DropColumn(
                name: "SpLight",
                table: "character_item");

            migrationBuilder.DropColumn(
                name: "SpWater",
                table: "character_item");

            migrationBuilder.RenameColumn(
                name: "SpStoneUpgrade",
                table: "character_item",
                newName: "SpecialistUpgrade2");

            migrationBuilder.AlterColumn<byte>(
                name: "HpMpPoints",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<byte>(
                name: "ElementPoints",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<byte>(
                name: "DefensePoints",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<byte>(
                name: "AttackPoints",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(short));
        }
    }
}
