using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NosSharp.DatabasePlugin.Migrations
{
    public partial class shops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shop_skill",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SkillId = table.Column<long>(nullable: false),
                    ShopId = table.Column<long>(nullable: false),
                    Slot = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shop_skill_map_npcs_shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "map_npcs_shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shop_skill__data_skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "_data_skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shop_skill_ShopId",
                table: "shop_skill",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_shop_skill_SkillId",
                table: "shop_skill",
                column: "SkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shop_skill");
        }
    }
}
