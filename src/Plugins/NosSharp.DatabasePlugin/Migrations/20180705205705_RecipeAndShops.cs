using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NosSharp.DatabasePlugin.Migrations
{
    public partial class RecipeAndShops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_data_card",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    EffectId = table.Column<int>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    TimeoutBuff = table.Column<short>(nullable: false),
                    BuffType = table.Column<int>(nullable: false),
                    TimeoutBuffChance = table.Column<byte>(nullable: false),
                    Delay = table.Column<int>(nullable: false),
                    Propability = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_card", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_data_item",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    BasicUpgrade = table.Column<byte>(nullable: false),
                    CellonLvl = table.Column<byte>(nullable: false),
                    Class = table.Column<byte>(nullable: false),
                    CloseDefence = table.Column<short>(nullable: false),
                    Color = table.Column<byte>(nullable: false),
                    Concentrate = table.Column<short>(nullable: false),
                    CriticalLuckRate = table.Column<byte>(nullable: false),
                    CriticalRate = table.Column<short>(nullable: false),
                    DamageMaximum = table.Column<short>(nullable: false),
                    DamageMinimum = table.Column<short>(nullable: false),
                    DarkElement = table.Column<byte>(nullable: false),
                    DarkResistance = table.Column<short>(nullable: false),
                    DefenceDodge = table.Column<short>(nullable: false),
                    DistanceDefence = table.Column<short>(nullable: false),
                    DistanceDefenceDodge = table.Column<short>(nullable: false),
                    Effect = table.Column<short>(nullable: false),
                    EffectValue = table.Column<int>(nullable: false),
                    Element = table.Column<byte>(nullable: false),
                    ElementRate = table.Column<short>(nullable: false),
                    EquipmentSlot = table.Column<byte>(nullable: false),
                    FireElement = table.Column<byte>(nullable: false),
                    FireResistance = table.Column<short>(nullable: false),
                    Height = table.Column<byte>(nullable: false),
                    HitRate = table.Column<short>(nullable: false),
                    Hp = table.Column<short>(nullable: false),
                    HpRegeneration = table.Column<short>(nullable: false),
                    IsMinilandActionable = table.Column<bool>(nullable: false),
                    IsColored = table.Column<bool>(nullable: false),
                    Flag1 = table.Column<bool>(nullable: false),
                    Flag2 = table.Column<bool>(nullable: false),
                    Flag3 = table.Column<bool>(nullable: false),
                    Flag4 = table.Column<bool>(nullable: false),
                    Flag5 = table.Column<bool>(nullable: false),
                    Flag6 = table.Column<bool>(nullable: false),
                    Flag7 = table.Column<bool>(nullable: false),
                    Flag8 = table.Column<bool>(nullable: false),
                    IsConsumable = table.Column<bool>(nullable: false),
                    IsDroppable = table.Column<bool>(nullable: false),
                    IsHeroic = table.Column<bool>(nullable: false),
                    Flag9 = table.Column<bool>(nullable: false),
                    IsWarehouse = table.Column<bool>(nullable: false),
                    IsSoldable = table.Column<bool>(nullable: false),
                    IsTradable = table.Column<bool>(nullable: false),
                    ItemSubType = table.Column<byte>(nullable: false),
                    ItemType = table.Column<byte>(nullable: false),
                    ItemValidTime = table.Column<long>(nullable: false),
                    LevelJobMinimum = table.Column<byte>(nullable: false),
                    LevelMinimum = table.Column<byte>(nullable: false),
                    LightElement = table.Column<byte>(nullable: false),
                    LightResistance = table.Column<short>(nullable: false),
                    MagicDefence = table.Column<short>(nullable: false),
                    MaxCellon = table.Column<byte>(nullable: false),
                    MaxCellonLvl = table.Column<byte>(nullable: false),
                    MaxElementRate = table.Column<short>(nullable: false),
                    MaximumAmmo = table.Column<byte>(nullable: false),
                    MinilandObjectPoint = table.Column<int>(nullable: false),
                    MoreHp = table.Column<short>(nullable: false),
                    MoreMp = table.Column<short>(nullable: false),
                    Morph = table.Column<short>(nullable: false),
                    Mp = table.Column<short>(nullable: false),
                    MpRegeneration = table.Column<short>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<long>(nullable: false),
                    PvpDefence = table.Column<short>(nullable: false),
                    PvpStrength = table.Column<byte>(nullable: false),
                    ReduceOposantResistance = table.Column<short>(nullable: false),
                    ReputationMinimum = table.Column<byte>(nullable: false),
                    ReputPrice = table.Column<long>(nullable: false),
                    SecondaryElement = table.Column<byte>(nullable: false),
                    Sex = table.Column<byte>(nullable: false),
                    Speed = table.Column<byte>(nullable: false),
                    SpType = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    WaitDelay = table.Column<short>(nullable: false),
                    WaterElement = table.Column<byte>(nullable: false),
                    WaterResistance = table.Column<short>(nullable: false),
                    Width = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_data_map",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AllowShop = table.Column<bool>(nullable: false),
                    AllowPvp = table.Column<bool>(nullable: false),
                    Music = table.Column<int>(nullable: false),
                    Height = table.Column<short>(nullable: false),
                    Width = table.Column<short>(nullable: false),
                    Grid = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_map", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_data_npc_monster",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CantWalk = table.Column<bool>(nullable: false),
                    CanCollect = table.Column<bool>(nullable: false),
                    CantDebuff = table.Column<bool>(nullable: false),
                    CanCatch = table.Column<bool>(nullable: false),
                    CanRegenMp = table.Column<bool>(nullable: false),
                    CantVoke = table.Column<bool>(nullable: false),
                    CantTargetInfo = table.Column<bool>(nullable: false),
                    AmountRequired = table.Column<byte>(nullable: false),
                    AttackClass = table.Column<byte>(nullable: false),
                    AttackUpgrade = table.Column<byte>(nullable: false),
                    BasicArea = table.Column<byte>(nullable: false),
                    BasicCooldown = table.Column<short>(nullable: false),
                    BasicRange = table.Column<byte>(nullable: false),
                    BasicSkill = table.Column<short>(nullable: false),
                    CloseDefence = table.Column<short>(nullable: false),
                    Concentrate = table.Column<short>(nullable: false),
                    CriticalChance = table.Column<byte>(nullable: false),
                    CriticalRate = table.Column<short>(nullable: false),
                    DamageMaximum = table.Column<short>(nullable: false),
                    DamageMinimum = table.Column<short>(nullable: false),
                    DarkResistance = table.Column<short>(nullable: false),
                    DefenceDodge = table.Column<short>(nullable: false),
                    DefenceUpgrade = table.Column<byte>(nullable: false),
                    DistanceDefence = table.Column<short>(nullable: false),
                    DistanceDefenceDodge = table.Column<short>(nullable: false),
                    Element = table.Column<int>(nullable: false),
                    ElementRate = table.Column<short>(nullable: false),
                    FireResistance = table.Column<short>(nullable: false),
                    HeroLevel = table.Column<byte>(nullable: false),
                    HeroXp = table.Column<int>(nullable: false),
                    IsHostile = table.Column<bool>(nullable: false),
                    IsHostileGroup = table.Column<bool>(nullable: false),
                    JobXp = table.Column<int>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    LightResistance = table.Column<short>(nullable: false),
                    MagicDefence = table.Column<short>(nullable: false),
                    MaxHp = table.Column<int>(nullable: false),
                    MaxMp = table.Column<int>(nullable: false),
                    MonsterType = table.Column<int>(nullable: false),
                    NoAggresiveIcon = table.Column<bool>(nullable: false),
                    NoticeRange = table.Column<byte>(nullable: false),
                    Race = table.Column<byte>(nullable: false),
                    RaceType = table.Column<byte>(nullable: false),
                    RespawnTime = table.Column<int>(nullable: false),
                    Speed = table.Column<byte>(nullable: false),
                    VNumRequired = table.Column<short>(nullable: false),
                    WaterResistance = table.Column<short>(nullable: false),
                    Xp = table.Column<int>(nullable: false),
                    IsPercent = table.Column<bool>(nullable: false),
                    TakeDamages = table.Column<int>(nullable: false),
                    GiveDamagePercentage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_npc_monster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_data_skill",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    AttackAnimation = table.Column<short>(nullable: false),
                    CastAnimation = table.Column<short>(nullable: false),
                    CastEffect = table.Column<short>(nullable: false),
                    CastId = table.Column<short>(nullable: false),
                    CastTime = table.Column<short>(nullable: false),
                    Class = table.Column<byte>(nullable: false),
                    Cooldown = table.Column<short>(nullable: false),
                    CpCost = table.Column<byte>(nullable: false),
                    Duration = table.Column<short>(nullable: false),
                    Effect = table.Column<short>(nullable: false),
                    Element = table.Column<byte>(nullable: false),
                    HitType = table.Column<byte>(nullable: false),
                    ItemVNum = table.Column<short>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    LevelMinimum = table.Column<byte>(nullable: false),
                    MinimumAdventurerLevel = table.Column<byte>(nullable: false),
                    MinimumArcherLevel = table.Column<byte>(nullable: false),
                    MinimumMagicianLevel = table.Column<byte>(nullable: false),
                    MinimumSwordmanLevel = table.Column<byte>(nullable: false),
                    MpCost = table.Column<short>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    Range = table.Column<byte>(nullable: false),
                    SkillType = table.Column<byte>(nullable: false),
                    TargetRange = table.Column<byte>(nullable: false),
                    TargetType = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    UpgradeSkill = table.Column<short>(nullable: false),
                    UpgradeType = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_skill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Authority = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    RegistrationEmail = table.Column<string>(maxLength: 50, nullable: true),
                    RegistrationIp = table.Column<string>(maxLength: 50, nullable: true),
                    RegistrationToken = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_data_card_bcard",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubType = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    FirstData = table.Column<int>(nullable: false),
                    SecondData = table.Column<int>(nullable: false),
                    ThirdData = table.Column<int>(nullable: false),
                    CastType = table.Column<byte>(nullable: false),
                    IsLevelScaled = table.Column<bool>(nullable: false),
                    IsLevelDivided = table.Column<bool>(nullable: false),
                    CardId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_card_bcard", x => x.Id);
                    table.ForeignKey(
                        name: "FK__data_card_bcard__data_card_CardId",
                        column: x => x.CardId,
                        principalTable: "_data_card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_data_item_bcard",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubType = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    FirstData = table.Column<int>(nullable: false),
                    SecondData = table.Column<int>(nullable: false),
                    ThirdData = table.Column<int>(nullable: false),
                    CastType = table.Column<byte>(nullable: false),
                    IsLevelScaled = table.Column<bool>(nullable: false),
                    IsLevelDivided = table.Column<bool>(nullable: false),
                    ItemId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_item_bcard", x => x.Id);
                    table.ForeignKey(
                        name: "FK__data_item_bcard__data_item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "_data_item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "map_drop",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    DropChance = table.Column<int>(nullable: false),
                    TypedId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_drop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_map_drop__data_map_TypedId",
                        column: x => x.TypedId,
                        principalTable: "_data_map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "map_portals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<short>(nullable: false),
                    DestinationMapId = table.Column<long>(nullable: false),
                    DestinationX = table.Column<short>(nullable: false),
                    DestinationY = table.Column<short>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    SourceMapId = table.Column<long>(nullable: false),
                    SourceX = table.Column<short>(nullable: false),
                    SourceY = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_portals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_map_portals__data_map_SourceMapId",
                        column: x => x.SourceMapId,
                        principalTable: "_data_map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_data_npc_monster_bcard",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubType = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    FirstData = table.Column<int>(nullable: false),
                    SecondData = table.Column<int>(nullable: false),
                    ThirdData = table.Column<int>(nullable: false),
                    CastType = table.Column<byte>(nullable: false),
                    IsLevelScaled = table.Column<bool>(nullable: false),
                    IsLevelDivided = table.Column<bool>(nullable: false),
                    NpcMonsterId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_npc_monster_bcard", x => x.Id);
                    table.ForeignKey(
                        name: "FK__data_npc_monster_bcard__data_npc_monster_NpcMonsterId",
                        column: x => x.NpcMonsterId,
                        principalTable: "_data_npc_monster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_data_npc_monster_drops",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    DropChance = table.Column<int>(nullable: false),
                    TypedId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_npc_monster_drops", x => x.Id);
                    table.ForeignKey(
                        name: "FK__data_npc_monster_drops__data_npc_monster_TypedId",
                        column: x => x.TypedId,
                        principalTable: "_data_npc_monster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "map_monsters",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PositionX = table.Column<short>(nullable: false),
                    PositionY = table.Column<short>(nullable: false),
                    MapId = table.Column<long>(nullable: false),
                    NpcMonsterId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_monsters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_map_monsters__data_map_MapId",
                        column: x => x.MapId,
                        principalTable: "_data_map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_map_monsters__data_npc_monster_NpcMonsterId",
                        column: x => x.NpcMonsterId,
                        principalTable: "_data_npc_monster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "map_npcs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Dialog = table.Column<short>(nullable: false),
                    Effect = table.Column<short>(nullable: false),
                    EffectDelay = table.Column<short>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    IsMoving = table.Column<bool>(nullable: false),
                    IsSitting = table.Column<bool>(nullable: false),
                    MapId = table.Column<long>(nullable: false),
                    MapX = table.Column<short>(nullable: false),
                    MapY = table.Column<short>(nullable: false),
                    NpcMonsterId = table.Column<long>(nullable: false),
                    Direction = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_npcs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_map_npcs__data_map_MapId",
                        column: x => x.MapId,
                        principalTable: "_data_map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_map_npcs__data_npc_monster_NpcMonsterId",
                        column: x => x.NpcMonsterId,
                        principalTable: "_data_npc_monster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_data_npc_monster_skill",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SkillId = table.Column<long>(nullable: false),
                    Rate = table.Column<short>(nullable: false),
                    NpcMonsterId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_npc_monster_skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK__data_npc_monster_skill__data_npc_monster_NpcMonsterId",
                        column: x => x.NpcMonsterId,
                        principalTable: "_data_npc_monster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__data_npc_monster_skill__data_skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "_data_skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_data_skill_bcard",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubType = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    FirstData = table.Column<int>(nullable: false),
                    SecondData = table.Column<int>(nullable: false),
                    ThirdData = table.Column<int>(nullable: false),
                    CastType = table.Column<byte>(nullable: false),
                    IsLevelScaled = table.Column<bool>(nullable: false),
                    IsLevelDivided = table.Column<bool>(nullable: false),
                    SkillId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__data_skill_bcard", x => x.Id);
                    table.ForeignKey(
                        name: "FK__data_skill_bcard__data_skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "_data_skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "character",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Act4Dead = table.Column<int>(nullable: false),
                    Act4Kill = table.Column<int>(nullable: false),
                    Act4Points = table.Column<int>(nullable: false),
                    ArenaWinner = table.Column<int>(nullable: false),
                    Biography = table.Column<string>(nullable: true),
                    BuffBlocked = table.Column<bool>(nullable: false),
                    Class = table.Column<byte>(nullable: false),
                    Compliment = table.Column<short>(nullable: false),
                    Dignity = table.Column<float>(nullable: false),
                    Elo = table.Column<int>(nullable: false),
                    EmoticonsBlocked = table.Column<bool>(nullable: false),
                    ExchangeBlocked = table.Column<bool>(nullable: false),
                    Faction = table.Column<byte>(nullable: false),
                    FamilyRequestBlocked = table.Column<bool>(nullable: false),
                    FriendRequestBlocked = table.Column<bool>(nullable: false),
                    Gender = table.Column<byte>(nullable: false),
                    Gold = table.Column<long>(nullable: false),
                    GroupRequestBlocked = table.Column<bool>(nullable: false),
                    HairColor = table.Column<byte>(nullable: false),
                    HairStyle = table.Column<byte>(nullable: false),
                    HeroChatBlocked = table.Column<bool>(nullable: false),
                    HeroLevel = table.Column<byte>(nullable: false),
                    HeroXp = table.Column<long>(nullable: false),
                    Hp = table.Column<int>(nullable: false),
                    HpBlocked = table.Column<bool>(nullable: false),
                    JobLevel = table.Column<byte>(nullable: false),
                    JobLevelXp = table.Column<long>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    LevelXp = table.Column<long>(nullable: false),
                    MapId = table.Column<short>(nullable: false),
                    MapX = table.Column<short>(nullable: false),
                    MapY = table.Column<short>(nullable: false),
                    MasterPoints = table.Column<int>(nullable: false),
                    MasterTicket = table.Column<int>(nullable: false),
                    MaxMateCount = table.Column<byte>(nullable: false),
                    MinilandInviteBlocked = table.Column<bool>(nullable: false),
                    MinilandMessage = table.Column<string>(nullable: true),
                    MinilandPoint = table.Column<short>(nullable: false),
                    MinilandState = table.Column<byte>(nullable: false),
                    MouseAimLock = table.Column<bool>(nullable: false),
                    Mp = table.Column<int>(nullable: false),
                    Prefix = table.Column<string>(nullable: true),
                    QuickGetUp = table.Column<bool>(nullable: false),
                    RagePoint = table.Column<long>(nullable: false),
                    Reput = table.Column<long>(nullable: false),
                    Slot = table.Column<byte>(nullable: false),
                    SpAdditionPoint = table.Column<int>(nullable: false),
                    SpPoint = table.Column<int>(nullable: false),
                    State = table.Column<byte>(nullable: false),
                    TalentLose = table.Column<int>(nullable: false),
                    TalentSurrender = table.Column<int>(nullable: false),
                    TalentWin = table.Column<int>(nullable: false),
                    WhisperBlocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character", x => x.Id);
                    table.ForeignKey(
                        name: "FK_character_account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "map_npcs_shop",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MapNpcId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MenuType = table.Column<byte>(nullable: false),
                    ShopType = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_npcs_shop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_map_npcs_shop_map_npcs_MapNpcId",
                        column: x => x.MapNpcId,
                        principalTable: "map_npcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "character_item",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CharacterId = table.Column<long>(nullable: false),
                    BoundCharacterId = table.Column<long>(nullable: true),
                    ItemId = table.Column<long>(nullable: false),
                    Amount = table.Column<short>(nullable: false),
                    Slot = table.Column<short>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    Design = table.Column<byte>(nullable: false),
                    Rarity = table.Column<byte>(nullable: false),
                    Upgrade = table.Column<byte>(nullable: false),
                    Ammo = table.Column<byte>(nullable: false),
                    CloseDefense = table.Column<short>(nullable: false),
                    RangeDefense = table.Column<short>(nullable: false),
                    MagicDefense = table.Column<short>(nullable: false),
                    CloseDodge = table.Column<short>(nullable: false),
                    RangeDodge = table.Column<short>(nullable: false),
                    MagicDodge = table.Column<short>(nullable: false),
                    DamageMinimum = table.Column<short>(nullable: false),
                    DamageMaximum = table.Column<short>(nullable: false),
                    Concentration = table.Column<short>(nullable: false),
                    HitRate = table.Column<short>(nullable: false),
                    CriticalDodge = table.Column<short>(nullable: false),
                    CriticalRate = table.Column<short>(nullable: false),
                    CriticalDamageRate = table.Column<short>(nullable: false),
                    Cellon = table.Column<byte>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    Xp = table.Column<long>(nullable: false),
                    SpecialistUpgrade = table.Column<byte>(nullable: false),
                    SpecialistUpgrade2 = table.Column<byte>(nullable: false),
                    AttackPoints = table.Column<byte>(nullable: false),
                    DefensePoints = table.Column<byte>(nullable: false),
                    ElementPoints = table.Column<byte>(nullable: false),
                    HpMpPoints = table.Column<byte>(nullable: false),
                    Sum = table.Column<byte>(nullable: false),
                    ElementType = table.Column<byte>(nullable: false),
                    ElementRate = table.Column<short>(nullable: false),
                    Hp = table.Column<short>(nullable: false),
                    Mp = table.Column<short>(nullable: false),
                    FireResistance = table.Column<short>(nullable: false),
                    FirePower = table.Column<short>(nullable: false),
                    WaterResistance = table.Column<short>(nullable: false),
                    WaterPower = table.Column<short>(nullable: false),
                    LightResistance = table.Column<short>(nullable: false),
                    LightPower = table.Column<short>(nullable: false),
                    DarkResistance = table.Column<short>(nullable: false),
                    DarkPower = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_character_item_character_BoundCharacterId",
                        column: x => x.BoundCharacterId,
                        principalTable: "character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_item_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_character_item__data_item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "_data_item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "character_mate",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<long>(nullable: false),
                    Attack = table.Column<byte>(nullable: false),
                    CanPickUp = table.Column<bool>(nullable: false),
                    Defence = table.Column<byte>(nullable: false),
                    Direction = table.Column<byte>(nullable: false),
                    Experience = table.Column<long>(nullable: false),
                    Hp = table.Column<int>(nullable: false),
                    IsSummonable = table.Column<bool>(nullable: false),
                    IsTeamMember = table.Column<bool>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    Loyalty = table.Column<short>(nullable: false),
                    MapX = table.Column<short>(nullable: false),
                    MapY = table.Column<short>(nullable: false),
                    Mp = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NpcMonsterId = table.Column<long>(nullable: false),
                    Skin = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_mate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_character_mate_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_character_mate__data_npc_monster_NpcMonsterId",
                        column: x => x.NpcMonsterId,
                        principalTable: "_data_npc_monster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "character_skill",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CharacterId = table.Column<long>(nullable: false),
                    SkillId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_character_skill_character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_character_skill__data_skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "_data_skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shop_item",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Color = table.Column<byte>(nullable: false),
                    ItemId = table.Column<long>(nullable: false),
                    Rare = table.Column<short>(nullable: false),
                    ShopId = table.Column<long>(nullable: false),
                    Slot = table.Column<byte>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    Upgrade = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shop_item__data_item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "_data_item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shop_item_map_npcs_shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "map_npcs_shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shop_recipe",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<byte>(nullable: false),
                    ItemId = table.Column<long>(nullable: false),
                    ShopId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_recipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shop_recipe__data_item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "_data_item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shop_recipe_map_npcs_shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "map_npcs_shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shop_recipe_item",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<byte>(nullable: false),
                    ItemId = table.Column<long>(nullable: false),
                    RecipeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_recipe_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shop_recipe_item__data_item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "_data_item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shop_recipe_item_shop_recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "shop_recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX__data_card_bcard_CardId",
                table: "_data_card_bcard",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX__data_item_bcard_ItemId",
                table: "_data_item_bcard",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX__data_npc_monster_bcard_NpcMonsterId",
                table: "_data_npc_monster_bcard",
                column: "NpcMonsterId");

            migrationBuilder.CreateIndex(
                name: "IX__data_npc_monster_drops_TypedId",
                table: "_data_npc_monster_drops",
                column: "TypedId");

            migrationBuilder.CreateIndex(
                name: "IX__data_npc_monster_skill_NpcMonsterId",
                table: "_data_npc_monster_skill",
                column: "NpcMonsterId");

            migrationBuilder.CreateIndex(
                name: "IX__data_npc_monster_skill_SkillId",
                table: "_data_npc_monster_skill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX__data_skill_bcard_SkillId",
                table: "_data_skill_bcard",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_character_AccountId",
                table: "character",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_character_item_BoundCharacterId",
                table: "character_item",
                column: "BoundCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_character_item_CharacterId",
                table: "character_item",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_character_item_ItemId",
                table: "character_item",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_character_mate_CharacterId",
                table: "character_mate",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_character_mate_NpcMonsterId",
                table: "character_mate",
                column: "NpcMonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_character_skill_CharacterId",
                table: "character_skill",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_character_skill_SkillId",
                table: "character_skill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_map_drop_TypedId",
                table: "map_drop",
                column: "TypedId");

            migrationBuilder.CreateIndex(
                name: "IX_map_monsters_MapId",
                table: "map_monsters",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_map_monsters_NpcMonsterId",
                table: "map_monsters",
                column: "NpcMonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_map_npcs_MapId",
                table: "map_npcs",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_map_npcs_NpcMonsterId",
                table: "map_npcs",
                column: "NpcMonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_map_npcs_shop_MapNpcId",
                table: "map_npcs_shop",
                column: "MapNpcId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_map_portals_SourceMapId",
                table: "map_portals",
                column: "SourceMapId");

            migrationBuilder.CreateIndex(
                name: "IX_shop_item_ItemId",
                table: "shop_item",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_shop_item_ShopId",
                table: "shop_item",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_shop_recipe_ItemId",
                table: "shop_recipe",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_shop_recipe_ShopId",
                table: "shop_recipe",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_shop_recipe_item_ItemId",
                table: "shop_recipe_item",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_shop_recipe_item_RecipeId",
                table: "shop_recipe_item",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_data_card_bcard");

            migrationBuilder.DropTable(
                name: "_data_item_bcard");

            migrationBuilder.DropTable(
                name: "_data_npc_monster_bcard");

            migrationBuilder.DropTable(
                name: "_data_npc_monster_drops");

            migrationBuilder.DropTable(
                name: "_data_npc_monster_skill");

            migrationBuilder.DropTable(
                name: "_data_skill_bcard");

            migrationBuilder.DropTable(
                name: "character_item");

            migrationBuilder.DropTable(
                name: "character_mate");

            migrationBuilder.DropTable(
                name: "character_skill");

            migrationBuilder.DropTable(
                name: "map_drop");

            migrationBuilder.DropTable(
                name: "map_monsters");

            migrationBuilder.DropTable(
                name: "map_portals");

            migrationBuilder.DropTable(
                name: "shop_item");

            migrationBuilder.DropTable(
                name: "shop_recipe_item");

            migrationBuilder.DropTable(
                name: "_data_card");

            migrationBuilder.DropTable(
                name: "character");

            migrationBuilder.DropTable(
                name: "_data_skill");

            migrationBuilder.DropTable(
                name: "shop_recipe");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "_data_item");

            migrationBuilder.DropTable(
                name: "map_npcs_shop");

            migrationBuilder.DropTable(
                name: "map_npcs");

            migrationBuilder.DropTable(
                name: "_data_map");

            migrationBuilder.DropTable(
                name: "_data_npc_monster");
        }
    }
}
