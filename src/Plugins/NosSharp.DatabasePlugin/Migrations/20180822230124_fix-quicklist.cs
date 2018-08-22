using Microsoft.EntityFrameworkCore.Migrations;

namespace NosSharp.DatabasePlugin.Migrations
{
    public partial class fixquicklist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Morph",
                table: "quicklist");

            migrationBuilder.DropColumn(
                name: "Pos",
                table: "quicklist");

            migrationBuilder.DropColumn(
                name: "Q1",
                table: "quicklist");

            migrationBuilder.DropColumn(
                name: "Elo",
                table: "character");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "quicklist",
                newName: "RelatedSlot");

            migrationBuilder.RenameColumn(
                name: "Slot",
                table: "quicklist",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "Q2",
                table: "quicklist",
                newName: "EnumType");

            migrationBuilder.AddColumn<bool>(
                name: "IsQ1",
                table: "quicklist",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSkill",
                table: "quicklist",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsQ1",
                table: "quicklist");

            migrationBuilder.DropColumn(
                name: "IsSkill",
                table: "quicklist");

            migrationBuilder.RenameColumn(
                name: "RelatedSlot",
                table: "quicklist",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "quicklist",
                newName: "Slot");

            migrationBuilder.RenameColumn(
                name: "EnumType",
                table: "quicklist",
                newName: "Q2");

            migrationBuilder.AddColumn<short>(
                name: "Morph",
                table: "quicklist",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Pos",
                table: "quicklist",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Q1",
                table: "quicklist",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "Elo",
                table: "character",
                nullable: false,
                defaultValue: 0);
        }
    }
}
