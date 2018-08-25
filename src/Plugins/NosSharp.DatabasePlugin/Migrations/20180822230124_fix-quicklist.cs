using Microsoft.EntityFrameworkCore.Migrations;

namespace NosSharp.DatabasePlugin.Migrations
{
    public partial class fixquicklist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Morph",
                "quicklist");

            migrationBuilder.DropColumn(
                "Pos",
                "quicklist");

            migrationBuilder.DropColumn(
                "Q1",
                "quicklist");

            migrationBuilder.DropColumn(
                "Elo",
                "character");

            migrationBuilder.RenameColumn(
                "Type",
                "quicklist",
                "RelatedSlot");

            migrationBuilder.RenameColumn(
                "Slot",
                "quicklist",
                "Position");

            migrationBuilder.RenameColumn(
                "Q2",
                "quicklist",
                "EnumType");

            migrationBuilder.AddColumn<bool>(
                "IsQ1",
                "quicklist",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "IsSkill",
                "quicklist",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "IsQ1",
                "quicklist");

            migrationBuilder.DropColumn(
                "IsSkill",
                "quicklist");

            migrationBuilder.RenameColumn(
                "RelatedSlot",
                "quicklist",
                "Type");

            migrationBuilder.RenameColumn(
                "Position",
                "quicklist",
                "Slot");

            migrationBuilder.RenameColumn(
                "EnumType",
                "quicklist",
                "Q2");

            migrationBuilder.AddColumn<short>(
                "Morph",
                "quicklist",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                "Pos",
                "quicklist",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                "Q1",
                "quicklist",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                "Elo",
                "character",
                nullable: false,
                defaultValue: 0);
        }
    }
}