using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Data.TransferObjects.NpcMonster;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Battle;
using ChickenAPI.Game.Features.Movement;
using ChickenAPI.Game.Maps;
using ChickenAPI.Packets.Game.Server.Entities;
using ChickenAPI.Packets.Game.Server.MiniMap;
using ChickenAPI.Packets.Game.Server._NotYetSorted;

namespace ChickenAPI.Game.Packets.Extensions
{
    public static class VisibleEntityExtensions
    {
        private static InPacketBase GenerateInMonster(IMonsterEntity monster)
        {
            BattleComponent battle = monster.Battle;
            NpcMonsterDto npcMonster = monster.NpcMonster;
            MovableComponent movable = monster.Movable;
            return new InPacketBase
            {
                VisualType = VisualType.Monster,
                Name = npcMonster.Id.ToString(),
                Unknown = monster.MapMonster.Id.ToString(),
                PositionX = movable.Actual.X,
                PositionY = movable.Actual.Y,
                DirectionType = movable.DirectionType,
                InMonsterSubPacket = new InMonsterSubPacket
                {
                    HpPercentage = battle.HpPercentage,
                    MpPercentage = battle.MpPercentage,
                    Unknown1 = 0,
                    Unknown2 = 0,
                    Unknown3 = -1,
                    NoAggressiveIcon = npcMonster.NoAggresiveIcon,
                    Unknown4 = 0,
                    Unknown5 = -1,
                    Unknown6 = "-",
                    Unknown7 = 0,
                    Unknown8 = -1,
                    Unknown9 = 0,
                    Unknown10 = 0,
                    Unknown11 = 0,
                    Unknown12 = 0,
                    Unknown13 = 0,
                    Unknown14 = 0,
                    Unknown15 = 0,
                    Unknown16 = 0
                }
            };
        }

        public static InPacketBase GenerateInPacket(this IEntity entity)
        {
            switch (entity)
            {
                case IMonsterEntity monster:
                    return GenerateInMonster(monster);
                case IPlayerEntity player:
                    return GenerateInPlayer(player);
                case INpcEntity npc:
                    return GenerateInNpc(npc);
                default:
                    return null;
            }
        }

        public static OutPacketBase GenerateOutPacket(this IPlayerEntity player) => new OutPacketBase
        {
            Type = VisualType.Character,
            EntityId = player.Character.Id
        };

        private static InPacketBase GenerateInNpc(INpcEntity npcEntity)
        {
            var npcMonster = npcEntity.GetComponent<NpcMonsterComponent>();
            MovableComponent movable = npcEntity.Movable;
            BattleComponent battle = npcEntity.Battle;

            return new InPacketBase
            {
                VisualType = VisualType.Npc,
                Name = npcMonster.Vnum.ToString(),
                Unknown = npcMonster.MapNpcMonsterId.ToString(),
                PositionX = movable.Actual.X,
                PositionY = movable.Actual.Y,
                DirectionType = movable.DirectionType,
                InNpcSubPacket = new InNpcSubPacket
                {
                    HpPercentage = battle.HpPercentage,
                    MpPercentage = battle.MpPercentage,
                    Dialog = npcEntity.MapNpc.Dialog,
                    Unknown1 = 0,
                    Unknown2 = 0,
                    Unknown3 = -1,
                    Unknown4 = 1,
                    IsSitting = false,
                    Unknown5 = -1,
                    Unknown6 = "-",
                    Unknown7 = 0,
                    Unknown8 = -1,
                    Unknown9 = 0,
                    Unknown10 = 0,
                    Unknown11 = 0,
                    Unknown12 = 0,
                    Unknown13 = 0,
                    Unknown14 = 0,
                    Unknown15 = 0,
                    Unknown16 = 0
                }
            };
        }

        private static InPacketBase GenerateInPlayer(IPlayerEntity player)
        {
            CharacterDto character = player.Character;
            MovableComponent movable = player.Movable;
            BattleComponent battle = player.Battle;
            return new InPacketBase
            {
                VisualType = VisualType.Character,
                Name = player.Character.Name,
                Unknown = "-",
                VNum = character.Id,
                PositionX = movable.Actual.X,
                PositionY = movable.Actual.Y,
                DirectionType = movable.DirectionType,
                InCharacterSubPacket = new InCharacterSubPacketBase
                {
                    Authority = player.Session.Account.Authority > AuthorityType.GameMaster ? (byte)2 : (byte)0,
                    Gender = character.Gender,
                    HairStyle = character.HairStyle,
                    HairColor = character.HairColor,
                    Class = character.Class,
                    Equipment = player.GenerateInventoryWearPacket(),
                    HpPercentage = battle.HpPercentage,
                    MpPercentage = battle.MpPercentage,
                    IsSitting = false,
                    GroupId = -1,
                    FairyId = 0,
                    FairyElement = 0,
                    IsBoostedFairy = 0,
                    FairyMorph = 0,
                    EntryType = 0,
                    Morph = 0,
                    EquipmentRare = "00",
                    EquipmentRareTwo = "00",
                    FamilyId = -1,
                    FamilyName = "-", // if not put -1
                    ReputationIcon = 27,
                    Invisible = !player.Visibility.IsVisible,
                    SpUpgrade = 0,
                    Faction = FactionType.Neutral, // todo faction system
                    SpDesign = 0,
                    Level = player.Experience.Level,
                    FamilyLevel = 0,
                    ArenaWinner = character.ArenaWinner,
                    Compliment = character.Compliment,
                    Size = 10,
                    HeroLevel = player.Experience.HeroLevel
                }
            };
        }

        public static AtPacketBase GenerateAtPacket(this IEntity entity)
        {
            if (!(entity is IPlayerEntity player))
            {
                // error, at packet should not be used for that player
                return null;
            }

            if (!(player.EntityManager is IMapLayer layer))
            {
                return null;
            }

            return new AtPacketBase
            {
                CharacterId = player.Character.Id,
                MapId = player.Character.MapId,
                PositionX = player.Movable.Actual.X,
                PositionY = player.Movable.Actual.Y,
                Unknown1 = 2, // TODO: Find signification
                Unknown2 = 0, // TODO: Find signification
                Music = layer.Map.MusicId, //layer.Map.MusicId;
                Unknown3 = -1 // TODO: Find signification
            };
        }
    }
}