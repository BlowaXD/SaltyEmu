using System;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Game.Server.Entities;
using ChickenAPI.Packets.Game.Server.MiniMap;
using ChickenAPI.Packets.Game.Server.Visibility;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class VisibleEntityExtensions
    {
        private static InPacket GenerateInMonster(IMonsterEntity monster)
        {
            NpcMonsterDto npcMonster = monster.NpcMonster;
            return new InPacket
            {
                VisualType = VisualType.Monster,
                Name = npcMonster.Id.ToString(),
                TransportId = monster.MapMonster.Id.ToString(),
                PositionX = monster.Position.X,
                PositionY = monster.Position.Y,
                DirectionType = monster.DirectionType,
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

                case IMateEntity mate:
                    return GenerateInMate(mate);

                case ItemDropEntity drop:
                    return GenerateInDrop(drop);

                default:
                    return null;
            }
        }

        public static OutPacket GenerateOutPacket(this IEntity entity) => new OutPacket
        {
            Type = entity.Type,
            EntityId = entity.Id
        };

        private static InPacket GenerateInMate(IMateEntity mate) =>
            new InPacket
            {
                VisualType = VisualType.Npc,
                Name = mate.Mate.NpcMonsterId.ToString(),
                TransportId = mate.Id.ToString(),
                PositionX = mate.Position.X,
                PositionY = mate.Position.Y,
                DirectionType = mate.DirectionType,
                InMateSubPacket = new InMateSubPacket
                {
                    HpPercentage = mate.HpPercentage,
                    MpPercentage = mate.MpPercentage,
                    Unknow = 0,
                    Faction = 0, // TODO: Faction syst ( In act4 owner.faction +2 )
                    Unknow2 = 3,
                    OwnerId = mate.Mate.CharacterId,
                    Unknow3 = 1,
                    Unknow4 = 0,
                    Morph = mate.Mate.Skin != 0 ? mate.Mate.Skin : -1, // (IsUsingSp && SpInstance != null ? SpInstance.Item.Morph : (Skin != 0 ? Skin : -1))
                    Name = mate.Mate.Name,
                    MateType = (byte)(mate.Mate.MateType + 1), // ( IsUsingSp && SpInstance != null ? 1 : MateType + 1 )
                    Unknow5 = 1,
                    Unknow6 = 0, // ( IsUsingSp && SpInstance != null ? 1 : 0 )
                    SPSkill1 = 0,
                    SPSkill2 = 0,
                    SPSkill3 = 0,
                    SPRank1 = 0, // If Rank = 7 { (SpInstance.SkillRank1 == 7 ? "4237" : "0")
                    SPRank2 = 0, // if rank = 7 (SpInstance.SkillRank1 == 7 ? "4238" : "0")
                    SPRank3 = 0 // xxx (SpInstance.SkillRank1 == 7 ? "4239" : "0")
                }
            };

        private static InPacket GenerateInNpc(INpcEntity npcEntity) =>
            new InPacket
            {
                VisualType = VisualType.Npc,
                Name = npcEntity.NpcMonster.Id.ToString(),
                TransportId = npcEntity.Id.ToString(),
                PositionX = npcEntity.Position.X,
                PositionY = npcEntity.Position.Y,
                DirectionType = npcEntity.DirectionType,
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

        private static InPacket GenerateInDrop(IDropEntity drop) =>
            new InPacket
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
                    Unknown2 = 0
                }
            };

        private static InPacket GenerateInPlayer(IPlayerEntity player) =>
            new InPacket
            {
                VisualType = VisualType.Character,
                Name = player.Character.Name,
                TransportId = "-",
                VNum = player.Character.Id,
                PositionX = player.Position.X,
                PositionY = player.Position.Y,
                DirectionType = player.DirectionType,
                InCharacterSubPacket = new InCharacterSubPacketBase
                {
                    NameAppearance = player.NameAppearance,
                    Gender = player.Character.Gender,
                    HairStyle = player.Character.HairStyle,
                    HairColor = player.Character.HairColor,
                    Class = player.Character.Class,
                    Equipment = player.GenerateInventoryWearPacket(),
                    HpPercentage = player.HpPercentage,
                    MpPercentage = player.MpPercentage,
                    IsSitting = false,
                    GroupId = -1,
                    FairyId = (byte)(player.Fairy != null ? 4 : 0),
                    FairyElement = (byte)(player.Fairy?.ElementType ?? 0),
                    IsBoostedFairy = 0,
                    FairyMorph = player.Fairy?.Item.Morph ?? 0,
                    EntryType = 0,
                    Morph = player.MorphId,
                    WeaponRareAndUpgradeInfo = $"{player.Weapon?.Rarity ?? 0}{player.Weapon?.Upgrade ?? 0}",
                    ArmorRareAndUpgradeInfo = $"{player.Armor?.Rarity ?? 0}{player.Armor?.Upgrade ?? 0}",
                    FamilyId = player.HasFamily ? player.Family.Id : -1,
                    FamilyName = player.HasFamily ? player.Family.Name : "-", // if not put -1
                    ReputationIcon = player.GetReputIcon(),
                    Invisible = player.IsInvisible,
                    SpUpgrade = player.Sp?.Upgrade ?? 0,
                    Faction = FactionType.Neutral, // todo faction system
                    SpDesign = player.Sp?.Design ?? 0,
                    Level = player.Level,
                    FamilyLevel = player.Family?.FamilyLevel ?? 0,
                    ArenaWinner = player.Character.ArenaWinner,
                    Compliment = player.Character.Compliment,
                    Size = 10,
                    HeroLevel = player.HeroLevel
                }
            };

        public static AtPacketBase GenerateAtPacket(this IPlayerEntity player) =>
            new AtPacketBase
            {
                CharacterId = player.Character.Id,
                MapId = Convert.ToInt16(player.CurrentMap.Map.Id),
                PositionX = player.Position.X,
                PositionY = player.Position.Y,
                Unknown1 = 2, // TODO: Find signification
                Unknown2 = 0, // TODO: Find signification
                Music = player.CurrentMap.Map.MusicId, //layer.Map.MusicId;
                Unknown3 = -1 // TODO: Find signification
            };
    }
}