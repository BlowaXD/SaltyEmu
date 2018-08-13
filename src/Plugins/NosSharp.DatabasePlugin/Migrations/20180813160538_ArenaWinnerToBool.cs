using Microsoft.EntityFrameworkCore.Migrations;

namespace NosSharp.DatabasePlugin.Migrations
{
    public partial class ArenaWinnerToBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ArenaWinner",
                table: "character",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ArenaWinner",
                table: "character",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
