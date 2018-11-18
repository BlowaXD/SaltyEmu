using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Packets.Game.Server.Entities;
using ChickenAPI.Packets.Game.Server.MiniMap;
using ChickenAPI.Packets.Game.Server.Visibility;
using System;

namespace ChickenAPI.Game.PacketHandling.Extensions
{
    public static class VisibleEntityExtensions
    {
        private static InPacket GenerateInMonster(IMonsterEntity monster)
        {
            NpcMonsterDto npcMonster = monster.NpcMonster;
            MovableComponent movable = monster.Movable;
            return new InPacket
            {
                VisualType = VisualType.Monster,
                Name = npcMonster.Id.ToString(),
                TransportId = monster.MapMonster.Id.ToString(),
                PositionX = movable.Actual.X,
                PositionY = movable.Actual.Y,
                DirectionType = movable.DirectionType,
                InMonsterSubPacket = new InMonsterSubPacket
                {
                    HpPercentage = monster.HpPercentage,
                    MpPercentage = monster.MpPercentage,
                    Unknown1 = 0,
                    Unknown2 = 0,
                    Unknown3 = 0,
                    Unknown4 = -1,
                    NoAggressiveIcon = npcMonster.NoAggresiveIcon,
                    Unknown5 = 0,
                    Unknown6 = -1,
                    Unknown7 = "-",
                    Unknown8 = 0,
                    Unknown9 = -1,
                    Unknown10 = 0,
                    Unknown11 = 0,
                    Unknown12 = 0,
                    Unknown13 = 0,
                    Unknown14 = 0,
                    Unknown15 = 0,
                    Unknown16 = 0,
                    Unknown17 = 0
                }
            };
        }

        public static InPacket GenerateInPacket(this IEntity entity)
        {
            switch (entity)
            {
                case IMonsterEntity monster:
                    return GenerateInMonster(monster);
                case IPlayerEntity player:
                    return GenerateInPlayer(player);
                case INpcEntity npc:
                    return GenerateInNpc(npc);
                case ItemDropEntity drop:
                    return GenerateInDrop(drop);
                default:
                    return null;
            }
        }

        public static OutPacketBase GenerateOutPacket(this IPlayerEntity player) => new OutPacketBase
        {
            Type = VisualType.Character,
            EntityId = player.Character.Id
        };

        private static InPacket GenerateInNpc(INpcEntity npcEntity)
        {
            var npcMonster = npcEntity.GetComponent<NpcMonsterComponent>();
            MovableComponent movable = npcEntity.Movable;
            return new InPacket
            {
                VisualType = VisualType.Npc,
                Name = npcMonster.Vnum.ToString(),
                TransportId = npcMonster.MapNpcMonsterId.ToString(),
                PositionX = movable.Actual.X,
                PositionY = movable.Actual.Y,
                DirectionType = movable.DirectionType,
                InNpcSubPacket = new InNpcSubPacket
                {
                    HpPercentage = npcEntity.HpPercentage,
                    MpPercentage = npcEntity.MpPercentage,
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

        private static InPacket GenerateInDrop(IDropEntity drop)
        {
            return new InPacket
            {
                VisualType = drop.Type,
                Name = drop.ItemVnum.ToString(),
                TransportId = drop.Id.ToString(),
                PositionX = drop.Position.X,
                PositionY = drop.Position.Y,
                Amount = drop.Quantity,
                InDropSubPacket = new InItemSubPacketBase
                {
                    Unknown = 0,
                    Unknown1 = 0,
                    Unknown2 = 0,
                }
            };
        }

        private static InPacket GenerateInPlayer(IPlayerEntity player)
        {
            MovableComponent movable = player.Movable;
            return new InPacket
            {
                VisualType = VisualType.Character,
                Name = player.Character.Name,
                TransportId = "-",
                VNum = player.Character.Id,
                PositionX = movable.Actual.X,
                PositionY = movable.Actual.Y,
                DirectionType = movable.DirectionType,
                InCharacterSubPacket = new InCharacterSubPacketBase
                {
                    Authority = player.Session.Account.Authority > AuthorityType.GameMaster ? (byte)2 : (byte)0,
                    Gender = player.Character.Gender,
                    HairStyle = player.Character.HairStyle,
                    HairColor = player.Character.HairColor,
                    Class = player.Character.Class,
                    Equipment = player.GenerateInventoryWearPacket(),
                    HpPercentage = player.HpPercentage,
                    MpPercentage = player.MpPercentage,
                    IsSitting = false,
                    GroupId = -1,
                    FairyId = 0,
                    FairyElement = 0,
                    IsBoostedFairy = 0,
                    FairyMorph = 0,
                    EntryType = 0,
                    Morph = player.MorphId,
                    EquipmentRare = "00",
                    EquipmentRareTwo = "00",
                    FamilyId = player.HasFamily ? player.Family.Id : -1,
                    FamilyName = player.HasFamily ? player.Family.Name : "-", // if not put -1
                    ReputationIcon = 27,
                    Invisible = player.IsInvisible,
                    SpUpgrade = player.Sp?.Upgrade ?? 0,
                    Faction = FactionType.Neutral, // todo faction system
                    SpDesign = player.Sp?.Design ?? 0,
                    Level = player.Level,
                    FamilyLevel = 0,
                    ArenaWinner = player.Character.ArenaWinner,
                    Compliment = player.Character.Compliment,
                    Size = 10,
                    HeroLevel = player.HeroLevel
                }
            };
        }

        public static AtPacketBase GenerateAtPacket(this IPlayerEntity player)
        {
            return new AtPacketBase
            {
                CharacterId = player.Character.Id,
                MapId = Convert.ToInt16(player.CurrentMap.Map.Id),
                PositionX = player.Movable.Actual.X,
                PositionY = player.Movable.Actual.Y,
                Unknown1 = 2, // TODO: Find signification
                Unknown2 = 0, // TODO: Find signification
                Music = player.CurrentMap.Map.MusicId, //layer.Map.MusicId;
                Unknown3 = -1 // TODO: Find signification
            };
        }
    }
}