using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NosSharp.DatabasePlugin.Migrations
{
    public partial class Quicklist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ElementType",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.CreateTable(
                name: "quicklist",
                columns: table => new
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
                        name: "FK_quicklist_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_quicklist_CharacterId",
                table: "quicklist",
                column: "CharacterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quicklist");

            migrationBuilder.AlterColumn<byte>(
                name: "ElementType",
                table: "character_item",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
