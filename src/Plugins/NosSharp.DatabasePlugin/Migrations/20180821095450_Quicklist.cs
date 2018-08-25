using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NosSharp.DatabasePlugin.Migrations
{
    public partial class Quicklist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                "ElementType",
                "character_item",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.CreateTable(
                "quicklist",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CharacterId = table.Column<long>(nullable: false),
                    Morph = table.Column<short>(nullable: false),
                    Pos = table.Column<short>(nullable: false),
                    Q1 = table.Column<short>(nullable: false),
                    Q2 = table.Column<short>(nullable: false),
                    Slot = table.Column<short>(nullable: false),
                    Type = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quicklist", x => x.Id);
                    table.ForeignKey(
                        "FK_quicklist_character_CharacterId",
                        x => x.CharacterId,
                        "character",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_quicklist_CharacterId",
                "quicklist",
                "CharacterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "quicklist");

            migrationBuilder.AlterColumn<byte>(
                "ElementType",
                "character_item",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}