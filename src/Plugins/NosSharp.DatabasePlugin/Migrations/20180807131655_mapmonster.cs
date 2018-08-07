using Microsoft.EntityFrameworkCore.Migrations;

namespace NosSharp.DatabasePlugin.Migrations
{
    public partial class mapmonster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PositionY",
                table: "map_monsters",
                newName: "MapY");

            migrationBuilder.RenameColumn(
                name: "PositionX",
                table: "map_monsters",
                newName: "MapX");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "map_monsters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMoving",
                table: "map_monsters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "map_monsters",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "map_monsters");

            migrationBuilder.DropColumn(
                name: "IsMoving",
                table: "map_monsters");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "map_monsters");

            migrationBuilder.RenameColumn(
                name: "MapY",
                table: "map_monsters",
                newName: "PositionY");

            migrationBuilder.RenameColumn(
                name: "MapX",
                table: "map_monsters",
                newName: "PositionX");
        }
    }
}
