using System;
using System.Collections.Generic;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Game.Entities.Drop;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Entities;
using ChickenAPI.Packets.ServerPackets.Minimap;
using ChickenAPI.Packets.ServerPackets.Visibility;

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
                VisualId = monster.MapMonster.Id,
                PositionX = monster.Position.X,
                PositionY = monster.Position.Y,
                Direction = (byte?)monster.DirectionType,
                InNonPlayerSubPacket = new InNonPlayerSubPacket
                {
                    Dialog = 0,
                    Faction = 0,
                    GroupEffect = 0,
                    InAliveSubPacket = new InAliveSubPacket
                    {
                        Hp = monster.HpPercentage,
                        Mp = monster.MpPercentage,
                    },
                    Morph = 0,
                    Skill1 = 0,
                    Skill2 = 0,
                    Skill3 = 0,
                    SkillRank1 = 0,
                    SkillRank2 = 0,
                    SkillRank3 = 0,
                    Unknow1 = 0,
                    Unknow3 = 0,
                    Unknow4 = 0,
                    Name = npcMonster.Id.ToString(),
                    IsSitting = false,
                    SpawnEffect = SpawnEffectType.Summon,
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

                case IDropEntity drop:
                    return GenerateInDrop(drop);

                default:
                    return null;
            }
        }

        public static OutPacket GenerateOutPacket(this IEntity entity) => new OutPacket
        {
            VisualType = entity.Type,
            VisualId = entity.Id
        };

        private static InPacket GenerateInMate(IMateEntity mate) =>
            new InPacket
            {
                VisualType = VisualType.Npc,
                Name = mate.Mate.NpcMonsterId.ToString(),
                VisualId = mate.Id,
                PositionX = mate.Position.X,
                PositionY = mate.Position.Y,
                Direction = (byte?)mate.DirectionType,
                /*
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
                */
            };

        private static InPacket GenerateInNpc(INpcEntity npcEntity) =>
            new InPacket
            {
                VisualType = VisualType.Npc,
                Name = npcEntity.NpcMonster.Id.ToString(),
                VisualId = npcEntity.Id,
                PositionX = npcEntity.Position.X,
                PositionY = npcEntity.Position.Y,
                VNum = npcEntity.NpcMonster.Id.ToString(),
                Direction = (byte?)npcEntity.DirectionType,
            };

        private static InPacket GenerateInDrop(IDropEntity drop) =>
            new InPacket
            {
                VisualType = drop.Type,
                Name = drop.ItemVnum.ToString(),
                VisualId = drop.Id,
                PositionX = drop.Position.X,
                PositionY = drop.Position.Y,
            };

        private static InPacket GenerateInPlayer(IPlayerEntity player) =>
            new InPacket
            {
                VisualType = VisualType.Player,
                Name = player.Character.Name,
                VNum = "-",
                VisualId = player.Id,
                PositionX = player.Position.X,
                PositionY = player.Position.Y,
                Direction = (byte?)player.DirectionType,
                InCharacterSubPacket = new InCharacterSubPacket()
                {
                    Authority = (byte)player.NameAppearance,
                    Gender = player.Character.Gender,
                    HairStyle = player.Character.HairStyle,
                    HairColor = player.Character.HairColor,
                    Class = player.Character.Class,
                    Equipment = player.Inventory.GenerateEqListInfoPacket(),
                    InAliveSubPacket = new InAliveSubPacket { Hp = player.HpPercentage, Mp = player.MpPercentage },
                    IsSitting = false,
                    GroupId = -1,
                    Fairy = (byte)(player.Fairy != null ? 4 : 0),
                    FairyElement = (byte)(player.Fairy?.ElementType ?? 0),
                    Unknown = 0,
                    Morph = (byte)player.MorphId,
                    Unknown2 = 0,
                    WeaponUpgradeRareSubPacket = player.Inventory.GenerateEqRareInfoPacket(EquipmentType.MainWeapon),
                    ArmorUpgradeRareSubPacket = player.Inventory.GenerateEqRareInfoPacket(EquipmentType.Armor),
                    FamilyId = player.HasFamily ? player.Family.Id : -1,
                    FamilyName = player.HasFamily ? player.Family.Name : "-", // if not put -1
                    Faction = FactionType.Neutral, // todo faction system
                    ReputIco = (short)player.GetReputIcon(),
                    Invisible = player.IsInvisible,
                    Level = player.Level,
                    FamilyLevel = player.Family?.FamilyLevel ?? 0,
                    ArenaWinner = player.Character.ArenaWinner,
                    Compliment = player.Character.Compliment,
                    Size = 10,
                    HeroLevel = player.HeroLevel,
                    FamilyIcons = new List<bool> { false, false, false },
                    MorphUpgrade = player.Sp?.Upgrade ?? 0,
                    MorphUpgrade2 = player.Sp?.Design ?? 0,
                    Unknown3 = 0
                }
            };

        public static AtPacket GenerateAtPacket(this IPlayerEntity player) =>
            new AtPacket
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