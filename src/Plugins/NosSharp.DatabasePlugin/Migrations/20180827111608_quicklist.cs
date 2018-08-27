using Microsoft.EntityFrameworkCore.Migrations;

namespace NosSharp.DatabasePlugin.Migrations
{
    public partial class quicklist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "EnumType",
                table: "quicklist",
                newName: "Slot");

            migrationBuilder.AddColumn<short>(
                name: "Morph",
                table: "quicklist",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<byte>(
                name: "Q1",
                table: "quicklist",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Q2",
                table: "quicklist",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Morph",
                table: "quicklist");

            migrationBuilder.DropColumn(
                name: "Q1",
                table: "quicklist");

            migrationBuilder.DropColumn(
                name: "Q2",
                table: "quicklist");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "quicklist",
                newName: "RelatedSlot");

            migrationBuilder.RenameColumn(
                name: "Slot",
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
    }
}
