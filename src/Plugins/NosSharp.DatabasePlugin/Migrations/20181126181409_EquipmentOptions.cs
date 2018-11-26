using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SaltyEmu.DatabasePlugin.Migrations
{
    public partial class EquipmentOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_item_options",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    Value = table.Column<long>(nullable: false),
                    WearableInstanceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_item_options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_character_item_options_character_item_WearableInstanceId",
                        column: x => x.WearableInstanceId,
                        principalTable: "character_item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Experience = table.Column<long>(nullable: false),
                    FamilyHeadGender = table.Column<byte>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    FamilyMessage = table.Column<string>(maxLength: 255, nullable: true),
                    FamilyFaction = table.Column<byte>(nullable: false),
                    ManagerAuthorityType = table.Column<int>(nullable: false),
                    ManagerCanGetHistory = table.Column<bool>(nullable: false),
                    ManagerCanInvite = table.Column<bool>(nullable: false),
                    ManagerCanNotice = table.Column<bool>(nullable: false),
                    ManagerCanShout = table.Column<bool>(nullable: false),
                    MaxSize = table.Column<short>(nullable: false),
                    MemberAuthorityType = table.Column<int>(nullable: false),
                    MemberCanGetHistory = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    WarehouseSize = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterFamily",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Authority = table.Column<int>(nullable: false),
                    CharacterId = table.Column<long>(nullable: false),
                    DailyMessage = table.Column<string>(maxLength: 255, nullable: true),
                    Experience = table.Column<int>(nullable: false),
                    FamilyCharacterId = table.Column<long>(nullable: false),
                    FamilyId = table.Column<long>(nullable: false),
                    Rank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterFamily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterFamily_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterFamily_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_character_item_options_WearableInstanceId",
                table: "character_item_options",
                column: "WearableInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterFamily_CharacterId",
                table: "CharacterFamily",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterFamily_FamilyId",
                table: "CharacterFamily",
                column: "FamilyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_item_options");

            migrationBuilder.DropTable(
                name: "CharacterFamily");

            migrationBuilder.DropTable(
                name: "Families");
        }
    }
}
